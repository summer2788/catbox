using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Transform SpawnTransform;
    public ItemHolder Holder;
    public List<Item> CurrentItemList = new List<Item>();  //아이템(가격, 구매여부, 선택여부 ) 저장
    public List<ItemHolder> HolderList = new List<ItemHolder>(); //아이템 이미지 저장

    public int ItemsCount;
    public string ResourcePath;

    private void Start()
    {
       
        CreateStore();
      
    }

    private void CreateStore()
    {
        for (int i = 0; i < ItemsCount; i++)//curent holder 가 슬롯이 됨
        {
            

            CurrentItemList.Add(new Item());  //빈 아이템 생성후 리스트에 넣음

            // Add price for items
            if (i < 5)
            {
                CurrentItemList[i].ItemPrice =20;
            }
            else if (i < 9)
            {
                CurrentItemList[i].ItemPrice =30;
            }
            else if (i < 13)
            {
                CurrentItemList[i].ItemPrice = 40;
            }
            else if (i < 41)
            {
                CurrentItemList[i].ItemPrice = 50;
            }
            /*
            You cam add more price
            else if(i < P)
            {
                CurrentItemList[i].ItemPrice = N;
            }
            */
        }
    }

    public void UpdateItemSprite() //아이템 이미지 값 저장
    {
        for (int i = 0; i < HolderList.Count; i++)
        {
            if (CurrentItemList[i].IsBough)
            {
                HolderList[i].ItemImage.sprite = Resources.Load<Sprite>("Cats/" + i);
                HolderList[i].Name.sprite = Resources.Load<Sprite>("Names/" + i);
                HolderList[i].Name.color = new Color(255, 255, 255, 255);
                HolderList[i].PriceText.color= new Color(255, 255, 255, 0);
                if (CurrentItemList[i].IsSelected == true)
                {
                    HolderList[i].ItemClickImage.color = new Color(255, 255, 255, 255); 
                }
                else
                {
                    HolderList[i].ItemClickImage.color = new Color(255, 255, 255, 0);

                }
               
            }
            else
            {
                HolderList[i].ItemImage.sprite = Resources.Load<Sprite>("Locked");
                HolderList[i].Name.color = new Color(255, 255, 255, 0);
                HolderList[i].PriceText.text = CurrentItemList[i].ItemPrice.ToString();
                HolderList[i].PriceText.color = new Color(255, 255, 255, 255);


                if (CurrentItemList[i].IsSelected == true)
                {
                    HolderList[i].ItemClickImage.color = new Color(255, 255, 255, 255);
                }
                else
                {
                    HolderList[i].ItemClickImage.color = new Color(255, 255, 255, 0);

                }
            }
        }
    }
}
