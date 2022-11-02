using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class HPBarAnimation : MonoBehaviour
{
    [SerializeField]
    private Slider enemyBar;
    [SerializeField]
    private RectTransform enemyHealthTransform;
    [SerializeField]
    private Slider castleBar;
    [SerializeField]
    private RectTransform castleHealthTransform;

    private Color enemyColor;
    private Color castleColor;

    private int starCount;
    private float collapsedEnemy;
    private float collapsedCastle;
    private bool gameClear=true;

    private void Awake()
    {
        enemyBar.value = Managers.Game.DungeonHP / Managers.Game.Setting.DungeonMaxHP;
        enemyColor = enemyHealthTransform.GetComponent<Image>().color;

        castleBar.value = Managers.Game.CastleHP / Managers.Game.Setting.CastleMaxHP;
        castleColor = castleHealthTransform.GetComponent<Image>().color;

        starCount = 3;
        collapsedEnemy = 0.66f;
        collapsedCastle = 0.66f;
        gameClear = false;
    }


    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Managers.Game.DungeonHP -= 10;
            Debug.Log(Managers.Game.DungeonHP);
            EnemyAttacked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Managers.Game.CastleHP -= 10;
            Debug.Log(Managers.Game.CastleHP);
            CastleAttacked();
        }

       
    }

    public void EnemyAttacked()
    {
        if (gameClear == false)
        {
            enemyBar.transform.DOGoto(0f, true);

            enemyBar.value = Managers.Game.DungeonHP / Managers.Game.Setting.DungeonMaxHP;

            if (enemyBar.value < collapsedEnemy)
            {
                //Debug.Log("적 건물 파괴");
                collapsedEnemy -= 0.33f;

                //적 건물 이미지 감소???

            }

            if (Managers.Game.DungeonHP <= 0f)
            {
                //승리        
                gameClear = true;
                ClearMenuAnimation.GetInstance.ClearMenuCall(starCount, true);
                StageTileInformation.GetInstance.GameClearSetStarCount(starCount);
            }

            enemyHealthTransform.offsetMin = new Vector2(0f, 0f);
            enemyHealthTransform.offsetMax = new Vector2(0f, 0f);


            enemyBar.transform.DOShakePosition(0.2f, 5, 10, 90, true, false);
            enemyHealthTransform.GetComponent<Image>().DOColor(Color.white, 0.3f).OnComplete(() =>
            {
                enemyHealthTransform.GetComponent<Image>().DOColor(enemyColor, 0.3f);

                enemyHealthTransform.offsetMin = new Vector2(0f, 0f);
                enemyHealthTransform.offsetMax = new Vector2(0f, 0f);

                enemyBar.transform.localPosition = new Vector3(0f, 250f, 0f);
            });
        }
    }


    public void CastleAttacked()
    {
        if (gameClear == false)
        {
            castleBar.transform.DOGoto(0f, true);

            castleBar.value = Managers.Game.CastleHP / Managers.Game.Setting.CastleMaxHP;

            if (castleBar.value < collapsedCastle)
            {
                Debug.Log("스타감소");
                starCount--;
                collapsedCastle -= 0.33f;

                //성벽 이미지 감소???

            }

            if (Managers.Game.CastleHP <= 0f)
            {
                //패배
                starCount = 0;
                gameClear = true;
                ClearMenuAnimation.GetInstance.ClearMenuCall(starCount, false);
                StageTileInformation.GetInstance.GameClearSetStarCount(starCount);
            }



            castleHealthTransform.offsetMin = new Vector2(0f, 0f);
            castleHealthTransform.offsetMax = new Vector2(0f, 0f);

            castleBar.transform.DOShakePosition(0.2f, 5, 10, 90, true, false);
            castleHealthTransform.GetComponent<Image>().DOColor(Color.white, 0.3f).OnComplete(() =>
            {
                castleHealthTransform.GetComponent<Image>().DOColor(castleColor, 0.3f);

                castleHealthTransform.offsetMin = new Vector2(0f, 0f);
                castleHealthTransform.offsetMax = new Vector2(0f, 0f);

                castleBar.transform.localPosition = new Vector3(0f, -250f, 0f);
            });
        }
    }






}