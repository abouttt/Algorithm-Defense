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

    private float maxHP = 100f;
    private float enemyCurHP = 100f;
    private float castleCurHP = 100f;

    private int starCount;
    private float collapsedEnemy;
    private float collapsedCastle;
    private bool gameClear;

    private void Awake()
    {
        enemyBar.value = enemyCurHP / maxHP;
        enemyColor = enemyHealthTransform.GetComponent<Image>().color;

        castleBar.value = castleCurHP / maxHP;
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
            EnemyAttacked(10f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CastleAttacked(10f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            enemyCurHP = 100f;
            enemyBar.value = enemyCurHP / maxHP;

            castleCurHP = 100f;
            castleBar.value = castleCurHP / maxHP;
            gameClear = true;
        }

    }

    public void EnemyAttacked(float damage)
    {
        if (gameClear == false)
        {
            enemyBar.transform.DOGoto(0f, true);

            if (enemyCurHP > 0f)
            {
                enemyCurHP -= damage;
            }

            enemyBar.value = enemyCurHP / maxHP;

            if (enemyBar.value < collapsedEnemy)
            {
                Debug.Log("�� �ǹ� �ı�");
                collapsedEnemy -= 0.33f;

                //�� �ǹ� �̹��� ����???

            }

            if (enemyCurHP <= 0f)
            {
                enemyCurHP = 0f;
                //�¸�        
                gameClear = true;
                ClearMenuAnimation.GetInstance.ClearMenuCall(starCount, true);
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


    public void CastleAttacked(float damage)
    {
        if (gameClear == false)
        {
            castleBar.transform.DOGoto(0f, true);

            if (castleCurHP > 0f)
            {
                castleCurHP -= damage;
            }

            castleBar.value = castleCurHP / maxHP;

            if (castleBar.value < collapsedCastle)
            {
                Debug.Log("��Ÿ����");
                starCount--;
                collapsedCastle -= 0.33f;

                //���� �̹��� ����???

            }

            if (castleCurHP <= 0f)
            {
                castleCurHP = 0f;
                //�й�
                starCount = 0;
                gameClear = true;
                ClearMenuAnimation.GetInstance.ClearMenuCall(starCount, false);
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
