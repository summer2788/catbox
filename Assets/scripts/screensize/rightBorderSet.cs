using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightBorderSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
    }

    // Update is called once per frame
    
}
