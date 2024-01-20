using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSet : MonoBehaviour
{
    
    public GameObject[] obstacles;// 장애물 오브젝트들
    public int rdIndex; //활성화될 장애물 인덱스
    
    private void OnEnable()
    {

        if (Cat.Instan.gameObject.transform.position.x > 0) //고양이가 오른쪽에 있을때 -> 왼쪽 바 활성화 
        {
            rdIndex = Random.Range(0, 3);

            switch (rdIndex)
            {
                case 0:
                    //위 바 활성화
                    obstacles[0].SetActive(true);
                    //위에 바 위치 무작위
                    obstacles[0].transform.localPosition += new Vector3(0, Random.Range(0, 1.5f), 0);
                    obstacles[1].SetActive(false);
                    obstacles[2].SetActive(false);
                    break;

                case 1:
                    //완쪽 바 활성화

                    obstacles[1].SetActive(true);
                    obstacles[0].SetActive(false);
                    obstacles[2].SetActive(false);
                    break;

                case 2:
                    //완쪽 바 활성화

                    obstacles[1].SetActive(true);
                    obstacles[0].SetActive(false);
                    obstacles[2].SetActive(false);
                    break;
               
            }

        }
        else //반대
        {
            rdIndex = Random.Range(0, 3);

            switch (rdIndex)
            {
                case 0:
                    //위 바 활성화
                    obstacles[0].SetActive(true);
                    obstacles[0].transform.localPosition  += new Vector3(0, Random.Range(0, 1.5f), 0);
                    obstacles[1].SetActive(false);
                    obstacles[2].SetActive(false);
                    break;

                case 1:
                    //오른쪽 바 활성화

                    obstacles[2].SetActive(true);
                    obstacles[0].SetActive(false);
                    obstacles[1].SetActive(false);
                    break;

                case 2:
                    //오른쪽 바 활성화

                    obstacles[2].SetActive(true);
                    obstacles[0].SetActive(false);
                    obstacles[1].SetActive(false);
                    break;

            }
        }

        
        
    }
}
