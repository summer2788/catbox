using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   


    public Cat cat;
    public GameObject gameoverUI;
    public GameObject StopUI;

    public bool isGameover = false; 



    public void OnPlayerDead()
    {
        isGameover = true;
        Scoremanager.Instance.UpdateBestScoreUI();
        Scoremanager.Instance.UpdateFishscoreUI();
        gameoverUI.SetActive(true);
        Scoremanager.Instance.UpdateScoreUI();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Gotomain()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
        
    }

    public void StopNow()
    {
        Time.timeScale = 0f;
        Cat.Instan.noTouch = true;
        StopUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Cat.Instan.noTouch = false;
        StopUI.SetActive(false);

    }

    public void btSound()
    {
        AudioControl.instarr.PlayAudio(AudioControl.instarr.buttonSound);

    }


}
