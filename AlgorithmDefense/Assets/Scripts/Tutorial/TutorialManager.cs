using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

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
    private int _errorTextIndex = 0;
    private bool _isShowTutorialText;
    private bool _isError;
    private bool _isEndedCheckEvent;

    private bool _canConnectCastle;

    private void Update()
    {
        if (_isShowTutorialText)
        {
            if (_isError)
            {
                ShowErrorText();
            }
            else
            {
                ShowText();
            }

        }
        else
        {
            CheckEvent();
        }

    }

    private void Start()
    {
        StartTutorial();
        RoadBuilder.GetInstance.ConnectedRoadDoneAction += CanConnectedCastle;
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
                return;
            }

            _tutorialText.text = _tutorialTextList.TextList[_eventNumber].TextList[_textIndex];
        }
    }

    private void ShowErrorText()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _errorTextIndex++;

            if (_errorTextIndex >= _tutorialTextList.ErrorTextList[_eventNumber].TextList.Count)
            {
                _isError = false;
                EndTutorial();
                return;
            }

            _tutorialText.text = _tutorialTextList.ErrorTextList[_eventNumber].TextList[_errorTextIndex];
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
        else if (_eventNumber == 1)
        {

        }
    }

    private void CheckEvent()
    {
        if (!_isEndedCheckEvent)
        {
            return;
        }

        if (_eventNumber == 0)
        {
            if (_canConnectCastle)
            {
                _eventNumber++;
            }
            else
            {
                _isError = true;
            }

            StartTutorial();
            _isEndedCheckEvent = false;
        }
        else if (_eventNumber == 1)
        {

        }
    }

    private void StartTutorial()
    {
        SetEvent();
        _tutorialUI.SetActive(true);
        _isShowTutorialText = true;
        if (_isError)
        {
            _tutorialText.text = _tutorialTextList.ErrorTextList[_eventNumber].TextList[0];
        }
        else
        {
            _tutorialText.text = _tutorialTextList.TextList[_eventNumber].TextList[0];
        }
    }

    private void EndTutorial()
    {
        _textIndex = 0;
        _errorTextIndex = 0;
        _isShowTutorialText = false;
        _tutorialUI.SetActive(false);
    }

    private void CanConnectedCastle()
    {
        bool[,] visited = new bool[Managers.Game.Setting.RampartHeight, Managers.Game.Setting.RampartWidth];
        Queue<Vector3Int> q = new();
        visited[_warriorCenterPos.y, _warriorCenterPos.x] = true;
        q.Enqueue(_warriorCenterPos);
        while (q.Count > 0)
        {
            Vector3Int pos = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int ny = pos.y + Define.DY[i];
                int nx = pos.x + Define.DX[i];

                if (ny < Managers.Game.Setting.StartPosition.y ||
                    nx < Managers.Game.Setting.StartPosition.x ||
                    ny > Managers.Game.Setting.StartPosition.y + Managers.Game.Setting.RampartHeight - 1 ||
                    nx > Managers.Game.Setting.StartPosition.x + Managers.Game.Setting.RampartWidth - 1)
                {
                    continue;
                }

                if (visited[ny, nx])
                {
                    continue;
                }

                var nextPos = new Vector3Int(nx, ny, 0);

                if (TileManager.GetInstance.GetTile(Define.Tilemap.Rampart, nextPos))
                {
                    continue;
                }

                if (TileManager.GetInstance.GetTile(Define.Tilemap.Road, nextPos))
                {
                    continue;
                }

                if (Util.GetBuilding<CastleGate>(nextPos))
                {
                    _canConnectCastle = true;
                    _isEndedCheckEvent = true;
                    return;
                }

                visited[ny, nx] = true;
                q.Enqueue(nextPos);
            }
        }

        _canConnectCastle = false;
        _isEndedCheckEvent = true;
    }

    public void OnClickExitButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
