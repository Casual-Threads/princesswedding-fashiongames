using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    public static ModeSelection instance;
    public static ModeSelection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ModeSelection();
            }
            return instance;
        }
    }
    [Header("GameObject")]
    public GameObject LoadingPanel;
    public GameObject NotEnoughPopUp, VideoNotAvaliblePopUp;
    public GameObject Scrollerone,scroller2;
    [Header("Arrays")]
    public ItemInfo[] itemInfo;
    [Header("Text")]
    public Text TotalCoins;
    private string playerText;
    [Header("UI Images")]
    public Image fillBar;
    [Header("Audio Source")]
    public AudioSource purchaseSFX;
    public AudioSource itemSelectSFX;
    public Image bgImage;
    public Sprite secondSelectionBgSprite;
    private int selectedIndex;
    int index;
    private enum RewardType
    {
        none, Coins, SelectionItem
    }
    private RewardType rewardType;
    public enum LoadLevel
    {
        WeddingDressup, PartyDressup
    }
    private LoadLevel loadLevel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void OnEnable()
    {
        if (MyAdsManager.Instance != null)
        {
            MyAdsManager.Instance.onRewardedVideoAdCompletedEvent += OnRewardedVideoComplete;
        }
    }

    void OnDisable()
    {
        if (MyAdsManager.Instance != null)
        {
            MyAdsManager.Instance.onRewardedVideoAdCompletedEvent -= OnRewardedVideoComplete;
        }
    }
    private void Start()
    {
        if (!GameManager.Instance.Initialized)
        {
            GSF_SaveLoad.LoadProgress();
            GameManager.Instance.Initialized = true;
        }
        SetInitialProps();
        
        GetItemsInfo();
        TotalCoins.text = SaveData.Instance.Coins.ToString();
    }


    #region SetInitialProps
    private void SetInitialProps()
    {

        #region Initialing 
        if (SaveData.Instance.ModeProps.Count < itemInfo.Length)
        {
            for (int i = 0; i < itemInfo.Length; i++)
            {
                if (SaveData.Instance.ModeProps.Count <= i)
                {
                    // Add new data to SaveData file in case the file is empty or new data is available
                    Modesprops modeProps = new Modesprops();
                    modeProps.isLocked = itemInfo[i].isLocked;
                    SaveData.Instance.ModeProps.Add(modeProps);
                }
            }
        }
        // Setting up Dresses Properties to actual Properties from SaveData file  
        for (int i = 0; i < itemInfo.Length; i++)
        {
            itemInfo[i].isLocked = SaveData.Instance.ModeProps[i].isLocked;
        }
        //Adding Click listeners to btns 
        for (int i = 0; i < itemInfo.Length; i++)
        {
            int Index = i;
            if (itemInfo[i].itemBtn)
            {
                itemInfo[i].itemBtn.onClick.AddListener(() =>
                {
                    selectedIndex = Index;
                    SelectItem(Index);
                });
            }
        }
        #endregion

        GSF_SaveLoad.SaveProgress();
    }
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        #region Get Info
        for (int i = 0; i < itemInfo.Length; i++)
        {
            if (itemInfo[i].isLocked)
            {
                if (itemInfo[i].LockIcon) itemInfo[i].LockIcon.SetActive(true);
                if (itemInfo[i].coinsUnlock)
                {

                    if (itemInfo[i].coinSlot)
                    {
                        itemInfo[i].coinSlot.SetActive(true);
                        if (itemInfo[i].unlockCoins)
                        {
                            itemInfo[i].unlockCoins.text = itemInfo[i].requiredCoins.ToString();
                        }
                    }
                }
            }
            else
            {
                if (itemInfo[i].LockIcon) itemInfo[i].LockIcon.SetActive(false);
                if (itemInfo[i].coinSlot)
                {
                    itemInfo[i].coinSlot.SetActive(false);
                }
            }
        }
        #endregion
    }
    #endregion

    #region SelectItem
    private void SelectItem(int selectedIndex)
    {
        if (itemSelectSFX) itemSelectSFX.Play();
        rewardType = RewardType.SelectionItem;
        if (itemInfo[selectedIndex].isLocked)
        {
            if (itemInfo[selectedIndex].coinsUnlock)
            {
                if (SaveData.Instance.Coins >= itemInfo[selectedIndex].requiredCoins)
                {
                    itemInfo[selectedIndex].isLocked = false;
                    SaveData.Instance.ModeProps[selectedIndex].isLocked = false;
                    SaveData.Instance.Coins -= itemInfo[selectedIndex].requiredCoins;
                    GSF_SaveLoad.SaveProgress();
                    if (purchaseSFX) purchaseSFX.Play();
                }
                else
                {
                    NotEnoughPopUp.SetActive(true);
                }
            }
        }
        else
        {
            ShowInterStitial();
            LoadingPanel.SetActive(true);
            loadLevel = (LoadLevel)selectedIndex;
            GameManager.Instance.resetValue();
            if (selectedIndex == 0 || selectedIndex == 1)
            {
                StartCoroutine(Loading(loadLevel.ToString()));
            }
            else if(selectedIndex == 2)
            {
                if (SaveData.Instance.isMode == true)
                {
                    GameManager.Instance.ModeSelected = 1;
                    StartCoroutine(Loading("WeddingDressup"));
                    SaveData.Instance.isMode = !SaveData.Instance.isMode;
                }
                else if (SaveData.Instance.isMode == false)
                {
                    GameManager.Instance.ModeSelected = 1;
                    StartCoroutine(Loading("PartyDressup"));
                    SaveData.Instance.isMode = !SaveData.Instance.isMode;
                }
            }
            else if (selectedIndex == 3)
            {
                //loadLevel = (LoadLevel)Random.Range(0, 1);
                if (SaveData.Instance.isModetrue == true)
                {
                    GameManager.Instance.randomIndex = 1;
                    GameManager.Instance.ModeSelected = 1;
                    StartCoroutine(Loading("WeddingDressup"));
                    SaveData.Instance.isModetrue = !SaveData.Instance.isModetrue;
                }
                else if (SaveData.Instance.isModetrue == false)
                {
                    GameManager.Instance.randomIndex = 1;
                    GameManager.Instance.ModeSelected = 1;
                    StartCoroutine(Loading("PartyDressup"));
                    SaveData.Instance.isModetrue = !SaveData.Instance.isModetrue;
                }
            }
        }
        TotalCoins.text = SaveData.Instance.Coins.ToString();
        GetItemsInfo();
    }
    #endregion

    #region GetRewardedCoins
    public void GetRewardedCoins()
    {
        rewardType = RewardType.Coins;
        CheckVideoStatus();
    }
    #endregion

    #region CheckVideoStatus
    public void CheckVideoStatus()
    {
        if (MyAdsManager.Instance != null)
        {
            if (MyAdsManager.Instance.IsRewardedAvailable())
            {
                MyAdsManager.Instance.ShowRewardedVideos();
            }
            else
            {
                VideoNotAvaliblePopUp.SetActive(true);
            }
        }
        else
        {
            VideoNotAvaliblePopUp.SetActive(true);
        }
        //VideoNotAvaliblePopUp.SetActive(true);
    }
    #endregion

    #region RewardedVideoCompleted
    public void OnRewardedVideoComplete()
    {

        if (rewardType == RewardType.Coins)
        {
            SaveData.Instance.Coins += 2000;
            TotalCoins.text = SaveData.Instance.Coins.ToString();
            GSF_SaveLoad.SaveProgress();
        }
        rewardType = RewardType.none;
        if (purchaseSFX) purchaseSFX.Play();
    }
    #endregion

    #region Ienumerator
    IEnumerator Loading(string sceneName)
    {
        fillBar.fillAmount = 0;
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    public void selectMode()
    {
        bgImage.sprite = secondSelectionBgSprite;
        scroller2.SetActive(true);
        Scrollerone.SetActive(false);
        GameManager.Instance.ModeSelected = 0;
    }
    public void Back()
    {
        Scrollerone.SetActive(true);
        scroller2.SetActive(false);
    }

    public void ShowInterStitial()
    {
        if (MyAdsManager.instance)
        {
            MyAdsManager.instance.ShowInterstitialAds();
        }
    }
}
