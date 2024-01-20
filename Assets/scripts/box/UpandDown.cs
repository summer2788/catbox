using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpandDown : MonoBehaviour
{
    [SerializeField] int dotsNumber; //serialize =인스펙터에서 접근 가능 but 외부 스크립트에서 접근 불가능
    [SerializeField] GameObject dotsParent;
    GameObject _dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float dotSpacing;
    [SerializeField] [Range(0, 1f)] float dotScale;
    Transform[] dotsList;

    public float startPoint;
    public float endPoint;
    public float distance;



    Vector3 pos; //현재위치
    public float delta = 2.0f; // 좌(우)로 이동가능한 (x)최대값 (-2~2까지 이동)
    [SerializeField] float speed = 3.0f; // 이동속도

    Vector2 dotpos;
    //dot pos
    float timestamp;
    IEnumerator coroutine;

    private void Awake()
    {
        _dotsParent = Instantiate(dotsParent, new Vector2(0, 0), Quaternion.identity);
    }
    //------------------------------------
    private void OnEnable()
    {
        pos = transform.position;
        //hide trajectory in the start
        Hide();
        ////prepae dots
        PrepareDots();
        coroutine = boxMove();
        
        StartCoroutine(coroutine);
        ////show dots
        show();
        startPoint = -delta;
        endPoint = delta;
        distance = endPoint-startPoint;


        UpdateDots(startPoint, endPoint);
    }


    public void Update()
    {

    }

    void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        dotPrefab.transform.localScale = Vector3.one * dotScale;

        float scale = dotScale;

        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = _dotsParent.transform;

            dotsList[i].localScale = Vector3.one * scale;

        }
    }

    public void UpdateDots(float firstPos, float endpos)
    {
        firstPos = transform.position.y + firstPos;
        for (int i = 0; i < dotsNumber; i++)
        {
            dotpos.y = (firstPos + distance / (dotsNumber - 1) * i);
            dotpos.x = transform.position.x;

            dotsList[i].position = dotpos;


        }

    }
    public void show()
    {
        _dotsParent.SetActive(true);
      
    }
    public void Hide()
    {
        _dotsParent.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator boxMove()
    {
        yield return null;

        while (true)
        {
            Vector3 v = pos;

            v.y += delta * Mathf.Sin(Time.time * speed);

            transform.position = v;
            yield return null;

        }
    }
}

