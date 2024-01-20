using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    IEnumerator coroutine;

    public float time;
    public Image img_fill;
    public TextMeshProUGUI TimeText;
    Color beta;
   

    void Start()
    {
        //rect = GetComponent<RectTransform>();
        
        coroutine = TimeAttack(time);
        show(); //타이머 보여지기
    }
    private void Update()
    {
        //rect.anchoredPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, Cat.Instan.gameObject.transform.position);
    }


    IEnumerator TimeAttack(float time)
    {
        float maxtime = time;
     
        img_fill.fillAmount = 0;
        while (time > 0)
        {
            time -= Time.deltaTime;
            img_fill.fillAmount += (Time.deltaTime / maxtime);
            yield return new WaitForFixedUpdate();
            TimeText.text = Mathf.Ceil(time).ToString();
        }
        img_fill.fillAmount = 1;
        TimeText.text = "0";
        Debug.Log("타임 어택 완료");
        Destroy(gameObject); //타임 만료됐으니 게임 오브젝트제거
    }

    public void show() //타이머 단순히 보여지기 
    {
  
        img_fill.fillAmount = 0;
        TimeText.text = time.ToString();

    }

    public void start() // 타이머 작동
    {
        StartCoroutine(coroutine);
    }
    public void stop()//공이 시간제한 안에 골인->타이머 중지 및 사라짐
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            Destroy(gameObject); //타이머 제거
        }
    }
}
