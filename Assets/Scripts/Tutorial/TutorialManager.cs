using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("[변수 초기화]")]
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private TextMeshProUGUI _tutorialText;
    [SerializeField]
    private Image _clickLimitImage;

    private TutorialBaseEvent _tutorialEvent;

    [SerializeField]
    private int _eventNumber = 0;
    private int _textIndex = 0;

    private Coroutine _textTypingEffectCoroutine;
    private bool _isShowAllText;

    private void Awake()
    {
        LoadingControl.GetInstance.LoadingCompleteAction += StartEvent;
    }

    private void Update()
    {
        if (_tutorialUI.activeSelf)
        {
            if (_tutorialEvent.IsFailureEvent)
            {
                ShowText(_tutorialEvent.FailedTextList);
            }
            else
            {
                ShowText(_tutorialEvent.TextList);
            }
        }
        else
        {
            _tutorialEvent.CheckEvent();

            if (_tutorialEvent.IsSuccessEvent || _tutorialEvent.IsFailureEvent)
            {
                if (_tutorialEvent.IsSuccessEvent)
                {
                    _eventNumber++;
                    StartEvent();
                }
                else if (_tutorialEvent.IsFailureEvent)
                {
                    OpenText();
                }
            }
        }
    }

    private void ShowText(List<string> textList)
    {
        if (_textIndex == _tutorialEvent.ShowGuideUIIndex)
        {
            _tutorialEvent.SetActiveGuideUI(true);
            var sprite = Managers.Resource.Load<Sprite>($"Textures/Tutorial/EventSprite_{_eventNumber}");
            if (sprite)
            {
                _clickLimitImage.sprite = sprite;
            }
            else
            {
                _clickLimitImage.sprite = Managers.Resource.Load<Sprite>($"Textures/Tutorial/EventSprite");
            }
        }
        else
        {
            _tutorialEvent.SetActiveGuideUI(false);
            _clickLimitImage.sprite = Managers.Resource.Load<Sprite>($"Textures/Tutorial/EventSprite");
        }

        if (!_isShowAllText && _textTypingEffectCoroutine == null)
        {
            _tutorialText.text = null;
            _textTypingEffectCoroutine = StartCoroutine(TextTypingEffect(textList[_textIndex].Replace("\\n", "\n"), 0.1f));
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_isShowAllText)
            {
                _textIndex++;
                _isShowAllText = false;
                _textTypingEffectCoroutine = null;

                if (_textIndex >= textList.Count)
                {
                    CloseText();
                    _tutorialEvent.StartEvent();
                    return;
                }
            }
            else
            {
                if (_textTypingEffectCoroutine != null)
                {
                    StopCoroutine(_textTypingEffectCoroutine);
                    _tutorialText.text = textList[_textIndex].Replace("\\n", "\n");
                    _isShowAllText = true;
                }
            }
        }
    }

    private IEnumerator TextTypingEffect(string text, float delayTime)
    {
        string codeText = null;
        bool isCodeTextTyping = false;

        foreach (char c in text)
        {
            if (c == '<')
            {
                isCodeTextTyping = true;
            }

            if (c == '>')
            {
                codeText += c;
                _tutorialText.text += codeText;
                codeText = null;
                isCodeTextTyping = false;
                continue;
            }

            if (isCodeTextTyping)
            {
                codeText += c;
                continue;
            }

            _tutorialText.text += c;
            yield return new WaitForSeconds(delayTime);
        }

        _isShowAllText = true;
    }

    private void StartEvent()
    {
        SetEvent();
        OpenText();
    }

    private void OpenText()
    {
        _tutorialUI.SetActive(true);
    }

    private void CloseText()
    {
        if (_tutorialEvent)
        {
            _tutorialEvent.IsSuccessEvent = false;
            _tutorialEvent.IsFailureEvent = false;
            _tutorialEvent.SetActiveGuideUI(false);
        }

        _textIndex = 0;
        _tutorialText.text = null;
        _tutorialUI.SetActive(false);
    }

    private void SetEvent()
    {
        if (_tutorialEvent)
        {
            Destroy(_tutorialEvent.gameObject);
        }

        var go = Managers.Resource.Instantiate($"Prefabs/Tutorial/TutorialEvent_{_eventNumber}");
        if (!go)
        {
            SceneManager.LoadScene("StartScene");
        }

        _tutorialEvent = go.GetComponent<TutorialBaseEvent>();
        _tutorialEvent.InitEvent();
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
