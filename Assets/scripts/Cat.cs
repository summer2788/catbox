using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cat : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CapsuleCollider2D col;
    public static Cat Instan { get; private set; }
    public Camerafollow camerafollow;
    public Trajectory trajectory;
    public GameManager gameManager;
    public GameObject basicRotation;
    public GameObject nowInBox;
    public GameObject PerfectText; //perfecttext 표시
    public GameObject BounceText; //Bouncetext 표시
    public GameObject ScoreText; //일반 스코어 text 표시

    public GameObject SuperIn; //고양이 수퍼 모드 in
    public GameObject SuperOut; //고양이 수퍼 모드 out 

    public Transform perfectshow; // 점수 뜨는 위치 
    public Transform bounceshow; // 점수 뜨는 위치 
    public Transform scoreshow; // 점수 뜨는 위치 
    public GameObject canvas1;
    private Animator animator;
    Camera cam;

    public Vector3 screenposR; //화면 해상도 맞추기 위해서
    public Vector3 screenposL;
    private float left;
    private float right;
    private float middle;

    [HideInInspector] public Vector3 pos { get { return transform.position; } }
    public int boxDirection = 1;
    public int TotalScore; //추가될 최종 점수 
    public int PerfectCount = 0; //perfect 점수
    public int BounceCount = 1; //bounce 점수, x1, x2 로 나뉠거임
    public int bxindex; //현재 박스 종류
    private SpriteRenderer render;
    private bool isDead = false; //사망 상태
    public bool isperfect =true; //perfect 상태인지 체크
    public bool isJump = false; //점프 상태
    public bool isbasic = true; //생성된 박스가 normal 박스인지 체크
    public bool isBounce = false; //바운스 됐는 지 체크
    public bool noTouch = false; //일시정지창 켜짐->터치 불가



    bool isDragging = false;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 direction;
    Vector2 force;
    float distance;
    [SerializeField] float pushForce = 4f;

    void Awake()
    {
        if (Instan == null)
        {
            Instan = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        cam = Camera.main;
        animator = GetComponent<Animator>();

    }

    public void Start()
    {
       DesactivateRb();
        screenposR = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
        Debug.Log(screenposR.x);
        screenposL = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
        left = screenposL.x;
        right = screenposR.x;
        middle = (left + right) / 2;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (noTouch)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && isJump==false )
        {
            isDragging = true;
            OnDragStart(); //고양이 멈춤 
        }

        if (Input.GetMouseButtonUp(0) && isJump == false)
        {
            isDragging = false;
           

            OnDragEnd(); //고양이 발사
        }

        if (isDragging)
        {
            OnDrag(); //유도선 표시
        }

    }

    //---drag
    void OnDragStart()
    {
        DesactivateRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

       
    }
    void OnDrag()
    {

        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        //just for Debug
        Debug.DrawLine(startPoint, endPoint);

        //Debug.Log(distance);

        if(distance>1.2f)
        {
            trajectory.show();
            if (distance >= 3.7f)
            {
                trajectory.UpdateDots(pos, direction * 3.7f * pushForce);

            }
            else
            {
                trajectory.UpdateDots(pos, force);

            }

        }
        else
        {
            trajectory.Hide();
        }

        //상자와 고양이 방향 변환

        // Debug.Log(direction);
        //cat 과 박스의 rotation 변환
        float rotationVal = Vector2.Angle(direction, Vector2.up);
           //Debug.Log(rotationVal);
           if (endPoint.x < startPoint.x)
               rotationVal *= -1;
           nowInBox.transform.rotation = Quaternion.Euler(0, 0, rotationVal);
           transform.rotation = nowInBox.gameObject.transform.rotation;
        


    }
    void OnDragEnd()
    {

        if (distance > 1.2f)
        {
            if (distance >= 3.7f)
            {
                force = direction * 3.7f * pushForce;
            }
            //push the cat
            ActivateRb();
            Push(force);

            trajectory.Hide();
        }
       
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActivateRb()
    {
        rb.isKinematic = false;
    }

    public void DesactivateRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

   


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "dead" && !isDead)
        {
            Die();


        }
        // Add fishscore
        if (collision.gameObject.tag == "Fish")
        {
            collision.gameObject.SetActive(false);
            Scoremanager.Instance.AddFish();
            AudioControl.instarr.PlayAudio(AudioControl.instarr.fishSound);

        }



        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "oldbox" || collision.gameObject.tag == "startbox")
        {
            Camerafollow.cameraIsMove = true; //카메라 이동
            isJump = false;
            animator.SetBool("Jump", false);
            DesactivateRb();
            //고양이 위치 박스 가운데로 조정
            transform.position = collision.gameObject.transform.position + new Vector3(0.09f, 0.26f,0);




            //이전 박스 방향 초기화
            if (nowInBox != null)
                nowInBox.transform.rotation = basicRotation.transform.rotation;

            //현재 박스 방향 초기화->점수 올곧게 보이게
            collision.transform.rotation = basicRotation.transform.rotation;

           
            

            //고양이 회전값 변경
            transform.rotation = collision.gameObject.transform.rotation;



            if (!(collision.gameObject.tag == "oldbox")) //초기 박스나 생성된 박스이면 --> 새로운 박스 생성&점수 제공
            {
                if(collision.gameObject.tag == "Box")
                {
                   

                    //좌우나 상하 박스일때 움직임 멈추기
                    if (bxindex == 3)
                    {
                        collision.GetComponent<MoveBox>().Hide();
                    }
                    else if (bxindex == 4)
                    {
                        collision.GetComponent<UpandDown>().Hide();
                    }

                    ScoreCal(); //점수 제공

                    //점수 제공 해서 perfect Count 3이상일시 수퍼뚱냥이모드 On
                    if (PerfectCount >= 3)
                    {
                        SuperIn.SetActive(true);
                        SuperOut.SetActive(false);
                    }
                   
                   

                    //상자에 착지 성공 때마다 바운스 퍼펙트 초기화
                    isBounce = false;
                    isperfect = true;
                    if (nowInBox != null)
                    {
                        if (nowInBox.GetComponent<boxEnable>() != null)
                        {
                            if (nowInBox.GetComponent<boxEnable>().TimeOn == true)
                            {
                                //이전박스 타이머 소멸(캣이 시간 제한안에 골인)
                                nowInBox.GetComponent<boxEnable>().StopTime();
                            }


                        }

                        nowInBox.SetActive(false); //이전 박스 소멸

                    }




                   


                    // 타이머 작동
                    if (collision.GetComponent<boxEnable>().TimeSet == true)
                    {
                        collision.GetComponent<boxEnable>().TimeStart();
                    }
                    


                }

                //장애물 오브젝트 생성단계
                GameObject box;
                //점수별 분기-startbox 랑 box 일 때 -랜덤 박스  재배치
                if (Scoremanager.Instance.score >= 0 && Scoremanager.Instance.score < 20)
                {

                    // Create new basket
                    box = BoxpoolManager.Instace.GetPooledObject(0);
                    box.SetActive(false);
                    box.SetActive(true);
                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                        {
                            box.transform.position = new Vector2((left+middle)/2, transform.position.y + Random.Range(2f, 4f));
                            box.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 0));
                            box.SetActive(true);
                            boxDirection++;
                        }
                        else
                        {
                            //오른쪽
                            box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(2f, 4f));
                            box.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 30f));
                            box.SetActive(true);
                            boxDirection = 0;


                        }



                    }
                }

                else if (Scoremanager.Instance.score >= 20 && Scoremanager.Instance.score < 70)
                {

                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1, 2,3,4 }; //기본박스 1개, 공 박스 1개, 바 박스 1개, 좌우박스 1개, 상하 박스 1개 꼴로 비율 부여
                        int rdx = Random.Range(0, 5); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {
                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f*middle+0.3f*left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;

                                }
                                


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);
                               
                               
                            }
                            else
                            {
                                 //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                    box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);
                              
                            }
                        
                                   

                               
                        }
                    }
                }

                
                else if (Scoremanager.Instance.score >= 70 && Scoremanager.Instance.score < 150)

                {
                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1, 2, 3, 4 ,5}; //기본박스 1개, 공 박스 1개, 바 박스 1개, 좌우박스 1개, 상하 박스 1개, 회전 박스 1개 꼴로 비율 부여
                        int rdx = Random.Range(0, 6); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {

                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }



                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);

                            }




                        }
                    }

                    //타이머 미리 보여지기 

                    box.GetComponent<boxEnable>().TimeSetting(10);




                }

                else if (Scoremanager.Instance.score >= 150 && Scoremanager.Instance.score < 250)
                {
                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1, 2, 3, 4, 5 ,1,2}; //기본박스 1개, 공 박스 2개, 바 박스 2개, 좌우박스 1개, 상하 박스 1개, 회전 박스 1개 꼴로 비율 부여
                        int rdx = Random.Range(0, 8); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {
                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }



                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);

                            }




                        }
                    }

                    //타이머 미리 보여지기 

                    box.GetComponent<boxEnable>().TimeSetting(8);

                }

                else if (Scoremanager.Instance.score >= 250 && Scoremanager.Instance.score < 500)
                {

                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1, 2, 3,3, 4,4, 5,5 }; //기본박스 1개, 공 박스 1개, 바 박스 1개, 좌우박스 2개, 상하 박스 2개, 회전 박스 2개 꼴로 비율 부여
                        int rdx = Random.Range(0, 9); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {
                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }



                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);

                            }




                        }
                    }

                    //타이머 미리 보여지기 

                    box.GetComponent<boxEnable>().TimeSetting(6);
                }

                else if (Scoremanager.Instance.score >= 500 && Scoremanager.Instance.score < 1000)
                {
                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1,1,2, 2, 3, 3, 4, 4, 5, 5 }; //기본박스 1개, 공 박스 2개, 바 박스 2개, 좌우박스 2개, 상하 박스 2개, 회전 박스 2개 꼴로 비율 부여
                        int rdx = Random.Range(0, 11); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {
                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }



                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);

                            }




                        }
                    }

                    //타이머 미리 보여지기 

                    box.GetComponent<boxEnable>().TimeSetting(4);

                }

                else //1000점 이상
                {
                    if (isbasic == true)//현재 박스가 basic 박스라면 
                    {
                        int[] boxrandom = { 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 }; //기본박스 1개, 공 박스 2개, 바 박스 2개, 좌우박스 2개, 상하 박스 2개, 회전 박스 2개 꼴로 비율 부여
                        int rdx = Random.Range(0, 11); //인덱스 값
                        bxindex = boxrandom[rdx];
                        if (bxindex == 0)
                        {
                            isbasic = true; //생성될 박스가 basic
                        }
                        else
                        {
                            isbasic = false;
                        }

                        box = BoxpoolManager.Instace.GetPooledObject(bxindex);

                    }
                    else //현재 장애물 박스 임 , 생성될 박스는 기본 박스임
                    {
                        bxindex = 0;
                        // Create normal basket
                        box = BoxpoolManager.Instace.GetPooledObject(0);
                        isbasic = true;
                    }


                    // GameObject fish = box.transform.Find("fish").gameObject;

                    if (box != null)
                    {
                        if (isbasic == false) //생성 될 박스가 기본박스가 아니라면 -> 각도 수정은 안할 것임
                        {
                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 2.65f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * left, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }



                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                switch (bxindex)
                                {
                                    case 1: //공박스 일때 위치 
                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;


                                    case 2: //바 박스 일 때 위치
                                        box.transform.position = new Vector2(0.7f * middle + 0.3f * right, transform.position.y + Random.Range(1.5f, 2.1f));
                                        box.SetActive(true);

                                        break;

                                    case 3: //좌우 박스 일 때 위치
                                        box.transform.position = new Vector2(0, transform.position.y + Random.Range(2.3f, 3.8f));
                                        box.SetActive(true);

                                        break;

                                    case 4: //상하 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                        box.SetActive(true);

                                        break;
                                    case 5: //회전 박스 일 때 위치

                                        box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.2f));
                                        box.SetActive(true);

                                        break;

                                }

                            }

                        }
                        else //기본 박스라면 ->각도 수정
                        {


                            if (gameObject.transform.position.x > 0) //캣이 오른쪽에 있으면 박스 왼쪽에 생성
                            {
                                box.transform.position = new Vector2((left + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, -30f);
                                box.SetActive(true);


                            }
                            else
                            {
                                //캣이 왼쪽에 있으면 박스 오른쪽부근에 생성
                                box.transform.position = new Vector2((right + middle) / 2, transform.position.y + Random.Range(1.5f, 3.6f));
                                box.transform.rotation = Quaternion.Euler(0, 0, 30f);
                                box.SetActive(true);

                            }




                        }
                    }

                    //타이머 미리 보여지기 

                    box.GetComponent<boxEnable>().TimeSetting(3);
                }

                
                



                collision.gameObject.tag = "oldbox";
                    //render = collision.gameObject.transform.Find("edge").gameObject.GetComponent<SpriteRenderer>();
                    //render.material.color = Color.gray;
            }

            //nowINbox 변경
            nowInBox = collision.gameObject;
           

            //장애물 오브젝트 끄기
            if (nowInBox.name == "BoxBall(Clone)")
            {

                for (int i = 0; i < 3; i++)
                {
                    nowInBox.transform.GetChild(i).gameObject.SetActive(false);
                } //볼 끄기

            }
            else if (nowInBox.name == "BoxBar(Clone)")
            {

                for (int i = 0; i < 3; i++)
                {
                    nowInBox.transform.GetChild(i).gameObject.SetActive(false); //bar 끄기
                }

            }

            else if (nowInBox.name == "RotateBox(Clone)")
            {

                for (int i = 0; i < 2; i++)
                {
                    nowInBox.transform.GetChild(i).gameObject.SetActive(false); //bar 끄기
                }

            }



        }
       

       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Box" || collision.gameObject.tag == "oldbox" || collision.gameObject.tag == "startbox")
        {
            //점프 직동
            animator.SetBool("Jump", true);
            isJump = true;
            //점프 소리 작동
            AudioControl.instarr.PlayAudio(AudioControl.instarr.jumpSound);
            if (PerfectCount >= 3)
            {
                SuperOut.SetActive(true);
                SuperIn.SetActive(false);

                //수퍼 뚱냥이 오디오 재생
                AudioControl.instarr.PlayAudio(AudioControl.instarr.superJumpSound);
            }
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        AudioControl.instarr.PlayAudio(AudioControl.instarr.collision);

        if(collision.gameObject.tag == "notPerfect")
        {
            
            isperfect = false;
            Debug.Log(5);
            SuperIn.SetActive(false);
            SuperOut.SetActive(false);

        }

        if (collision.gameObject.tag == "bounce")
        {
          
            isBounce = true;
        }
        
    }




    private void Die()
    {
        DesactivateRb();
        isDead = true;

        gameManager.OnPlayerDead();
    }

    private void ScoreCal()  //점수 계산-> 보여지기 
    {
        if (isperfect)
        {
            PerfectCount++;
            GameObject perfect = Instantiate(PerfectText);
            perfect.transform.position = perfectshow.position;
            perfect.GetComponent<ScoreShow>().count = PerfectCount;
            AudioControl.instarr.PlayAudio(AudioControl.instarr.perfectScoreSound);
        }
        else
        {
            PerfectCount = 0;
            AudioControl.instarr.PlayAudio(AudioControl.instarr.normalScoreSound);
        }

        if (isBounce)
        {
            BounceCount = 2;
            GameObject bounce = Instantiate(BounceText);
            bounce.transform.position = bounceshow.position;
            AudioControl.instarr.PlayAudio(AudioControl.instarr.bounseSound);
        }
        else
        {
            BounceCount = 1;
        }

        TotalScore = (PerfectCount + 1) * BounceCount;


        GameObject score1 = Instantiate(ScoreText);
        score1.transform.position = scoreshow.position;
        score1.GetComponent<ShowScore>().count = TotalScore;
        Scoremanager.Instance.AddScore(TotalScore); //점수 증가

        

    }


}
