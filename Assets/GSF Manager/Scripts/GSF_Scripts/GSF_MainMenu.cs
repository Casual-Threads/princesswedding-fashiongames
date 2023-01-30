using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;

#region MainMenu_UI
[System.Serializable]
public class MainMenu_UI
{
    public Image profileImg, ReplaceableAvatar, fillBar;
    public Text profileName, inputName, totalCoins, loadProgressText;
    public GameObject storeBtn;
    [Header("UI Panels")]
    public GameObject HelpScreen;
    public GameObject profilePanel;
    public GameObject menuPanel;
    public GameObject ExitDialogue;
    public GameObject loadingPanel;
}
#endregion

#region Settings_Props
[System.Serializable]
public class Settings_Props
{
    public GameObject soundOn, soundOff;
    public GameObject musicOn, musicOff;
    public GameObject vibrationOn, vibrationOff;
    public GameObject leftControlOn, leftControlOff;
    public GameObject rightControlOn, rightControlOff;
}
#endregion

public class GSF_MainMenu : MonoBehaviour
{
    #region Variables
    public Sprite[] playerAvatars;
    [FoldoutGroup("Settings Props")]
    [HideLabel]
    public Settings_Props settingsProps;
    [FoldoutGroup("MainMenu UI")]
    [HideLabel]
    public MainMenu_UI menu_UI;
    public AudioSource bgMusic, defaultBtn_SFX, playBtn_SFX;
    public Scenes NextScene;
	[Header ("Ad Sequence ID")]
	public int SequenceID = 0;
    public float loadTime = 4f;
    private bool loadNow = false;
    private string loadScene;
    #endregion

