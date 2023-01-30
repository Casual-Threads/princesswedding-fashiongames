using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Sirenix.OdinInspector;

#region SoundProps
[System.Serializable]
public class LevelSelection_Sounds
{
    public AudioSource bgMusic;
    public AudioSource defaultBtnSFX;
    public AudioSource nextBtnSFX;
    public AudioSource backBtnSFX;
    public AudioSource selectionBtnSFX;
    public AudioSource buyingBtnSFX;
    public bool sameBGMInNextScene;
}
#endregion

#region LevelSelectionUI
[System.Serializable]
public class LevelSelectionUI
{
    public GameObject loadingPanel, normalLevelPanel, specialLevelPanel, unlockPanel;
    public GameObject eventsBtn, levelsBtn, unlockAllLevelsBtn;
    public Sprite defaultImage, lockedImage;
    public Text loadProgressText, totalCoins, notEnoughCoins;
    public Image barFill;
}
#endregion

#region NormalLevelProperties
[System.Serializable]
public class NormalLevelProperties
{
    public Button gamelLevel;
}
#endregion

#region SpecialLevelProperties
[System.Serializable]
public class SpecialLevelProperties
{
    public Button gamelLevel;
    [OnValueChanged("IsLockedChanged")]
    public bool isLocked;
    public enum UnlockType
    {
        none,
        withPrice,
        withCoins,
        withBoth
    }
    [ShowIf("isLocked")]
    [OnValueChanged("UnlockTypeChanged")]
    public UnlockType unlockType;
    [ShowIf("isPriceBased")]
    [Range(0, 1000)]
    public float unlockPrice;
    [ShowIf("isCoinBased")]
    [Range(0, 100000)]
    public int unlockCoins;
    [ShowIf("isPriceBased")]
    public string IAP_Id;
    [HideInInspector]
    public bool isPriceBased, isCoinBased;
    private void UnlockTypeChanged()
    {
        if (unlockType == UnlockType.withCoins)
        {
            isPriceBased = false;
            isCoinBased = true;
        }
        else if (unlockType == UnlockType.withPrice)
        {
            isCoinBased = false;
            isPriceBased = true;
        }
        else if (unlockType == UnlockType.withBoth)
        {
            isCoinBased = true;
            isPriceBased = true;
        }
        else
        {
            isPriceBased = false;
            isCoinBased = false;
        }
    }
    private void IsLockedChanged()
    {
        if (!isLocked)
        {
            isPriceBased = false;
            isCoinBased = false;
        }
        else
        {
            unlockType = UnlockType.none;
        }
    }
}
#endregion

public class GSF_LevelSelection : MonoBehaviour
{
    #region Variables
    [FoldoutGroup("LevelSelection Sounds")]
    [HideLabel]
    public LevelSelection_Sounds soundProps;
    [FoldoutGroup("levelSelection UI")]
    [HideLabel]
    public LevelSelectionUI levelSelectionUI;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public NormalLevelProperties[] normalLevelProps;
    [ListDrawerSettings(ShowIndexLabels = true)]
    public SpecialLevelProperties[] specialLevelProps;
    [Header("Scene Selection")]
    public Scenes PreviousScene;
    public Scenes NextScene;
    [Header("Ad Sequence ID")]
    public int SequenceID = 2;
    public bool LoadingSequence;
    public int LoadingSequenceID = 4;
    private AsyncOperation asyncOperation;
    private string dollarSign = "$  ";
    private int lockedLevelIndex, selectedLevel;
    private GameObject[] bgMusicReferences;
    #endregion

