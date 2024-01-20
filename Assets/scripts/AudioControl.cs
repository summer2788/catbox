using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public static AudioControl instarr;


    public AudioSource audio_bgm; 
    public AudioSource audio_effect;

   

    public AudioClip bounseSound;
    public AudioClip buttonSound;
    public AudioClip fishSound;
    public AudioClip jumpSound;
    public AudioClip MainBgm;
    public AudioClip normalScoreSound;
    public AudioClip perfectScoreSound;
    public AudioClip superJumpSound;
    public AudioClip collision;


    //Singleton Pattern
    private void Awake()
    {
        if (instarr != null)
        {
            Destroy(gameObject);
            return;
        }
        instarr = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        if (PlayerPrefs.GetInt("BgmOff") == 0)//사용자가 소리 켜놨더라면 이미지는 off 소리도 on
        {
            AudioControl.instarr.PlayBgm();
        }
    }

    public void BgmOn()
    {
        audio_bgm.enabled = true;
        PlayBgm();
    }

    public void BgmOff()
    {
        audio_bgm.enabled = false;
    }

    public void EffectAllStop()
    {
        audio_effect.enabled=false;
    }

    public void EffectAllplay()
    {
        audio_effect.enabled = true;
    }

    public void PlayAudio(AudioClip currentClip)
    {
        audio_effect.PlayOneShot(currentClip);
    }

    public void PlayBgm()
    {

        audio_bgm.clip = MainBgm;
        audio_bgm.Play();



    }

        
}
