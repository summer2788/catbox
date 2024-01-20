using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreShow : MonoBehaviour
{

    public float moveSpeed;
    public float destroyTime;
    public float alphaSpeed;
    public int count;
    public TMP_Text floatingText;
    Color alpha;

    public Vector3 vector;


    // Start is called before the first frame update
    void Start()
    {
        floatingText = GetComponent<TextMeshPro>();
        floatingText.text = "Perfect x" + count;
        alpha = floatingText.color;
        Invoke("DestroyObject", destroyTime);
        
    }

    private void Update()
    {
       
       
        vector.Set(floatingText.transform.position.x, floatingText.transform.position.y + (moveSpeed * Time.deltaTime), floatingText.transform.position.z);
        floatingText.transform.position = vector;
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        floatingText.color = alpha;
        
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

  
  

    

}
