using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("[이벤트 0]")]
    [SerializeField]
    private Vector3Int _warriorCenterPos;


    
    [Header("[변수 초기화]")]
    [SerializeField]
    private TutorialTextList _tutorialTextList;
    [SerializeField]
    private GameObject _tutorialUI;
    [SerializeField]
    private TextMeshProUGUI _tutorialText;

    private int _eventNumber = 0;
    private int _textIndex = 0;
    private bool _isShowTutorialText;

    private void Update()
    {
        if (_isShowTutorialText)
        {
            ShowText();
        }
        else
        {
            CheckEvent();
        }

    }

    private void Start()
    {
        StartTutorial();
    }

    private void ShowText()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_eventNumber >= _tutorialTextList.TextList.Count)
            {
                SceneManager.LoadScene("StartScene");
            }

            _textIndex++;

            if (_textIndex >= _tutorialTextList.TextList[_eventNumber].TextList.Count)
            {
                EndTutorial();
            }

            _tutorialText.text = _tutorialTextList.TextList[_eventNumber].TextList[_textIndex];
        }
    }

    private void StartEvent()
    {
        
    }

    private void SetEvent()
    {
        if (_eventNumber == 0)
        {
            TileManager.GetInstance.SetTile(Define.Tilemap.Building, _warriorCenterPos, Define.Building.WarriorCenter);
        }
    }

    private void CheckEvent()
    {
        if (_eventNumber == 0)
        {
            if (RoadBuilder.GetInstance.GroupCount == 2)
            {
                Debug.Log("이벤트 0 성공.");
            }
        }
    }

    private void StartTutorial()
    {
        SetEvent();
        _tutorialUI.SetActive(true);
        _isShowTutorialText = true;
        _tutorialText.text = _tutorialTextList.TextList[_eventNumber].TextList[_textIndex];
    }

    private void EndTutorial()
    {
        _textIndex = 0;
        _isShowTutorialText = false;
        _tutorialUI.SetActive(false);
    }

}
