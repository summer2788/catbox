using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoremanager : MonoBehaviour
{

    public static Scoremanager Instance { get; private set; }
    public GameObject canvasshow; //캔버스 이미지 보여지기 위해 어쩔수 없이 여기 설정

    public TMP_Text scoreText;
    public TMP_Text GameOverScoreText;
    public TMP_Text fishText;
    public TMP_Text bestScore;
    public int score=0;
    public int fishscore=0;
    private int _bestScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        _bestScore = PlayerPrefs.GetInt("BestScore");
        fishscore = PlayerPrefs.GetInt("Fishscore");
        UpdateScoreUI();
        UpdateBestScoreUI();
        UpdateFishscoreUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int para)
    {
        score += para;
        UpdateScoreUI();
    }

    public void AddFish()
    {
        fishscore += 1;
        fishText.text=fishscore.ToString();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
        GameOverScoreText.text= score.ToString();
    }

    public void UpdateFishscoreUI()
    {
        
        PlayerPrefs.SetInt("Fishscore", fishscore);
        fishText.text = fishscore.ToString();
    }

    public void UpdateBestScoreUI()
    {
        if (score > _bestScore)
        {
            _bestScore = score;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
        bestScore.text = _bestScore.ToString();
    }

}
