using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber; //serialize =인스펙터에서 접근 가능 but 외부 스크립트에서 접근 불가능
    [SerializeField] GameObject dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float dotSpacing;
    [SerializeField] [Range(0.01f,0.2f)]float dotMinScale;
    [SerializeField] [Range(0.2f, 1f)] float dotMaxScale;
    Transform[] dotsList;

    Vector2 pos;
    public Vector3 screenposR; //화면 해상도 맞추기 위해서
    public Vector3 screenposL;
    //dot pos
    float timestamp;

    //------------------------------------
    private void Start()
    {
        screenposR = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
        screenposL = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
        //hide trajectory in the start
        Hide();
        //prepae dots
        PrepareDots();
    }

    void PrepareDots()
    {
        Debug.Log(1);
        dotsList = new Transform[dotsNumber];
        dotPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for (int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;

            dotsList[i].localScale = Vector3.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }
    }

    public void UpdateDots(Vector3 catPos, Vector2 forceApplied)
    {
        timestamp = dotSpacing;
        for (int i=0; i<dotsNumber; i++)
        {
            pos.x = (catPos.x + forceApplied.x * timestamp);

            if (pos.x < screenposL.x)
            {
                pos.x = screenposL.x*2 - pos.x;
            }
            else if (pos.x > screenposR.x)
            {
                pos.x = screenposR.x*2 - pos.x;
            }
            else
            {
                pos.x = (catPos.x + forceApplied.x * timestamp);
            }    
            pos.y = (catPos.y + forceApplied.y * timestamp) - (Physics2D.gravity.magnitude * timestamp * timestamp) / 2f;

            dotsList[i].position = pos;
            timestamp += dotSpacing;
        }
    }
    public void show()
    {
        dotsParent.SetActive(true);
    }
    public void Hide()
    {
        dotsParent.SetActive(false);
    }
}