    #region Start
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            GSF_SaveLoad.LoadProgress();
        }
        if(SaveData.Instance.Coins <= 0)
        {
            levelSelectionUI.totalCoins.text = "000";
        }
        else
        {
            levelSelectionUI.totalCoins.text = SaveData.instance.Coins.ToString();
        }
        if (SaveData.Instance.LevelsUnlocked == normalLevelProps.Length)
        {
            levelSelectionUI.unlockAllLevelsBtn.SetActive(false);
        }
        LevelSelectionSettings();
        SetupLevels();
        CheckLevelStatus();
        ShowAds(SequenceID, "Level Selection");
        LogEvents.Instance.Event("level_selection_started", "level_selection_started", "started");
    }
    #endregion

    #region ShowAds
    void ShowAds(int id, string SceneName)
    {
        GSF_AdsManager.ShowInterstitial(id, SceneName);
    }
    #endregion

    #region SetupLevels
    private void SetupLevels()
    {
        for (int i = 0; i < normalLevelProps.Length; i++)
        {
            int levelIndex = i;
            normalLevelProps[i].gamelLevel.onClick.AddListener(() => {
                selectedLevel = levelIndex;
                SetColors(selectedLevel);
                soundProps.defaultBtnSFX.Play();
            });

        }
        for (int i = 0; i < specialLevelProps.Length; i++)
        {
            int levelIndex = i;
            specialLevelProps[i].gamelLevel.onClick.AddListener(() => {
            });
        }
    }
    private void SetColors(int selectedIndex)
    {
        for (int i = 0; i < normalLevelProps.Length; i++)
        {
            if(selectedIndex == i)
            {
                normalLevelProps[i].gamelLevel.GetComponent<Image>().color = Color.green;
            }
            else
            {
                normalLevelProps[i].gamelLevel.GetComponent<Image>().color = Color.white;
            }
        }
    }
    #endregion

    #region SoundsInitialization
    private void SoundsInitialization()
    {
        var bgSounds = GameObject.FindGameObjectsWithTag("BgMusic");
        bgMusicReferences = new GameObject[bgSounds.Length];
        for (int i = 0; i < bgSounds.Length; i++)
        {
            bgMusicReferences[i] = bgSounds[i];
        }
        if (bgMusicReferences.Length < 2)
        {
            if (soundProps.bgMusic)
            {
                soundProps.bgMusic.Play();
                if (soundProps.sameBGMInNextScene)
                {
                    if (!soundProps.bgMusic.gameObject.GetComponent<BgSoundController>())
                    {
                        soundProps.bgMusic.gameObject.AddComponent<BgSoundController>();
                    }
                    soundProps.bgMusic.gameObject.GetComponent<BgSoundController>().enabled = true;
                }
            }
            else
            {
                Debug.Log("BgMusic reference is not assigned");
            }
        }
    }
    #endregion

    #region LevelSelectionSettings
    private void LevelSelectionSettings()
    {
        if (!SaveData.Instance.isMusic)
        {
            soundProps.bgMusic.enabled = false;
        }
        if (!SaveData.Instance.isSound)
        {
            soundProps.defaultBtnSFX.enabled = false;
            soundProps.nextBtnSFX.enabled = false;
            soundProps.backBtnSFX.enabled = false;
            soundProps.selectionBtnSFX.enabled = false;
            soundProps.buyingBtnSFX.enabled = false;
        }
    }
    #endregion

    #region CheckLevelStatus
    private void CheckLevelStatus()
    {
        for (int i = 0; i < normalLevelProps.Length; i++)
        {
            normalLevelProps[i].gamelLevel.transform.GetChild(0).GetComponent<Text>().text = ""+ (i + 1);
            if (i < SaveData.Instance.LevelsUnlocked)
            {
                normalLevelProps[i].gamelLevel.GetComponent<Image>().sprite = levelSelectionUI.defaultImage;
                normalLevelProps[i].gamelLevel.interactable = true;
                //normalLevelProps[i].gamelLevel.transform.GetChild(0).GetComponent<Image>().enabled = true;
                normalLevelProps[i].gamelLevel.transform.GetChild(0).GetComponent<Text>().enabled = true;
            }
            else
            {
                normalLevelProps[i].gamelLevel.GetComponent<Image>().sprite = levelSelectionUI.lockedImage;
                normalLevelProps[i].gamelLevel.interactable = false;
                //normalLevelProps[i].gamelLevel.transform.GetChild(0).GetComponent<Image>().enabled = false;
                normalLevelProps[i].gamelLevel.transform.GetChild(0).GetComponent<Text>().enabled = false;

            }
        }
        if (specialLevelProps.Length > 0)
        {
            if (SaveData.Instance.EventsUnlocked == 0 && !specialLevelProps[0].isLocked)
            {
                SaveData.Instance.EventsUnlocked = 1;
                for (int i = 0; i < specialLevelProps.Length; i++)
                {
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                GSF_SaveLoad.SaveProgress();
            }
        }
        for (int i = 0; i < specialLevelProps.Length; i++)
        {
            if (i < SaveData.instance.EventsUnlocked)
            {
                specialLevelProps[i].gamelLevel.GetComponent<Image>().sprite = levelSelectionUI.defaultImage;
                specialLevelProps[i].gamelLevel.interactable = true;
                specialLevelProps[i].isLocked = false;
                specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                specialLevelProps[i].gamelLevel.GetComponent<Image>().sprite = levelSelectionUI.lockedImage;
                if (specialLevelProps[i].isLocked && specialLevelProps[i].unlockType == SpecialLevelProperties.UnlockType.none)
                {
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
                else if (specialLevelProps[i].isLocked && specialLevelProps[i].unlockType == SpecialLevelProperties.UnlockType.withCoins)
                {
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text
                        = specialLevelProps[i].unlockCoins.ToString();
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
                else if (specialLevelProps[i].isLocked && specialLevelProps[i].unlockType == SpecialLevelProperties.UnlockType.withPrice)
                {
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text
                       = dollarSign + specialLevelProps[i].unlockPrice;
                }
                else if (specialLevelProps[i].isLocked && specialLevelProps[i].unlockType == SpecialLevelProperties.UnlockType.withBoth)
                {
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text
                        = specialLevelProps[i].unlockCoins.ToString();
                    specialLevelProps[i].gamelLevel.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text
                       = dollarSign + specialLevelProps[i].unlockPrice;
                }
                specialLevelProps[i].gamelLevel.GetComponent<Image>().sprite = levelSelectionUI.lockedImage;
                specialLevelProps[i].gamelLevel.interactable = false;
            }
        }
    }
    #endregion

    #region SpecialEventsBtn
    public void SpecialEvents()
    {
        soundProps.defaultBtnSFX.Play();
        levelSelectionUI.totalCoins.gameObject.transform.parent.gameObject.SetActive(true);
        levelSelectionUI.totalCoins.text = SaveData.instance.Coins.ToString();
        levelSelectionUI.normalLevelPanel.SetActive(false);
        levelSelectionUI.specialLevelPanel.SetActive(true);
        levelSelectionUI.eventsBtn.SetActive(false);
        levelSelectionUI.levelsBtn.SetActive(true);
    }
    #endregion

    #region NormalLevelsBtn
    public void NormalLevels()
    {
        soundProps.defaultBtnSFX.Play();
        levelSelectionUI.normalLevelPanel.SetActive(true);
        levelSelectionUI.specialLevelPanel.SetActive(false);
        levelSelectionUI.eventsBtn.SetActive(true);
        levelSelectionUI.levelsBtn.SetActive(false);
    }
    #endregion

    #region PlayLevel
    public void PlayLevel()
    {
        soundProps.nextBtnSFX.Play();
        GameManager.Instance.selectedLevel = selectedLevel;
        levelSelectionUI.loadingPanel.SetActive(true);
        StartCoroutine(LoadScene());
        LogEvents.Instance.Event("level_selection_completed", "level_selection_completed", "completed");
    }
    IEnumerator LoadScene()
    {
        if (LoadingSequence)
        {
            ShowAds(LoadingSequenceID, "Loading Screen");
            yield return new WaitForSeconds(3);
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextScene.ToString());
        while (!asyncOperation.isDone)
        {
            levelSelectionUI.loadProgressText.text = Mathf.Floor(asyncOperation.progress * 100) + "%";
            if (asyncOperation.progress >= 0.9f)
            {
                levelSelectionUI.loadProgressText.text = "100%";
                levelSelectionUI.barFill.fillAmount = 1f;
            }
            yield return null;
        }
    }
    #endregion

    #region BackBtn
    public void BackBtn()
    {
        soundProps.backBtnSFX.Play();
        SceneManager.LoadScene(PreviousScene.ToString());
        LogEvents.Instance.Event("level_selection_to_player_selection", "level_selection_to_player_selection", "completed");
    }
    #endregion

    #region UnlockBtn
    public void UnlockBtn(int levelIndex)
    {
        soundProps.defaultBtnSFX.Play();
        levelSelectionUI.notEnoughCoins.enabled = false;
        if (specialLevelProps[levelIndex].unlockType == SpecialLevelProperties.UnlockType.withCoins)
        {
            levelSelectionUI.unlockPanel.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }
        else if (specialLevelProps[levelIndex].unlockType == SpecialLevelProperties.UnlockType.withPrice)
        {
            levelSelectionUI.unlockPanel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        lockedLevelIndex = levelIndex;
        levelSelectionUI.unlockPanel.SetActive(true);
        levelSelectionUI.unlockPanel.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text
            = specialLevelProps[levelIndex].unlockCoins.ToString();
        levelSelectionUI.unlockPanel.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>().text
            = dollarSign + specialLevelProps[levelIndex].unlockPrice;
    }
    #endregion

    #region PurchaseWithCoins
    public void PurchaseWithCoins()
    {
        levelSelectionUI.notEnoughCoins.enabled = false;
        soundProps.buyingBtnSFX.Play();
        if (SaveData.Instance.Coins >= specialLevelProps[lockedLevelIndex].unlockCoins)
        {
            levelSelectionUI.unlockPanel.SetActive(false);
            SaveData.Instance.EventsUnlocked++;
            SaveData.Instance.Coins -= specialLevelProps[lockedLevelIndex].unlockCoins;
            levelSelectionUI.totalCoins.text = SaveData.Instance.Coins.ToString();
            CheckLevelStatus();
        }
        else
        {
            levelSelectionUI.notEnoughCoins.enabled = true;
            Debug.Log("Not Enough Coins");
        }
    }
    #endregion

    #region PurchaseWithPrice
    public void PurchaseWithPrice()
    {
        soundProps.buyingBtnSFX.Play();
        levelSelectionUI.notEnoughCoins.enabled = false;
    }
    #endregion

    #region CancleBtn
    public void CancleBtn()
    {
        soundProps.defaultBtnSFX.Play();
        levelSelectionUI.unlockPanel.SetActive(false);
    }
    #endregion

    #region Update
    private void Update()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    if (Input.GetKey(KeyCode.Escape))
        //    {
        //        SceneManager.LoadScene(PreviousScene.ToString());
        //    }
        //}
    }
    #endregion

    #region UnlockAllLevels
    public void UnlockAllLevels()
    {
        soundProps.buyingBtnSFX.Play();
        Helper.UnlockAllLevels(normalLevelProps.Length);
        CheckLevelStatus();
        StartCoroutine(DisableObject(levelSelectionUI.unlockAllLevelsBtn));
    }
    IEnumerator DisableObject(GameObject refObject)
    {
        yield return new WaitForSeconds(1f);
        refObject.SetActive(false);
    }
    #endregion

    public void UpdateStatus()
    {

    }

}
