using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatspriteChanger : MonoBehaviour
{
    public SpriteRenderer CurrentSprite;
    public List<AnimatorOverrideController> animlist = new List<AnimatorOverrideController>();
    public Animator animator;
    public int catID;
    public int themID;
    public GameObject firstTheme;
    public List<GameObject> themePrefabList = new List<GameObject>();
    private void OnEnable()
    {

        catID = PlayerPrefs.GetInt("CatID");
        themID = PlayerPrefs.GetInt("ThemeId");
        
        CurrentSprite.sprite = Resources.Load<Sprite>("Cats/" + PlayerPrefs.GetInt("CatID"));
        animator.runtimeAnimatorController = animlist[catID]; //애니메이션 할당


        firstTheme = Instantiate(themePrefabList[themID], new Vector3(0, 0, 0), Quaternion.identity);



    }



}
   


