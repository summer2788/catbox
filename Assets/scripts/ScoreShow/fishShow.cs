using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishShow : MonoBehaviour
{
    int fishshow;

    private void OnEnable()
    {
        fishshow = Random.Range(0, 3);
        if (fishshow == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
