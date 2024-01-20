using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxEnable : MonoBehaviour
{

    public int rdIndex; //활성화될 장애물 인덱스
    public GameObject Timer; //타이머 프리펩
    public GameObject Timer1; //타이머 인스턴스
    public float time;
    public Transform timeshow; // 타이머 표시 위치
    public bool TimeSet; //타이머 보여지기 여부
    public bool TimeOn; //타이머 활성화 여부
    public float FadeTime; // Fade효과 재생시간

    float start;
    float end;
    float normaltime;

    private SpriteRenderer BoxBody;
    private SpriteRenderer Boxback;
    private SpriteRenderer Boxfrontline;
    public Color fadecolor;
    public Color fadecolor1;
    public Color fadecolor2;

    void Awake()

    {

        BoxBody = gameObject.transform.Find("BoxBody").gameObject.GetComponent<SpriteRenderer>();
        Boxback = gameObject.transform.Find("Boxback").gameObject.GetComponent<SpriteRenderer>();
        Boxfrontline = gameObject.transform.Find("Boxfrontline").gameObject.GetComponent<SpriteRenderer>();

        fadecolor = BoxBody.material.color;
        fadecolor1 = Boxback.material.color;
        fadecolor2 = Boxfrontline.material.color;
        normaltime = 0f;
        end = 0f;
        start = 1f;


    }

    public void OnEnable()
    {
        TimeSet = false; 
        TimeOn = false; //타이머 초기화
        gameObject.tag = "Box";
        //투명값 다시 초기화

        normaltime = 0f;

        BoxBody.material.color = Color.white;
        Boxback.material.color = Color.white;
        Boxfrontline.material.color = Color.white;

       




    }

    public void Update()
    {
        
            if (TimeSet)
            {
                if (Timer1 != null)
                {
                    //timer 위치 박스에 고정
                    Timer1.transform.position = timeshow.position;
                }


              

                if (TimeOn)
                {
                    if (Timer1 == null)
                    {
                        //타이머 만료되면 박스 제거 ->떨어져 죽음
                        gameObject.SetActive(false);
                        Cat.Instan.ActivateRb();
                    }
                    else
                    {
                        //타이머 지속되는 동안 ->박스 점점 투명하게 


                        //박스 투명부분 시작

                        if (fadecolor.a > 0f)
                        {

                            normaltime += (Time.deltaTime / time);

                            fadecolor.a = Mathf.Lerp(start, end, normaltime);
                            BoxBody.material.color = fadecolor;

                            fadecolor1.a = Mathf.Lerp(start, end, normaltime);
                            Boxback.material.color = fadecolor1;

                            fadecolor2.a = Mathf.Lerp(start, end, normaltime);
                            Boxfrontline.material.color = fadecolor2;

                        }

                    }

                }
            }
        
       
       
    }

        public void TimeStart() //타이머 작동 및 박스 색 변경
        {
            // time = time1;
            //Timer1 = Instantiate(Timer);
            //Timer1.transform.position = timeshow.position;
            //Timer1.transform.SetParent(Scoremanager.Instance.canvasshow.transform);
            //Timer1.GetComponent<Timer>().time = time;

             TimeOn = true;
             Timer1.GetComponent<Timer>().start();
       

        }

    public void TimeSetting(float time1) //타이머 Set
    {
        time = time1;
        TimeSet = true;
        Timer1 = Instantiate(Timer);
        Timer1.transform.position = timeshow.position;
        Timer1.transform.SetParent(Scoremanager.Instance.canvasshow.transform);
        Timer1.GetComponent<Timer>().time = time;

    }

    public void StopTime() //공이 시간제한 안에 골인->타이머 중지 및 박스 원래 색으로 
        {
            TimeSet = false;
            TimeOn = false;
            if(Timer1!=null)
                Timer1.GetComponent<Timer>().stop();
        }

    

}
      

