using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class TutorialManager : MonoBehaviour
{
    [Header("[변수 초기화]")]
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private TextMeshProUGUI _tutorialText;

    private TutorialBaseEvent _tutorialEvent;

    [SerializeField]
    private int _eventNumber = 0;
    private int _textIndex = 0;

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
        if (string.IsNullOrEmpty(_tutorialText.text))
        {
            _tutorialText.text = textList[0].Replace("\\n", "\n");
        }

        if (_textIndex == _tutorialEvent.ShowGuideUIIndex)
        {
            _tutorialEvent.SetActiveGuideUI(true);
        }
        else
        {
            _tutorialEvent.SetActiveGuideUI(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            _textIndex++;

            if (_textIndex >= textList.Count)
            {
                CloseText();
                _tutorialEvent.StartEvent();
                return;
            }

            _tutorialText.text = textList[_textIndex].Replace("\\n", "\n"); ;
        }
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
