using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickManager : MonoBehaviour
{
    public GameObject catPanel;
    public GameObject themePanel;
    public GameObject NoMoneyPanel;
    public GameObject NoScorePanel;
    public Image CatBt;
    public Image themeBt;

    public Image Caton;
    public Image CatOff;
    public Image ThemeOn;
    public Image ThemeOff;

    public Image fishImage;
    public TMP_Text BestScoreText;


    public void closeMoney()
    {
        NoMoneyPanel.SetActive(false);
    }

    public void closeScore()
    {
        NoScorePanel.SetActive(false);
    }

    public void clickcatPanel()
    {
        catPanel.SetActive(true);
        themePanel.SetActive(false);
        fishImage.gameObject.SetActive(true);
        BestScoreText.gameObject.SetActive(false);
        CatBt.sprite = Caton.sprite;
        themeBt.sprite = ThemeOff.sprite;

    }

    public void clickthemePanel()
    {
        themePanel.SetActive(true);
   
        catPanel.SetActive(false);
        fishImage.gameObject.SetActive(false);
        BestScoreText.gameObject.SetActive(true);
       
        themeBt.sprite = ThemeOn.sprite;
        CatBt.sprite = CatOff.sprite;

    }
}
