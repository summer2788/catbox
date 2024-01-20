using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxpoolManager : MonoBehaviour
{
    public static BoxpoolManager Instace { get; private set; }

    public List<GameObject> pooledNormalObjects;
    public List<GameObject> pooledBallObjects;
    public List<GameObject> pooledBarObjects;
    public List<GameObject> pooledLRObjects; //LEFT RIGHT 움직이는 박스
    public List<GameObject> pooledUdObjects; //Up Down 움직이는 박스
    public List<GameObject> pooledRtObjects; //rotation 움직이는 박스
    public GameObject normalBox;
    public GameObject boxWithBall;
    public GameObject boxWithBar;
    public GameObject boxRL;
    public GameObject boxUd;
    public GameObject boxRotate;

    public int amountToPool;
    public int index0=0; //normal 박스 인덱스
    public int index1= 0; //공 박스 인덱스
    public int index2 = 0; //바 박스 인덱스
    public int index3 = 0; //좌우 박스 인덱스
    public int index4 = 0; //up down 박스 인덱스
    public int index5 = 0; //rotate 박스 인덱스


    void Awake()
    {
        Instace = this;
    }

    private void Start()
    {
        pooledNormalObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(normalBox,new Vector2(0,-25),Quaternion.identity);
            obj.SetActive(false);
           
            pooledNormalObjects.Add(obj);
        }
        // 공박스 생성
        pooledBallObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(boxWithBall,new Vector2(0,-30),Quaternion.identity);
            obj.SetActive(false);
            pooledBallObjects.Add(obj);
        }

        // 바 박스 생성
        pooledBarObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(boxWithBar, new Vector2(0, -30), Quaternion.identity);
            obj.SetActive(false);
            pooledBarObjects.Add(obj);
        }

        // 좌우 박스 생성
        pooledLRObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(boxRL, new Vector2(0, -30), Quaternion.identity);
            obj.SetActive(false);
            pooledLRObjects.Add(obj);
        }

        // 업다운 박스 생성
        pooledUdObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(boxUd, new Vector2(-30, -30), Quaternion.identity);
            obj.SetActive(false);
            pooledUdObjects.Add(obj);
        }

        // rotate 박스 생성
        pooledRtObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(boxRotate, new Vector2(-30, -30), Quaternion.identity);
            obj.SetActive(false);
            pooledRtObjects.Add(obj);
        }

        //GetPooledObject().SetActive(true);
    }

    public GameObject GetPooledObject(int boxnum) //boxnum= 박스 종류
    {
        switch (boxnum)
        {
            case 0: //기본박스
                int tempt = index0;
                index0++;
                if (index0 >= 10)
                {
                    index0 = 0;
                }

                return pooledNormalObjects[tempt];

            case 1: //공박스
                int tempt1 = index1;
                index1++;
                if (index1 >= 10)
                {
                    index1 = 0;
                }

                return pooledBallObjects[tempt1];
                
            case 2: //바박스
                int tempt2 = index2;
                index2++;
                if (index2 >= 10)
                {
                    index2 = 0;
                }

                return pooledBarObjects[tempt2];
              
            case 3: //좌우 무브박스

                int tempt3 = index3;
                index3++;
                if (index3 >= 10)
                {
                    index3 = 0;
                }

                return pooledLRObjects[tempt3];

            case 4: //업다운 무브박스

                int tempt4 = index4;
                index4++;
                if (index4 >= 10)
                {
                    index4 = 0;
                }

                return pooledUdObjects[tempt4];

            case 5: //rotate 박스

                int tempt5 = index5;
                index5++;
                if (index5 >= 10)
                {
                    index5 = 0;
                }

                return pooledRtObjects[tempt5];


        }


        return null;
        
    }
   
}
