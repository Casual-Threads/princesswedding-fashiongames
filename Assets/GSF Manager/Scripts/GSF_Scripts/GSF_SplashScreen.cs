using System.Collections;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class SplashProperties
{
    public Image splashImage;
    public Image fillBar;
    public bool canBlink;
    public enum BlinkType
    {
        onlyOnce,
        inLoop
    }
    [ShowIf("canBlink")]
    public BlinkType blinkType;
    [ShowIf("canBlink")]
    [Range(0, 10)]
    public float blinkSpeed;
    [ShowIf("canBlink")]
    [Range(6, 10)]
    public float blinkDuration;
    [Range(6, 10)]
    public float waitTime;
    public Scenes nextScene;
    public bool playBgMusic;
    [ShowIf("playBgMusic")]
    public AudioSource bgMusic;
    [ShowIf("playBgMusic")]
    public bool sameBGMInNextScene;
}

public class GSF_SplashScreen : MonoBehaviour
{
    [FoldoutGroup("Splash Properties")]
    [HideLabel]
    public SplashProperties splashProps;
    private float maxAlpha = 1f;
    private float alphaDifference = 0.015f;
    private float splashDelay, coroutineDelay;
    private float maxValue = 76f;
    private bool loadNow;
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (splashProps.fillBar) splashProps.fillBar.fillAmount = 0;
        if (splashProps.playBgMusic && splashProps.bgMusic)
        {
            splashProps.bgMusic.Play();
            if (splashProps.sameBGMInNextScene)
            {
                if (!splashProps.bgMusic.gameObject.GetComponent<BgSoundController>())
                {
                    splashProps.bgMusic.gameObject.AddComponent<BgSoundController>();
                }
                splashProps.bgMusic.gameObject.GetComponent<BgSoundController>().enabled = true;
            }
        }
        splashDelay = (alphaDifference * 3) - ((splashProps.blinkSpeed) / 223);
        coroutineDelay = splashDelay * (maxValue + (((splashProps.blinkSpeed) / 223) * 1000));
        if (!GameManager.Instance.Initialized)
        {
            InitializeGame();
        }
        if (splashProps.canBlink)
        {
            if (splashProps.splashImage)
            {
                StartCoroutine(BlinkManager());
            }
            else
            {
                Debug.Log("No splash image reference is assigned to blink");
            }          
        }
        StartCoroutine(LoadNextScene());
        loadNow = true;
    }

    void InitializeGame()
    {
        GSF_SaveLoad.LoadProgress();
        GameManager.Instance.Initialized = true;
    }


    IEnumerator BlinkManager()
    {
        if (splashProps.blinkType == SplashProperties.BlinkType.onlyOnce)
        {
            StartCoroutine(BlinkEffect());
            yield return new WaitForSeconds(coroutineDelay);
            alphaDifference *= -1f;
            StartCoroutine(BlinkEffect());
        }
        else if (splashProps.blinkType == SplashProperties.BlinkType.inLoop)
        {
            for (int i = 0; i < 100; i++)
            {
                StartCoroutine(BlinkEffect());
                yield return new WaitForSeconds(coroutineDelay);
                alphaDifference *= -1f;
                maxAlpha = 0;
                StartCoroutine(BlinkEffect());
                yield return new WaitForSeconds(coroutineDelay);
                alphaDifference *= -1f;
                maxAlpha = 1;
            }
        }
    }
    IEnumerator BlinkEffect()
    {
        for (int i = 0; i < maxValue; i++)
        {
            maxAlpha -= alphaDifference;
            splashProps.splashImage.color = new Color(1, 1, 1, maxAlpha);
            yield return new WaitForSeconds(splashDelay);
        }
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(splashProps.waitTime);
        SceneManager.LoadScene(splashProps.nextScene.ToString());
    }

    #region Update
    private void Update()
    {
        if (loadNow)
        {
            if (splashProps.fillBar)
            {
                splashProps.fillBar.fillAmount += Time.deltaTime / splashProps.waitTime;
                if (splashProps.fillBar.fillAmount >= 1)
                {
                    loadNow = false;
                }
            }
        }
    }
    #endregion
}
