using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton2 : MonoBehaviour
{
    public themStore CurrentStore;
    public SaveandLoad2 SaveLoad;
    public GameObject NoMoneyPanel;
    public GameObject StorePanel;



    public int btID;

    public void Start()
    {
        btID = gameObject.GetComponent<ThemHolder>().ItemID;

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

            if (CurrentStore.CurrentItemList[btID].IsSelected == true)
            {
                Debug.Log(btID);
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

        if (MoneyManager.Instance.IsEnoughScore(CurrentStore.CurrentItemList[btID].ItemPrice))
        {
            CurrentStore.CurrentItemList[btID].IsBough = true;
            PlayerPrefs.SetInt("ThemeId", btID);
            CurrentStore.UpdateItemSprite();
            SaveLoad.Save();
        }
        else
        {
            CurrentStore.UpdateItemSprite();
            NoMoneyPanel.SetActive(true);
            SceneMover.Instankk.normalSet();
        }
    }

    private void Use()
    {
        foreach (var item in CurrentStore.CurrentItemList)
        {
            item.IsSelected = false;
        }
        CurrentStore.CurrentItemList[btID].IsSelected = true;
        PlayerPrefs.SetInt("ThemeId", btID);
        if (PlayerPrefs.GetInt("FirstStart2") == 0)
        {
            PlayerPrefs.SetInt("FirstStart2", 1);
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
        PlayerPrefs.SetInt("ThemeId", btID);
        if (PlayerPrefs.GetInt("FirstStart2") == 0)
        {
            PlayerPrefs.SetInt("FirstStart2", 1);
        }
        CurrentStore.UpdateItemSprite();
        SaveLoad.Save();
        StorePanel.SetActive(false);
        SceneMover.Instankk.normalSet();
        
    }
}
