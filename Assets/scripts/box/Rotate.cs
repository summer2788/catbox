using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    IEnumerator coroutine;

    public float turnSpeed = 10;
    // Start is called before the first frame update

    private void OnEnable()
    {
        coroutine = boxRotate();
        show();
    }
    // Update is called once per frame
    public void show()
    {
       
       
        StartCoroutine(coroutine);
    }
    public void Hide()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    IEnumerator boxRotate()
    {

        while (true)
        {
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
            yield return null;

        }
        
       

    }
}
