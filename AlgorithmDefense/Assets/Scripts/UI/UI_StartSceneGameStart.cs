using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StartSceneGameStart : MonoBehaviour
{
    [SerializeField]
    private Button gameStartButton;
    [SerializeField]
    private Button gameLodeButton;



    private void Start()
    {
        gameStartButton.onClick.AddListener(NewGameStart);
        gameLodeButton.onClick.AddListener(LodeGameStart);

    }


    public void NewGameStart()
    {

        SceneManager.LoadScene("GameScene");

    }

    public void LodeGameStart()
    {

        //����� ������ �ҷ�����

        SceneManager.LoadScene("GameScene");
    }
}
