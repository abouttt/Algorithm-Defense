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
    private float curHP = 100f;


    private void Awake()
    {
        enemyBar.value = curHP / maxHP;
        enemyColor = enemyHealthTransform.GetComponent<Image>().color;

        castleColor = castleHealthTransform.GetComponent<Image>().color;

    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1");
           
            EnemyAttacked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            Debug.Log("2");
            CastleAttacked();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            curHP = 100f;
            enemyBar.value = curHP / maxHP;
        }

    }

    public void EnemyAttacked()
    {
        enemyBar.transform.DOGoto(0f, true);

        

        if (curHP > 0f)
        {
            curHP -= 10f;
        }
        else
        {
            curHP = 0f;
            //½Â¸®


        }

        enemyBar.value = curHP / maxHP;

        enemyHealthTransform.offsetMin = new Vector2(0f, 0f);
        enemyHealthTransform.offsetMax = new Vector2(0f, 0f);


        enemyBar.transform.DOShakePosition(0.2f, 5, 10, 90, true, false);
        enemyHealthTransform.GetComponent<Image>().DOColor(Color.white, 0.3f).OnComplete(() =>
        {
            enemyHealthTransform.GetComponent<Image>().DOColor(enemyColor, 0.3f);
       
            enemyHealthTransform.offsetMin = new Vector2(0f, 0f);
            enemyHealthTransform.offsetMax = new Vector2(0f, 0f);

            enemyBar.transform.localPosition = new Vector3(0f, 220f, 0f);
        });

    }


    public void CastleAttacked()
    {
        castleHealthTransform.DOGoto(0f, true);

        castleHealthTransform.DOShakePosition(0.2f, 5, 10, 90, true, false);
        castleHealthTransform.GetComponent<Image>().DOColor(Color.white, 0.3f).OnComplete(() =>
        {
            castleHealthTransform.GetComponent<Image>().DOColor(castleColor, 0.3f);
            castleHealthTransform.localPosition = new Vector3(0f, -225f, 0f);
        });
    }






}
