using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager s_instance;
    public static UIManager GetInstance { get { Init(); return s_instance; } }

    public GameObject GatewayUIController;

    private GameObject _currentShowUIController;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (GatewayUIController == null)
            Debug.Log("null");
    }

    public void ShowUIController(Define.Building building)
    {
        if (_currentShowUIController)
        {
            return;
        }

        if (building == Define.Building.Gateway)
        {
            GatewayUIController.SetActive(true);
            _currentShowUIController = GatewayUIController;
        }

        _currentShowUIController = GatewayUIController;
    }

    public void CloseUIController(Define.Building building)
    {
        if (!_currentShowUIController)
        {
            return;
        }

        if (building == Define.Building.Gateway)
        {
            _currentShowUIController.SetActive(false);
        }

        _currentShowUIController = null;
    }

    private static void Init()
    {
        if (!s_instance)
        {
            var go = GameObject.Find("@UIManager");
            if (!go)
            {
                go = new GameObject { name = "@UIManager" };
                go.AddComponent<UIManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<UIManager>();
        }
    }
}
