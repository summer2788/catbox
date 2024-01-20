using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftBorderSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
