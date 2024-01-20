using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSet : MonoBehaviour
{
    public GameObject[] obstacles;// 장애물 오브젝트들
 
    private void OnEnable()
    {

        if (Cat.Instan.gameObject.transform.position.x < 0) //고양이가 왼쪽에 있을때 -> 왼쪽 생선 활성화
        {

            //완쪽 볼 활성화

            obstacles[0].SetActive(true);
            obstacles[1].SetActive(false);




        }
        else //왼쪽일때 
        {


            //오른쪽 볼 활성화


            obstacles[0].SetActive(false);
            obstacles[1].SetActive(true);


        }



    }
}
