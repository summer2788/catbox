using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public Store CurrentStore;

    public List<Item> SavedList = new List<Item>(); //아이템들 정보 저장하는 리스트

    public string DataNeme;

    private void OnEnable()
    {

       
       Invoke("Load", 0.05f); //스토어가 create 먼저 되고 load되게 할려고
    }

    public void Save()
    {
        SavedList.Clear();

        for (int i = 0; i < CurrentStore.CurrentItemList.Count; i++)
        {
            SavedList.Add(CurrentStore.CurrentItemList[i]);
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + DataNeme + ".data", FileMode.Create);

        bf.Serialize(stream, SavedList);
        stream.Close();

        CurrentStore.UpdateItemSprite();
    }

    public void Load()
    {
        
        if (File.Exists(Application.persistentDataPath + "/" + DataNeme + ".data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + DataNeme + ".data", FileMode.Open);

            SavedList = (List<Item>)bf.Deserialize(stream);
            stream.Close();

            for (int i = 0; i < SavedList.Count; i++) //아이템 저장값들 로드해옴
            {
                CurrentStore.CurrentItemList[i] = SavedList[i]; //current store의 아이템 리스트에 불러온 정보 저장함
            }

         
            
            
            
        }
        CurrentStore.CurrentItemList[0].IsBough = true;    //첫번째 고양이 구매된 걸로 
        CurrentStore.CurrentItemList[0].IsSelected = true;   //첫번째 고양이 선택되게 함
        CurrentStore.UpdateItemSprite();
    }

    public void ClearData()
    {
        for (int i = 0; i < SavedList.Count; i++)
        {
            CurrentStore.CurrentItemList[i].IsBough = false;
            CurrentStore.CurrentItemList[i].IsSelected = false;
        }
        PlayerPrefs.DeleteAll();
        Save();
    }
}
