using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{

    public Store CurrentStore;
    public SaveLoad SaveLoad;
    public GameObject NoMoneyPanel;
    public GameObject StorePanel;
   


    public int btID;

    public void Start()
    {
        btID = gameObject.GetComponent<ItemHolder>().ItemID;
        
    }
    public void ButtonAction()
    {
        if (!CurrentStore.CurrentItemList[btID].IsBough)
        {
            Debug.Log(1);
            Buy();
            
        }
        else
        {
             
            if (CurrentStore.CurrentItemList[btID].IsSelected==true)
            {
                Debug.Log(2);
                Use2();
                
            }
            else
            {
                Debug.Log(3);
                Use();
               
            }
           

        }
    }

    private void Buy()
    {
        foreach (var item in CurrentStore.CurrentItemList)
        {
            item.IsSelected = false;
        }
        CurrentStore.CurrentItemList[btID].IsSelected = true;

        if (MoneyManager.Instance.IsEnoughMoney(CurrentStore.CurrentItemList[btID].ItemPrice))
        {
            MoneyManager.Instance.MinusMoney(CurrentStore.CurrentItemList[btID].ItemPrice);
            CurrentStore.CurrentItemList[btID].IsBough = true;
            PlayerPrefs.SetInt("CatID", btID);

            CurrentStore.UpdateItemSprite();
            SaveLoad.Save();
        }
        else
        {
            CurrentStore.UpdateItemSprite();
            NoMoneyPanel.SetActive(true);
        }
    }

    private void Use()
    {
        foreach (var item in CurrentStore.CurrentItemList)
        {
            item.IsSelected = false;
        }
        CurrentStore.CurrentItemList[btID].IsSelected = true;
        PlayerPrefs.SetInt("CatID", btID);
        if (PlayerPrefs.GetInt("FirstStart") == 0)
        {
            PlayerPrefs.SetInt("FirstStart", 1);
        }
        CurrentStore.UpdateItemSprite();
        SaveLoad.Save();
        
    }

    private void Use2()
    {
        
        foreach (var item in CurrentStore.CurrentItemList)
        {
            item.IsSelected = false;
        }
        CurrentStore.CurrentItemList[btID].IsSelected = true;
        PlayerPrefs.SetInt("CatID", btID);
        if (PlayerPrefs.GetInt("FirstStart") == 0)
        {
            PlayerPrefs.SetInt("FirstStart", 1);
        }
        CurrentStore.UpdateItemSprite();
        SaveLoad.Save();
        StorePanel.SetActive(false);
        SceneMover.Instankk.normalSet();

    }
  
}
