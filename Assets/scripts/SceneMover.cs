using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneMover : MonoBehaviour
{

    public static SceneMover Instankk { get; private set; }
    public List<GameObject> intros;
    public GameObject storytext;
    public GameObject storyintro; //인트로 게임오브젝트
    public TMP_Text story;
    public TMP_Text next;
    public Image setting;
    public Button Bgmbt;
    public Button Soundbt;
    public Image soundOn;
    public Image soundOff;



    public static bool introstart=false;
    

    bool starton; //인트로1~6 실행 여부
    
    public int click;
    private string story1 = "평화로운 61번지, 우리 귀여운 친구 일오는 모두에게 사랑받는 행복한 고양이였습니다.";
    IEnumerator coroutine1;
    IEnumerator coroutine2;

    public Image Title;
    public Image background;
    public Image forground;

    public Image start_bt;
    public Image cust_bt;

    public Image set_bt;

    public GameObject customUI;


    private RectTransform TitleRect;

    

    private void Awake()
    {
        TitleRect =Title.GetComponent<RectTransform>();

        Instankk = this;




    }
    void Start()
    {

     
        
       

       // PlayerPrefs.SetInt("FirsIntrotStart", 0);
        if (PlayerPrefs.GetInt("FirsIntrotStart") == 0)
        {
            PlayerPrefs.SetInt("FirsIntrotStart", 1);
            //인트로 시작
            start1();

        }
        else
        {
            if (introstart == false)
            {
                //인트로 스플래시 화면 작동! -처음 한번만(함수 introStart)
                IntroStart();
                introstart = true; //더이상 인트로 스플래시는 실행되지 않음
            }
            else
            {
                normalSet();
            }
        }



        
        

       
    }
  


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && starton)
        {
            switch (click)
            {
                case 1:
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    click++;
                    coroutine1 = _typing("그러다보니 일오는 어느새 뚠냥이가 되어버렸어요!!");
                    StartCoroutine(coroutine1);
                    break;
                case 2:
                    StartCoroutine(coroutine2);
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    click++;
                    coroutine1 = _typing("뚠냥이 일오는 더욱 사랑받았지만, ");
                    StartCoroutine(coroutine1);

                    break;
                case 3:
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    click++;
                    coroutine1 = _typing("이제 몸에 맞는 보금자리가 없어졌어요!");
                    StartCoroutine(coroutine1);

                    break;
                case 4:
                    if (coroutine2 != null)
                    {
                        StopCoroutine(coroutine2);
                    }
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    intros[3].SetActive(true);
                    click++;
                    coroutine1 = _typing("그렇게 일오는 집사가 버리기 위해 현관에 놓은 박스에 들어가게 되는데...");
                    StartCoroutine(coroutine1);

                    break;
                case 5:
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    intros[4].SetActive(true);

                    click++;
                    coroutine1 = _typing("생각보다 편하잖아...?!");
                    StartCoroutine(coroutine1);

                    break;
                case 6:
                    if (coroutine1 != null)
                    {
                        StopCoroutine(coroutine1);
                    }
                    intros[5].SetActive(true);
                    click++;
                    coroutine1 = _typing("이제는 건강까지 걱정되는 일오는 박스를 통한 여정을 시작합니다! ");
                    StartCoroutine(coroutine1);
                    break;
                case 7:
                    storyintro.SetActive(false);
                    IntroStart();

                    starton = false;
                    break;
            }
        }
    }

    public void start1()
    {
        click = 0;
        coroutine1 = _typing(story1);
        coroutine2 = splash();

        //intro 시작
        starton = true;

        next.gameObject.SetActive(true);
        intros[0].SetActive(true);
        storytext.SetActive(true);
        click++;
        StartCoroutine(coroutine1);

        //SceneManager.LoadScene("IngameScene");
    }

    public void loadscene()
    {

        SceneManager.LoadScene("IngameScene");
        // intros[page - 1].SetActive(false);
        storytext.SetActive(false);
    }

    IEnumerator _typing(string rtext)
    {
        story.text = "";
        for (int i = 0; i <= rtext.Length; i++)
        {
            story.text = rtext.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator splash()
    {

        while (true)
        {
            intros[2].SetActive(false);
            intros[1].SetActive(true);
            yield return new WaitForSeconds(0.15f);
            intros[2].SetActive(true);
            intros[1].SetActive(false);
            yield return new WaitForSeconds(0.15f);


        }
    }

    public void IntroStart()
    {
        //Title 가운데로 set 후 Fade in 효과 
        Title.gameObject.SetActive(true);
        TitleRect.anchoredPosition = new Vector3(0, -600, 0);
        Title.color = new Color(255, 255, 255, 0);
        StartCoroutine(FadeIn(Title, 1.5f));
        //Title 화면 올라감
        StartCoroutine(moveUp(TitleRect, -276,1f)); //-200까지 1초로 움직일 거임
        //나머지 for, back 화면 Fade in 효과
        background.gameObject.SetActive(true);
        forground.gameObject.SetActive(true);
        background.color = new Color(255, 255, 255, 0);
        forground.color = new Color(255, 255, 255, 0);
       
        StartCoroutine(FadeIn2(forground, 1.5f));
        StartCoroutine(FadeIn3(background, 1.5f));
        Invoke("btn_Set", 1f);
    }

    public void normalSet()
    {
        Title.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        forground.gameObject.SetActive(true);
        start_bt.gameObject.SetActive(true);
        cust_bt.gameObject.SetActive(true);
        set_bt.gameObject.SetActive(true);

    }


    public void normalClose()
    {
        Title.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        forground.gameObject.SetActive(false);
        start_bt.gameObject.SetActive(false);
        cust_bt.gameObject.SetActive(false);
        
        set_bt.gameObject.SetActive(false);

    }

    public void customClick ()
    {
        normalClose();
        customUI.SetActive(true);
        

    }

    public void customBack()
    {
        customUI.SetActive(false);
        SceneMover.Instankk.normalSet();

    }

    public void settingClick()
    {
        if (PlayerPrefs.GetInt("BgmOff") == 0)//사용자가 소리 켜놨더라면 이미지는 off 소리도 on
        {
            Bgmbt.GetComponent<Image>().sprite = soundOff.sprite;
        }
        else
        {
            Bgmbt.GetComponent<Image>().sprite = soundOn.sprite;
        }
        if (PlayerPrefs.GetInt("SoundOff") == 0)
        {
            Soundbt.GetComponent<Image>().sprite = soundOff.sprite;
        }
        else
        {
            Soundbt.GetComponent<Image>().sprite = soundOn.sprite;
        }
       
        setting.gameObject.SetActive(true);


    }

    public void settingClose()
    {
       
        setting.gameObject.SetActive(false);


    }

    public void BgMClick()
    {
        if (PlayerPrefs.GetInt("BgmOff") == 0)
        {
            //소리를 끔,  이미지 on로 변경
            PlayerPrefs.SetInt("BgmOff", 1);
            Bgmbt.GetComponent<Image>().sprite = soundOn.sprite;
            Debug.Log(1);
            AudioControl.instarr.BgmOff();


        }
        else if (PlayerPrefs.GetInt("BgmOff") == 1)
        {
            //소리를 킴, 이미지 off으로 변경
            PlayerPrefs.SetInt("BgmOff", 0);
            Bgmbt.GetComponent<Image>().sprite = soundOff.sprite;
            Debug.Log(2);
            AudioControl.instarr.BgmOn();
        }


    }

    public void soundEffectClick()
    {

        if (PlayerPrefs.GetInt("SoundOff") == 0)
        {
            //소리를 끔,  이미지 on로 변경
            PlayerPrefs.SetInt("SoundOff", 1);
            Soundbt.GetComponent<Image>().sprite = soundOn.sprite;
            AudioControl.instarr.EffectAllStop();
        }
        else if (PlayerPrefs.GetInt("SoundOff") == 1)
        {
            //소리를 킴, 이미지 off으로 변경
            PlayerPrefs.SetInt("SoundOff", 0);
            Soundbt.GetComponent<Image>().sprite = soundOff.sprite;
            AudioControl.instarr.EffectAllplay();
        }

    }



    IEnumerator FadeIn(Image image, float time)
    {
        float normaltime = 0f;
        float end = 1f;
        float start = 0f;
        Color fadecolor = image.color;
        while (fadecolor.a < 1f)
        {
            
            normaltime += (Time.deltaTime / time);

            fadecolor.a = Mathf.Lerp(start, end, normaltime);
            image.color = fadecolor;

            yield return null;
        }
    }

    IEnumerator FadeIn2(Image image, float time)
    {
        float normaltime = 0f;
        float end = 1f;
        float start = 0f;
        Color fadecolor = image.color;
        yield return new WaitForSecondsRealtime(1f);
        while (fadecolor.a < 1f)
        {
            
            normaltime += (Time.deltaTime / time);

            fadecolor.a = Mathf.Lerp(start, end, normaltime);
            image.color = fadecolor;

            yield return null;
        }
    }

    IEnumerator FadeIn3(Image image, float time)
    {
        float normaltime = 0f;
        float end = 1f;
        float start = 0f;
        Color fadecolor = image.color;
        yield return new WaitForSecondsRealtime(1f);
        while (fadecolor.a < 1f)
        {
           
            normaltime += (Time.deltaTime / time);

            fadecolor.a = Mathf.Lerp(start, end, normaltime);
            image.color = fadecolor;

            yield return null;
        }
    }

    IEnumerator moveUp(RectTransform rect,float distance,float realtime) //타이틀 화면 올라감
    {
        float normaltime = 0f;
        float end = distance;
        float start = rect.anchoredPosition.y;
        float tempt;
       
        while (rect.anchoredPosition.y <distance)
        {
          
            normaltime += (Time.deltaTime / realtime);
            tempt = Mathf.Lerp(start, end, normaltime);
            rect.anchoredPosition =  new Vector3(0, tempt, 0);

            yield return null;
        }
    }

    private void btn_Set()
    {
        start_bt.gameObject.SetActive(true);
        cust_bt.gameObject.SetActive(true);
       
        set_bt.gameObject.SetActive(true);
    }

    public void btnclick()
    {
        AudioControl.instarr.PlayAudio(AudioControl.instarr.buttonSound);
    }
}

