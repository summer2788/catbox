using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    public GameObject target;
    public GameObject border;
    public GameObject canvas2; //캔버스 객체

    public static bool cameraIsMove = true;
    public float smooth = 0.02f;

    private void Update()
    {
      

        if (cameraIsMove)
        {
            transform.position = new Vector3(transform.position.x,
                Mathf.Lerp(transform.position.y, target.transform.position.y + 2f, Time.deltaTime + smooth),  transform.position.z);
            
            if (transform.position.y >= target.transform.position.y + 1.6f)
            {
              
                cameraIsMove = false;
                border.transform.position = transform.position;
                canvas2.transform.position = transform.position;


            }




        }
      

    }


 
}
