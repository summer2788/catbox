using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMover : MonoBehaviour
{
    public GameObject themeprefab;
    public GameObject themeinstan;
    public float height;
    public bool oneSpawn; //한번반 생성되게 함
    // Start is called before the first frame update
    void Start()
    {
        oneSpawn = false;
        height = gameObject.GetComponent<BoxCollider2D>().bounds.size.y;
     
    }
     
    

    // Update is called once per frame
    void Update()
    {
        if (oneSpawn != true)
        {
            if (Cat.Instan.gameObject.transform.position.y >= gameObject.transform.position.y-2.3)
            {
                oneSpawn = true;
                themeinstan = Instantiate(themeprefab);
                themeinstan.transform.position = transform.position+new Vector3(0,height,0);
            }
        }
        else
        {
           if(Cat.Instan.gameObject.transform.position.y >= gameObject.transform.position.y+height*2 )
            {
                Destroy(gameObject);
            }
        }
        
    }
}
