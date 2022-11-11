using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : MonoBehaviour
{
    [Header("[게임 초기 설정] - 씬 시작 시 1회만 동작한다.")]

    [Header("[카메라 설정]")]
    public float CameraX;
    public float CameraY;
    public float CameraZ;
    public float CameraSize;

    private Transform _contentsRoot;

    private void Awake()
    {
        Managers.Clear();

        InitCamera();
        InitContents();
        InitGround();

        LoadingControl.GetInstance.LoadingComplete();
    }

    private void InitCamera()
    {
        var camera = Camera.main;
        camera.transform.position = new Vector3(CameraX, CameraY, CameraZ);
        camera.orthographicSize = CameraSize;
    }

    private void InitContents()
    {
        _contentsRoot = Util.CreateGameObject("@Contens_Root").transform;

        if (!FindObjectOfType<TileManager>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@TileManager").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<MouseController>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@MouseController").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<RoadBuilder>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@RoadBuilder").transform.SetParent(_contentsRoot);
        }

        if (!FindObjectOfType<TutorialManager>())
        {
            Managers.Resource.Instantiate($"{Define.CONTENTS_PATH}@TutorialManager").transform.SetParent(_contentsRoot);
        }
    }

    private void InitGround()
    {
        var go = Managers.Resource.Instantiate($"{Define.GROUND_PREFAB_PATH}Ground_0");
        go.transform.position = new Vector3(5f, 5f, 0f);
        go.transform.SetParent(TileManager.GetInstance.GetGrid().transform);
    }
}
