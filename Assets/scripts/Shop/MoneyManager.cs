using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public TMP_Text MoneyText;
    public TMP_Text ScoreText;

    

    [HideInInspector]
    public int MoneyValue;
    public int ScoreValue;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MoneyValue = PlayerPrefs.GetInt("Fishscore");
        ScoreValue= PlayerPrefs.GetInt("BestScore");
        UpdateMoneyUI();
        UpdateScoreUI();
    }

    public void AddMoney(int value)
    {
        MoneyValue += value;
        UpdateMoneyUI();
    }

    public void MinusMoney(int value)
    {
        MoneyValue -= value;
        UpdateMoneyUI();
    }

    public bool IsEnoughMoney(int PriceValue)
    {
        if (MoneyValue >= PriceValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateMoneyUI()
    {
        MoneyText.text = MoneyValue.ToString();
        PlayerPrefs.SetInt("Fishscore", MoneyValue);
    }


   

    public bool IsEnoughScore(int PriceValue)
    {
        if (ScoreValue >= PriceValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateScoreUI()
    {
        ScoreText.text = ScoreValue.ToString();
        
    }
}