    #region Start
    void Start ()
    {
        Time.timeScale = 1;
		AudioListener.pause = false;
        loadScene = NextScene.ToString();
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            GSF_SaveLoad.LoadProgress();
        }
        if (SaveData.Instance.Players.Count < 1)
        {
            menu_UI.storeBtn.GetComponent<Button>().interactable = false;
            menu_UI.storeBtn.GetComponent<Image>().enabled = false;
        }
        InitializeUI ();
        SettingsAdjustment();
        ShowAds(SequenceID);
        LogEvents.Instance.Event("main_menu_started", "main_menu_started", "started");
    }
    #endregion

    #region ShowAds
    void ShowAds (int id)
    {
        GSF_AdsManager.ShowInterstitial (id, "Main Menu");
		GSF_AdsManager.ShowBanner (id, "Main Menu");
	}
    #endregion

    #region InitializeUI
    void InitializeUI () {
        menu_UI.HelpScreen.SetActive (false);
        menu_UI.ExitDialogue.SetActive (false);
        if(SaveData.Instance.Coins <= 0)
        {
            menu_UI.totalCoins.text = "000";
        }
        else
        {
            menu_UI.totalCoins.text = SaveData.Instance.Coins.ToString();
        }

        //if (!SaveData.Instance.ProfileCreated)
        //{
        //    menu_UI.menuPanel.SetActive(false);
        //    menu_UI.profilePanel.SetActive(true);
        //}
        //else
        //{
        //    menu_UI.menuPanel.SetActive(true);
        //    menu_UI.profilePanel.SetActive(false);
        //    menu_UI.profileName.text = SaveData.Instance.ProfileName;
        //    menu_UI.profileImg.sprite = playerAvatars[SaveData.Instance.SelectedAvatar];
        //    menu_UI.ReplaceableAvatar.sprite = playerAvatars[SaveData.Instance.SelectedAvatar];
        //}
    }
    #endregion

    #region PlayBtn
    public void PlayBtn ()
    {
        menu_UI.fillBar.fillAmount = 0;
        menu_UI.loadingPanel.SetActive(true);
        loadScene = NextScene.ToString();
        loadNow = true;
        LogEvents.Instance.Event("main_menu_completed", "main_menu_completed", "completed");
    }
    #endregion

    #region RemoveAds
    public void RemoveAds ()
    {
        Helper.RemoveAds();
        GSF_SaveLoad.SaveProgress();
	}
    #endregion

    #region RestorePurchases
    public void RestorePurchases () {
        GSF_InAppController.Instance.RestoreButtonClick();
	}
    #endregion

    #region ShowRateUs
    public void RateUs ()
    {
        //if (ConsoliAds.Instance != null)
        //{
        //    Application.OpenURL(ConsoliAds.Instance.rateUsURL);
        //}
    }
    #endregion

    #region Settings
    public void AdjustSound()
    {
        SaveData.Instance.isSound = !SaveData.Instance.isSound;
        SettingsAdjustment();
    }
    public void AdjustMusic()
    {
        SaveData.Instance.isMusic = !SaveData.Instance.isMusic;
        SettingsAdjustment();
    }
    public void AdjustVibration()
    {
        SaveData.Instance.isVibration = !SaveData.Instance.isVibration;
        SettingsAdjustment();
    }
    public void RightControls()
    {
        SaveData.Instance.isRightControls = true;
        SettingsAdjustment();
    }

    public void LeftControls()
    {
        SaveData.Instance.isRightControls = false;
        SettingsAdjustment();
    }
    private void SettingsAdjustment()
    {
        if (SaveData.Instance.isRightControls)
        {
            settingsProps.rightControlOn.SetActive(true);
            settingsProps.rightControlOff.SetActive(false);
            settingsProps.leftControlOff.SetActive(true);
            settingsProps.leftControlOn.SetActive(false);
        }
        else if (!SaveData.Instance.isRightControls)
        {
            settingsProps.rightControlOn.SetActive(false);
            settingsProps.rightControlOff.SetActive(true);
            settingsProps.leftControlOff.SetActive(false);
            settingsProps.leftControlOn.SetActive(true);
        }
        if (SaveData.Instance.isSound)
        {
            playBtn_SFX.enabled = true;
            defaultBtn_SFX.enabled = true;
            settingsProps.soundOn.SetActive(true);
            settingsProps.soundOff.SetActive(false);
        }
        else if (!SaveData.Instance.isSound)
        {
            playBtn_SFX.enabled = false;
            defaultBtn_SFX.enabled = false;
            settingsProps.soundOn.SetActive(false);
            settingsProps.soundOff.SetActive(true);
        }
        if (SaveData.Instance.isMusic)
        {
            if (!bgMusic.enabled)
            {
                bgMusic.enabled = true;
                bgMusic.Play();
            }
            settingsProps.musicOn.SetActive(true);
            settingsProps.musicOff.SetActive(false);
        }
        else if (!SaveData.Instance.isMusic)
        {
            bgMusic.enabled = false;
            settingsProps.musicOn.SetActive(false);
            settingsProps.musicOff.SetActive(true);
        }
        if (SaveData.Instance.isVibration)
        {
            settingsProps.vibrationOn.SetActive(true);
            settingsProps.vibrationOff.SetActive(false);
        }
        else if (!SaveData.Instance.isVibration)
        {
            settingsProps.vibrationOn.SetActive(false);
            settingsProps.vibrationOff.SetActive(true);
        }
        GSF_SaveLoad.SaveProgress();
    }
    #endregion

    #region ProfileManagement
    public void ShowProfile()
    {
        defaultBtn_SFX.Play();
        menu_UI.menuPanel.SetActive(false);
        menu_UI.profilePanel.SetActive(true);
    }
    public void SelectAvatar(int avatarIndex)
    {
        defaultBtn_SFX.Play();
        SaveData.Instance.SelectedAvatar = avatarIndex;
        menu_UI.profileImg.sprite = playerAvatars[avatarIndex];
        menu_UI.ReplaceableAvatar.sprite = playerAvatars[avatarIndex];
    }
    public void CreateProfile()
    {
        defaultBtn_SFX.Play();
        if (string.IsNullOrEmpty(menu_UI.inputName.text) && !SaveData.Instance.ProfileCreated)
        {
            SaveData.Instance.ProfileName = "Player Name";
        }
        else
        {
            SaveData.Instance.ProfileName = menu_UI.inputName.text.ToUpper();
        }
        SaveData.Instance.ProfileCreated = true;
        menu_UI.profileName.text = SaveData.Instance.ProfileName;
        menu_UI.menuPanel.SetActive(true);
        menu_UI.profilePanel.SetActive(false);
        GSF_SaveLoad.SaveProgress();
    }
    #endregion

    #region Exit
    public void Exit () {
		Application.Quit ();
	}
    #endregion

    #region ResetSaveData
    public void ResetSaveData () {
		GSF_SaveLoad.DeleteProgress ();
		GSF_SaveLoad.LoadProgress ();
	}
    #endregion

    #region MoreFunBtn
    public void MoreFunBtn ()
    {
       //if(ConsoliAds.Instance != null)
       // {
       //     Application.OpenURL(ConsoliAds.Instance.moreAppsURL);
       // }
    }
    #endregion

    #region UpdateStatus
    public void UpdateStatus()
    {

    }
    #endregion

    #region Update
    private void Update()
    {
        if (loadNow)
        {
            if (menu_UI.fillBar)
            {
                menu_UI.fillBar.fillAmount += Time.deltaTime / loadTime;
                if (menu_UI.fillBar.fillAmount >= 1)
                {
                    loadNow = false;
                    SceneManager.LoadScene(loadScene);
                }
            }
        }
    }
    #endregion
}