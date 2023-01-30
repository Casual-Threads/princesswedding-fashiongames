using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;

[System.Serializable]
public class WeddingDressUpElements
{
    [Header("Panels")]
    public GameObject GamePanel;
    public GameObject VsPanel, CoinNotAvailablePanel, VideoNotAvailablePanel, WinLevelCompletePanel, LoseLevelCompletePanel, SubmitPanel, JudgementPanel, AdPenl, ObjectOff;
    [Header("Scrollers")]
    public GameObject allScroller;
    public GameObject categoryScroller, dressUpScroller, makeUpScroller, itemsScroller, DressScroller, EaringScroller, NecklesScroller, BangalScroller, BouqetScroller, ShoesScroller,
                      HairScroller, LipsScroller, CrownScroller, VeilScroller, EyeshadeScroller, BagScroller, BlushScroller;
    [Header("UI")]
    public GameObject Preview;
    public GameObject Home, PurppleBar, CoinSlot, VideoSlot, LetsStartBtn, Winner;
    [Header("BG Image")]
    public Image modeCard;
    public Image bgImage;
    [Header("Button")]
    public GameObject MakeUpBtn;
    public GameObject DressUpBtn;

}

[System.Serializable]
public class WeddingDressUpPlayerElements
{
    [Header("Player Character")]
    public GameObject PlayerCharactar;
    [Header("Player Images")]
    public Image DressImage;
    public Image EaringImage, NecklesImage, BangalImage, BouqetImage, ShoesImage, HairImage, LipsImage, CrownImage, VeilImage, EyeshadeImage, BagImage, BlushImage;
    [Header("Player Bot Images")]
    public Image playerBotInVsPanel;
    public Image playerBotInAnim, playerBotInJudge;
    [Header("Player Text")]
    public Text playerIdInVsPanel;
    public Text playerIdInAnim, playerVoteText;
}

[System.Serializable]
public class WeddingDressUpOppnentElements
{
    [Header("Bot & Opponent")]
    public GameObject OpponentCharactar;
    [Header("Opponent Images")]
    public Image oppoDressImage;
    public Image oppoVeilImage, oppoNecklesImage, oppoBouqetImage, oppoBangalImage, oppoShoesImage, oppoHairImage, oppoLipsImage,
                 oppoCrownImage, oppoEyeshadeImage, oppoEaringImage, oppoBagImage, oppoBlushImage;
    [Header("Opponent Bot Images")]
    public Image oppoBotInVsPanel;
    public Image oppoBotInAnim, oppoBotInJudge;
    [Header("Opponent Text")]
    public Text oppoIdInVsPanel;
    public Text oppoIdInAnim, oppoVoteText;
}

[System.Serializable]
public enum WeddingDressUpSelectedItem
{
    Dress, Earing, Bangal, Veil, Crown, Neckles, Bouqet, Bag, Shoes, Hair, Lips, Eyeshade, Blush
}

public class WeddingDressUp : MonoBehaviour
{
    [FoldoutGroup("UI Elements")]
    [HideLabel]
    public WeddingDressUpElements uIElements;
    public WeddingDressUpPlayerElements playerElements;
    public WeddingDressUpOppnentElements oppoElements;
    public WeddingDressUpSelectedItem selectedItem;
    [Header("Loading")]
    public GameObject LoadingPanel;
    public Image fillBar;
    [Header("Mover Item")]
    public MRS_Manager CharactorMover;
    public MRS_Manager OpponentMover;
    public CoinsAdder coinsAdder;
    [Header("Sprites Array")]
    public Sprite[] cardSprites;
    public Sprite[] playerBotImageSprites;
    public Sprite[] oppoBotImageSprites;
    public Sprite[] DressSprites;
    public Sprite[] VeilSprites;
    public Sprite[] NecklesSprites;
    public Sprite[] BouqetSprites;
    public Sprite[] BangalSprites;
    public Sprite[] ShoesSprites;
    public Sprite[] HairSprites;
    public Sprite[] LipsSprites;
    public Sprite[] CrownSprites;
    public Sprite[] EyeshadeSprites;
    public Sprite[] EaringSprites;
    public Sprite[] BagSprites;
    public Sprite[] BlushSprites;
    public Sprite[] CategoryDefaultSprites;
    public Sprite[] CategorySelectedSprites;
    [Header("Scroller Btn Image Array")]
    public Image[] CategoryImage;
    [Header("Item List")]
    private List<ItemInfo> DressList = new List<ItemInfo>();
    private List<ItemInfo> VeilList = new List<ItemInfo>();
    private List<ItemInfo> NecklesList = new List<ItemInfo>();
    private List<ItemInfo> BouqetList = new List<ItemInfo>();
    private List<ItemInfo> BangalList = new List<ItemInfo>();
    private List<ItemInfo> ShoesList = new List<ItemInfo>();
    private List<ItemInfo> HairList = new List<ItemInfo>();
    private List<ItemInfo> LipsList = new List<ItemInfo>();
    private List<ItemInfo> CrownList = new List<ItemInfo>();
    private List<ItemInfo> EyeshadeList = new List<ItemInfo>();
    private List<ItemInfo> EaringList = new List<ItemInfo>();
    private List<ItemInfo> BagList = new List<ItemInfo>();
    private List<ItemInfo> BlushList = new List<ItemInfo>();
    [Header("Sprites")]
    public Sprite SubmitPanelBGSprites;
    public Sprite JudgementPanelBGSprites;
    public Sprite selectionSelectedSprite;
    public Sprite selectionDefaultSprite;
    [Header("Audio")]
    public AudioSource CoinsSFX;
    public AudioSource CategorySFX;
    public AudioSource purchaseSFX;
    public AudioSource itemSelectSFX;
    public AudioSource WinnerSFX;
    public AudioSource LoseSFX;
    [Header("Particals")]
    public GameObject FinalPartical;
    [Header("Text")]
    public Text TotalCoins;
    public Text PlayerScoreText, OpponentScoreText, Comparetext;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;
    [Header("Different Index")]
    private int selectedIndex, bgindex;
    [Header("Bool Variable")]
    bool ADTime = true;
    private bool canShowInterstitial;
    bool IsDressTrue, IsHairTrue, IsShoesTrue = false;
    [Header("Different Rank")]
    private int DressRank, VeilRank, NecklesRank, BouqetRank, BangalRank, ShoesRank, HairRank, LipsRank, CrownRank, EyeshadeRank, EaringRank, BagRank, BlushRank;
    private int DressTotalRank, VeilTotalRank, NecklesTotalRank, BouqetTotalRank, BangalTotalRank, ShoesTotalRank, HairTotalRank,
                LipsTotalRank, CrownTotalRank, EyeshadeTotalRank, EaringTotalRank, BagTotalRank, BlushTotalRank;
    private int oppoDressTotalRank, oppoVeilTotalRank, oppoNecklesTotalRank, oppoBouqetTotalRank, oppoBangalTotalRank, oppoShoesTotalRank,
                oppoHairTotalRank, oppoLipsTotalRank, oppoCrownTotalRank, oppoEyeshadeTotalRank, oppoEaringTotalRank, oppoBagTotalRank, oppoBlushTotalRank;
    [Header("Int Variable")]
    private int playerTotal = 0, opponentTotal = 0;
    private int playerTotalMarks = 0, opponentTotalMarks = 0;
    public Animator VsAnim;
    public Animator CompareAnim;
    public Animator CardAnimation;
    public GameObject Compareing;
    string _name = "";
    private enum RewardType
    {
        none, Coins, SelectionItem, MultipleOfTwo
    }
    private RewardType rewardType;

    #region start
    private void Start()
    {
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            GSF_SaveLoad.LoadProgress();
        }
        if (GameManager.Instance.ModeSelected == 1 || GameManager.Instance.randomIndex == 1)
        {
            uIElements.VsPanel.SetActive(true);
            uIElements.GamePanel.SetActive(false);
            GameManager.Instance.randomMode = 1;
        }

        selectedItem = WeddingDressUpSelectedItem.Dress;
        uIElements.DressScroller.SetActive(true);
        TotalCoins.text = SaveData.Instance.Coins.ToString();
        StartCoroutine(FindOpponent());
        SetInitialValues();
        GetItemsInfo();
        StartCoroutine(AdDelay(45));
        DressRank = HairRank = LipsRank= 1;
        VeilRank = NecklesRank = BouqetRank = BangalRank = ShoesRank = EyeshadeRank = CrownRank = EaringRank = BagRank = BlushRank = -1;
    }
    private void ShowInterstitial()
    {
        if (MyAdsManager.Instance)
        {
            MyAdsManager.Instance.ShowInterstitialAds();
        }
    }
    #endregion

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

    #region SetInitialValues
    private void SetInitialValues()
    {

        #region Initialing Dress
        if (uIElements.DressScroller)
        {
            var Dressinfo = uIElements.DressScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < Dressinfo.Length; i++)
            {
                DressList.Add(Dressinfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.DressLocked, DressList);
        SetItemIcon(DressList, DressSprites);
        #endregion

        #region Initialing Earing
        if (uIElements.EaringScroller)
        {
            var EaringInfo = uIElements.EaringScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < EaringInfo.Length; i++)
            {
                EaringList.Add(EaringInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.EaringLocked, EaringList);
        SetItemIcon(EaringList, EaringSprites);
        #endregion

        #region Initialing Bangal
        if (uIElements.BangalScroller)
        {
            var BangalInfo = uIElements.BangalScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < BangalInfo.Length; i++)
            {
                BangalList.Add(BangalInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.BangalLocked, BangalList);
        SetItemIcon(BangalList, BangalSprites);
        #endregion

        #region Initialing Veil
        if (uIElements.VeilScroller)
        {
            var VeilInfo = uIElements.VeilScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < VeilInfo.Length; i++)
            {
                VeilList.Add(VeilInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.VeilLocked, VeilList);
        SetItemIcon(VeilList, VeilSprites);
        #endregion

        #region Initialing Crown
        if (uIElements.CrownScroller)
        {
            var CrownInfo = uIElements.CrownScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < CrownInfo.Length; i++)
            {
                CrownList.Add(CrownInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.CrownLocked, CrownList);
        SetItemIcon(CrownList, CrownSprites);
        #endregion

        #region Initialing Neckles
        if (uIElements.NecklesScroller)
        {
            var NecklesInfo = uIElements.NecklesScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < NecklesInfo.Length; i++)
            {
                NecklesList.Add(NecklesInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.NecklesLocked, NecklesList);
        SetItemIcon(NecklesList, NecklesSprites);
        #endregion

        #region Initialing Bouqet
        if (uIElements.BouqetScroller)
        {
            var BouqetInfo = uIElements.BouqetScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < BouqetInfo.Length; i++)
            {
                BouqetList.Add(BouqetInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.BouqetLocked, BouqetList);
        SetItemIcon(BouqetList, BouqetSprites);
        #endregion

        #region Initialing Bag
        if (uIElements.BagScroller)
        {
            var BagInfo = uIElements.BagScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < BagInfo.Length; i++)
            {
                BagList.Add(BagInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.BagLocked, BagList);
        SetItemIcon(BagList, BagSprites);
        #endregion

        #region Initialing Shoes
        if (uIElements.ShoesScroller)
        {
            var ShoesInfo = uIElements.ShoesScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < ShoesInfo.Length; i++)
            {
                ShoesList.Add(ShoesInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.ShoesLocked, ShoesList);
        SetItemIcon(ShoesList, ShoesSprites);
        #endregion

        #region Initialing Hair
        if (uIElements.HairScroller)
        {
            var HairInfo = uIElements.HairScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < HairInfo.Length; i++)
            {
                HairList.Add(HairInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.HairLocked, HairList);
        SetItemIcon(HairList, HairSprites);
        #endregion

        #region Initialing Lips
        if (uIElements.LipsScroller)
        {
            var LipsInfo = uIElements.LipsScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < LipsInfo.Length; i++)
            {
                LipsList.Add(LipsInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.LipsLocked, LipsList);
        SetItemIcon(LipsList, LipsSprites);
        #endregion

        #region Initialing Eyeshade
        if (uIElements.EyeshadeScroller)
        {
            var EyeshadeInfo = uIElements.EyeshadeScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < EyeshadeInfo.Length; i++)
            {
                EyeshadeList.Add(EyeshadeInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.EyeshadeLocked, EyeshadeList);
        SetItemIcon(EyeshadeList, EyeshadeSprites);
        #endregion

        #region Initialing Blush
        if (uIElements.BlushScroller)
        {
            var BlushInfo = uIElements.BlushScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < BlushInfo.Length; i++)
            {
                BlushList.Add(BlushInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.weddingDressUpModeElements.BlushLocked, BlushList);
        SetItemIcon(BlushList, BlushSprites);
        #endregion

        GSF_SaveLoad.SaveProgress();
    }
    #endregion

    #region SetupItemData
    private void SetupItemData(List<bool> unlockItems, List<ItemInfo> _ItemsInfo)
    {
        if (_ItemsInfo.Count > 0)
        {
            if (unlockItems.Count < _ItemsInfo.Count)
            {
                for (int i = 0; i < _ItemsInfo.Count; i++)
                {
                    if (unlockItems.Count <= i)
                    {
                        // Add new data to SaveData file in case the file is empty or new data is available
                        unlockItems.Add(_ItemsInfo[i].isLocked);
                    }
                }
            }
            // Setting up Hairs Properties to actual Properties from SaveData file  
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                _ItemsInfo[i].isLocked = unlockItems[i];
            }
            //Adding Click listeners to btns 
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                int Index = i;
                if (_ItemsInfo[i].itemBtn)
                {
                    _ItemsInfo[i].itemBtn.onClick.AddListener(() =>
                    {
                        selectedIndex = Index;
                        SelectItem(Index);
                    });
                }
            }
        }
    }
    #endregion

    #region SetItemIcon
    private void SetItemIcon(List<ItemInfo> refList, Sprite[] btnIcons)
    {
        if (refList != null)
        {
            for (int i = 0; i < refList.Count; i++)
            {
                if (btnIcons.Length > i)
                {
                    if (btnIcons[i] && refList[i].itemIcon)
                    {
                        refList[i].itemIcon.sprite = btnIcons[i];
                    }
                }
            }
        }
    }
    #endregion

    #region SelectItem
    public void SelectItem(int index)
    {
        if (itemSelectSFX) itemSelectSFX.Play();
        if (selectedItem == WeddingDressUpSelectedItem.Dress)
        {
            CheckSelectedItem(DressList, DressSprites, playerElements.DressImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Veil)
        {
            CheckSelectedItem(VeilList, VeilSprites, playerElements.VeilImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Neckles)
        {
            CheckSelectedItem(NecklesList, NecklesSprites, playerElements.NecklesImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bouqet)
        {
            CheckSelectedItem(BouqetList, BouqetSprites, playerElements.BouqetImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bangal)
        {
            CheckSelectedItem(BangalList, BangalSprites, playerElements.BangalImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Shoes)
        {
            CheckSelectedItem(ShoesList, ShoesSprites, playerElements.ShoesImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Hair)
        {
            CheckSelectedItem(HairList, HairSprites, playerElements.HairImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Lips)
        {
            CheckSelectedItem(LipsList, LipsSprites, playerElements.LipsImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Crown)
        {
            CheckSelectedItem(CrownList, CrownSprites, playerElements.CrownImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Eyeshade)
        {
            CheckSelectedItem(EyeshadeList, EyeshadeSprites, playerElements.EyeshadeImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Earing)
        {
            CheckSelectedItem(EaringList, EaringSprites, playerElements.EaringImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bag)
        {
            CheckSelectedItem(BagList, BagSprites, playerElements.BagImage);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Blush)
        {
            CheckSelectedItem(BlushList, BlushSprites, playerElements.BlushImage);
        }
        GetItemsInfo();
        TotalCoins.text = SaveData.Instance.Coins.ToString();
        //TotalCoinsLevelComp.text = SaveData.Instance.Coins.ToString();

    }
    #endregion

    #region CheckSelectedItem
    private void CheckSelectedItem(List<ItemInfo> itemInfoList, Sprite[] itemSprites, Image itemImage)
    {
        rewardType = RewardType.SelectionItem;
        if (itemInfoList.Count > selectedIndex)
        {
            tempItem = itemInfoList[selectedIndex];
            if (itemInfoList[selectedIndex].isLocked)
            {
                if (itemInfoList[selectedIndex].videoUnlock)
                {
                    //OnRewardedVideoComplete();
                    CheckVideoStatus();
                }
                else if (itemInfoList[selectedIndex].coinsUnlock)
                {
                    if (SaveData.Instance.Coins >= itemInfoList[selectedIndex].requiredCoins)
                    {
                        itemInfoList[selectedIndex].isLocked = false;
                        SaveData.Instance.Coins -= itemInfoList[selectedIndex].requiredCoins;
                        UnlockSingleItem();
                        if (purchaseSFX) purchaseSFX.Play();
                        SelectItem(selectedIndex);
                    }
                    else
                    {
                        if (uIElements.CoinNotAvailablePanel)
                            uIElements.CoinNotAvailablePanel.SetActive(true);
                    }
                }
            }
            else
            {
                if (itemSprites.Length > selectedIndex)
                {
                    if (itemSprites[selectedIndex])
                    {
                        if (itemImage)
                        {
                            if (selectedItem == WeddingDressUpSelectedItem.Dress)
                            {
                                IsDressTrue = true;
                                GameManager.Instance.weddingDress = selectedIndex;
                                playerElements.DressImage.gameObject.SetActive(true);
                                DressRank = GetRank(selectedIndex, DressList.Count);
                                GameManager.Instance.weddingDressIndex = DressRank;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Veil)
                            {
                                GameManager.Instance.weddingVeil = selectedIndex;
                                VeilRank = GetRank(selectedIndex, VeilList.Count);
                                GameManager.Instance.weddingVeilIndex = VeilRank;
                                GameManager.Instance.wedVeil = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Neckles)
                            {
                                GameManager.Instance.weddingNeckles = selectedIndex;
                                //print(selectedIndex);
                                NecklesRank = GetRank(selectedIndex, NecklesList.Count);
                                GameManager.Instance.weddingNecklesIndex = NecklesRank;
                                GameManager.Instance.wedNeckles = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Bouqet)
                            {
                                GameManager.Instance.weddingBouqet = selectedIndex;
                                BouqetRank = GetRank(selectedIndex, BouqetList.Count);
                                GameManager.Instance.weddingBouqetIndex = BouqetRank;
                                GameManager.Instance.wedBouqet = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Bangal)
                            {
                                GameManager.Instance.weddingBangal = selectedIndex;
                                BangalRank = GetRank(selectedIndex, BangalList.Count);
                                GameManager.Instance.weddingBangalIndex = BangalRank;
                                GameManager.Instance.wedBangal = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Shoes)
                            {
                                IsShoesTrue = true;
                                GameManager.Instance.weddingShoes = selectedIndex;
                                ShoesRank = GetRank(selectedIndex, ShoesList.Count);
                                GameManager.Instance.weddingShoesIndex = ShoesRank;
                                GameManager.Instance.wedShoes = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Hair)
                            {
                                IsHairTrue = true;
                                GameManager.Instance.weddingHair = selectedIndex;
                                HairRank = GetRank(selectedIndex, HairList.Count);
                                GameManager.Instance.weddingHairIndex = HairRank;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Lips)
                            {
                                GameManager.Instance.weddingLips = selectedIndex;
                                LipsRank = GetRank(selectedIndex, LipsList.Count);
                                GameManager.Instance.weddingLipsIndex = LipsRank;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Crown)
                            {
                                GameManager.Instance.weddingCrown = selectedIndex;
                                CrownRank = GetRank(selectedIndex, CrownList.Count);
                                GameManager.Instance.weddingCrownIndex = CrownRank;
                                GameManager.Instance.wedCrown = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Eyeshade)
                            {
                                GameManager.Instance.weddingEyeshade = selectedIndex;
                                EyeshadeRank = GetRank(selectedIndex, EyeshadeList.Count);
                                GameManager.Instance.weddingEyeshadeIndex = EyeshadeRank;
                                GameManager.Instance.wedEyeshade = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Earing)
                            {
                                GameManager.Instance.weddingEaring = selectedIndex;
                                EaringRank = GetRank(selectedIndex, EaringList.Count);
                                GameManager.Instance.weddingEaringIndex = EaringRank;
                                GameManager.Instance.wedEaring = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Bag)
                            {
                                GameManager.Instance.weddingBag = selectedIndex;
                                BagRank = GetRank(selectedIndex, BagList.Count);
                                GameManager.Instance.weddingBagIndex = BagRank;
                                GameManager.Instance.wedBag = true;
                            }
                            else if (selectedItem == WeddingDressUpSelectedItem.Blush)
                            {
                                GameManager.Instance.weddingBlush = selectedIndex;
                                BlushRank = GetRank(selectedIndex, BlushList.Count);
                                GameManager.Instance.weddingBlushIndex = BlushRank;
                                GameManager.Instance.wedBlush = true;
                            }

                            if (IsDressTrue == true && IsHairTrue == true && IsShoesTrue == true)
                            {
                                if (uIElements.Preview) uIElements.Preview.gameObject.SetActive(true);
                            }
                            itemImage.gameObject.SetActive(false);
                            itemImage.gameObject.SetActive(true);
                            itemImage.sprite = itemSprites[selectedIndex];
                        }
                    }
                }
                CheckInterstitialAD();
            }
        }
    }
    #endregion

    #region SelectedCatagory
    private void DisableScrollers()
    {
        for (int i = 0; i < CategoryImage.Length; i++)
        {
            CategoryImage[i].sprite = CategoryDefaultSprites[i];
        }
        uIElements.DressScroller.SetActive(false);
        uIElements.HairScroller.SetActive(false);
        uIElements.BouqetScroller.SetActive(false);
        uIElements.ShoesScroller.SetActive(false);
        uIElements.NecklesScroller.SetActive(false);
        uIElements.LipsScroller.SetActive(false);
        uIElements.CrownScroller.SetActive(false);
        uIElements.EyeshadeScroller.SetActive(false);
        uIElements.BangalScroller.SetActive(false);
        uIElements.VeilScroller.SetActive(false);
        uIElements.EaringScroller.SetActive(false);
        uIElements.BagScroller.SetActive(false);
        uIElements.BlushScroller.SetActive(false);
    }
    public void SelectedCatagory(int index)
    {

        DisableScrollers();
        if (CategorySFX) CategorySFX.Play();
        CategoryImage[index].sprite = CategorySelectedSprites[index];
        if (index == (int)WeddingDressUpSelectedItem.Dress)
        {
            selectedItem = WeddingDressUpSelectedItem.Dress;
            uIElements.DressScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Hair)
        {
            selectedItem = WeddingDressUpSelectedItem.Hair;
            uIElements.HairScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Bouqet)
        {
            selectedItem = WeddingDressUpSelectedItem.Bouqet;
            uIElements.BouqetScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Neckles)
        {
            selectedItem = WeddingDressUpSelectedItem.Neckles;
            uIElements.NecklesScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Lips)
        {
            selectedItem = WeddingDressUpSelectedItem.Lips;
            uIElements.LipsScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Shoes)
        {
            selectedItem = WeddingDressUpSelectedItem.Shoes;
            uIElements.ShoesScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Eyeshade)
        {
            selectedItem = WeddingDressUpSelectedItem.Eyeshade;
            uIElements.EyeshadeScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Bangal)
        {
            selectedItem = WeddingDressUpSelectedItem.Bangal;
            uIElements.BangalScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Veil)
        {
            selectedItem = WeddingDressUpSelectedItem.Veil;
            uIElements.VeilScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Crown)
        {
            selectedItem = WeddingDressUpSelectedItem.Crown;
            uIElements.CrownScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Earing)
        {
            selectedItem = WeddingDressUpSelectedItem.Earing;
            uIElements.EaringScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Bag)
        {
            selectedItem = WeddingDressUpSelectedItem.Bag;
            uIElements.BagScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, -10, 0), 0.5f, true, false);
        }
        else if (index == (int)WeddingDressUpSelectedItem.Blush)
        {
            selectedItem = WeddingDressUpSelectedItem.Blush;
            uIElements.BlushScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);
        }
        GetItemsInfo();
    }
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        if (selectedItem == WeddingDressUpSelectedItem.Dress)
        {
            SetItemsInfo(DressList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Hair)
        {
            SetItemsInfo(HairList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bouqet)
        {
            SetItemsInfo(BouqetList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Shoes)
        {
            SetItemsInfo(ShoesList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Neckles)
        {
            SetItemsInfo(NecklesList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Lips)
        {
            SetItemsInfo(LipsList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Eyeshade)
        {
            SetItemsInfo(EyeshadeList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bangal)
        {
            SetItemsInfo(BangalList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Veil)
        {
            SetItemsInfo(VeilList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Crown)
        {
            SetItemsInfo(CrownList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Earing)
        {
            SetItemsInfo(EaringList);
        } 
        else if (selectedItem == WeddingDressUpSelectedItem.Bag)
        {
            SetItemsInfo(BagList);
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Blush)
        {
            SetItemsInfo(BlushList);
        }
    }
    #endregion

    #region SetItemsInfo
    private void SetItemsInfo(List<ItemInfo> _ItemInfo)
    {
        if (_ItemInfo == null) return;
        for (int i = 0; i < _ItemInfo.Count; i++)
        {
            if (_ItemInfo[i].btnBG)
            {
                if (i == selectedIndex)
                {
                    _ItemInfo[i].btnBG.sprite = selectionSelectedSprite;
                }
                else
                {
                    _ItemInfo[i].btnBG.sprite = selectionDefaultSprite;
                }
            }
            if (_ItemInfo[i].isLocked)
            {
                if (_ItemInfo[i].videoUnlock)
                {
                    if (_ItemInfo[i].videoBtn)
                    {
                        _ItemInfo[i].videoBtn.SetActive(true);
                    }
                    if (_ItemInfo[i].coinSlot)
                    {
                        _ItemInfo[i].coinSlot.SetActive(false);
                    }
                }
                else if (_ItemInfo[i].coinsUnlock)
                {
                    if (_ItemInfo[i].videoBtn)
                    {
                        _ItemInfo[i].videoBtn.SetActive(false);
                    }
                    if (_ItemInfo[i].coinSlot)
                    {
                        _ItemInfo[i].coinSlot.SetActive(true);
                        if (_ItemInfo[i].unlockCoins)
                        {
                            _ItemInfo[i].unlockCoins.text = _ItemInfo[i].requiredCoins.ToString();
                        }
                    }
                }
            }
            else
            {
                if (_ItemInfo[i].videoBtn) _ItemInfo[i].videoBtn.SetActive(false);
                if (_ItemInfo[i].coinSlot) _ItemInfo[i].coinSlot.SetActive(false);
            }
        }
    }
    #endregion

    #region UnlockSingleItem
    public void UnlockSingleItem()
    {
        if (selectedItem == WeddingDressUpSelectedItem.Dress)
        {
            SaveData.Instance.weddingDressUpModeElements.DressLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Hair)
        {
            SaveData.Instance.weddingDressUpModeElements.HairLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bouqet)
        {
            SaveData.Instance.weddingDressUpModeElements.BouqetLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Shoes)
        {
            SaveData.Instance.weddingDressUpModeElements.ShoesLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Neckles)
        {
            SaveData.Instance.weddingDressUpModeElements.NecklesLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Lips)
        {
            SaveData.Instance.weddingDressUpModeElements.LipsLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Eyeshade)
        {
            SaveData.Instance.weddingDressUpModeElements.EyeshadeLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bangal)
        {
            SaveData.Instance.weddingDressUpModeElements.BangalLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Veil)
        {
            SaveData.Instance.weddingDressUpModeElements.VeilLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Crown)
        {
            SaveData.Instance.weddingDressUpModeElements.CrownLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Earing)
        {
            SaveData.Instance.weddingDressUpModeElements.EaringLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Bag)
        {
            SaveData.Instance.weddingDressUpModeElements.BagLocked[selectedIndex] = false;
        }
        else if (selectedItem == WeddingDressUpSelectedItem.Blush)
        {
            SaveData.Instance.weddingDressUpModeElements.BlushLocked[selectedIndex] = false;
        }
        GSF_SaveLoad.SaveProgress();
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
                uIElements.VideoNotAvailablePanel.SetActive(true);
            }
        }
        else
        {
            uIElements.VideoNotAvailablePanel.SetActive(true);
        }
        //uIElements.VideoNotAvailablePanel.SetActive(true);
    }
    #endregion

    #region Btnfunctions
    public void SheIsReady()
    {
        ShowInterstitial();
        StartCoroutine(sheIsReadywithDelay());
    }

    public void Play(string str)
    {
        if(FinalPartical) FinalPartical.gameObject.SetActive(false);
        ShowInterstitial();
        LoadingPanel.SetActive(true);
        StartCoroutine(LoadingScene(str));
    }
    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Stylist+Girls+Game+Studio");
    }
    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.sggs.diyjoystick.phonecover.tiedye.games");
    }
    public void LetsGo()
    {
        StartCoroutine(EnableOrDisable(0.1f, uIElements.VsPanel, false));
        VsAnim.gameObject.SetActive(true);
        StartCoroutine(EnableAnim(0.2f, VsAnim));
        StartCoroutine(EnableOrDisable(1.5f, uIElements.GamePanel, true));
    }
    public void Back()
    {
        ShowInterstitial();
        uIElements.SubmitPanel.SetActive(false);
        uIElements.allScroller.SetActive(true);
        uIElements.CoinSlot.SetActive(true);
        uIElements.VideoSlot.SetActive(true);
        CharactorMover.Move(new Vector3(-19, 4, 0), 0.5f, true, false);
    }
    public void Review()
    {
        ShowInterstitial();
        StartCoroutine(review());
    }
    public void Comparesion()
    {
        //ShowInterstitial();
        SceneManager.LoadScene("ComparsionMode");
    }

    public void Submit()
    {
        ShowInterstitial();
        uIElements.bgImage.sprite = JudgementPanelBGSprites;
        OpponentMover.transform.localScale = new Vector3(-1, 1, 1);
        uIElements.JudgementPanel.SetActive(true);
        uIElements.allScroller.SetActive(false);
        uIElements.CoinSlot.SetActive(false);
        uIElements.VideoSlot.SetActive(false);
        uIElements.SubmitPanel.SetActive(false);
        oppoElements.OpponentCharactar.SetActive(true);
        CharactorMover.Move(new Vector3(-330, 0, 100), 0.5f, true, false);
        OpponentMover.Move(new Vector3(330, 0, 100), 0.5f, true, false);
        DressUpOpponent();
        SaveData.instance.LevelsUnlocked++;
        StartCoroutine(StartComparing(3f));
    }
    public void SubmitBtn()
    {
        if (GameManager.Instance.ModeSelected == 0 )
        {
            SheIsReady();
        }
        else if (GameManager.Instance.ModeSelected == 1 && GameManager.Instance.randomIndex == 0)
        {
            Submit();
        }
        else if (GameManager.Instance.ModeSelected == 1 && GameManager.Instance.randomIndex == 1)
        {
            Comparesion();
        }
    }
    #endregion

    #region Change Scroller
    public void ScrollerChange(int index)
    {
        DisableScrollers();
        if (index == 0)
        {
            selectedItem = WeddingDressUpSelectedItem.Hair;
            uIElements.MakeUpBtn.SetActive(false);
            uIElements.DressUpBtn.SetActive(true);
            uIElements.makeUpScroller.SetActive(true);
            uIElements.dressUpScroller.SetActive(false);
            uIElements.HairScroller.SetActive(true);
            uIElements.DressScroller.SetActive(false);
            CharactorMover.Move(new Vector3(-79, -503, -900), 0.5f, true, false);

        }
        else if (index == 1)
        {
            selectedItem = WeddingDressUpSelectedItem.Dress;
            uIElements.MakeUpBtn.SetActive(true);
            uIElements.DressUpBtn.SetActive(false);
            uIElements.dressUpScroller.SetActive(true);
            uIElements.makeUpScroller.SetActive(false);
            uIElements.HairScroller.SetActive(false);
            uIElements.DressScroller.SetActive(true);
            CharactorMover.Move(new Vector3(-19, 4, 0), 0.5f, true, false);

        }
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

    #region RewardedVideoCompleted
    public void OnRewardedVideoComplete()
    {
        if (rewardType == RewardType.SelectionItem)
        {
            if (tempItem != null) tempItem.isLocked = false;
            UnlockSingleItem();
            SelectItem(selectedIndex);
        }
        else if (rewardType == RewardType.Coins)
        {
            SaveData.Instance.Coins += 2000;
            TotalCoins.text = SaveData.Instance.Coins.ToString();
            //TotalCoinsLevelComp.text = SaveData.Instance.Coins.ToString();
            GSF_SaveLoad.SaveProgress();
        }
        GetItemsInfo();
        rewardType = RewardType.none;
        if (purchaseSFX) purchaseSFX.Play();
    }
    #endregion

    #region ProcessingAction
    IEnumerator ProcessingAction(float actionDelay, float nextActionDelay, Animator animatedObject, GameObject linkedObject, GameObject nextActionObject)
    {
        //Make sure you have same name of animation as action type
        if (animatedObject)
        {
            animatedObject.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(actionDelay);
        if (animatedObject)
        {
            animatedObject.gameObject.SetActive(false);
        }
        if (linkedObject)
        {
            linkedObject.SetActive(true);
        }
        //addCoins = 100;
        //addNow = true;
        yield return new WaitForSeconds(nextActionDelay);
        if (nextActionObject)
        {
            nextActionObject.SetActive(true);
        }
    }
    #endregion

    #region IEnumerator
    IEnumerator sheIsReadywithDelay()
    {
        uIElements.PurppleBar.SetActive(false);
        uIElements.CoinSlot.SetActive(false);
        uIElements.VideoSlot.SetActive(false);
        uIElements.allScroller.SetActive(false);
        uIElements.Preview.SetActive(false);
        CharactorMover.Move(new Vector3(-75, -300, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(-75, 200, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(-75, 0, -100), 0.5f, true, false);
        yield return new WaitForSeconds(1f);
        uIElements.Winner.SetActive(true);
        yield return new WaitForSeconds(1f);
        uIElements.CoinSlot.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(AddCoins(0.7f, 2000));
        yield return new WaitForSeconds(1f);
        FinalPartical.gameObject.SetActive(true);
        yield return new WaitForSeconds(7f);
        //FinalPartical.gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.5f);
        uIElements.WinLevelCompletePanel.SetActive(true);

    }
    IEnumerator FindOpponent()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < Random.Range(10, 25); i++)
        {
            uIElements.modeCard.sprite = cardSprites[Random.Range(0, cardSprites.Length)];
            playerElements.playerBotInVsPanel.sprite = playerBotImageSprites[Random.Range(0, playerBotImageSprites.Length)];
            oppoElements.oppoBotInVsPanel.sprite = oppoBotImageSprites[Random.Range(0, oppoBotImageSprites.Length)];
            playerElements.playerIdInVsPanel.text = Random.Range(10000,99999).ToString();
            oppoElements.oppoIdInVsPanel.text = Random.Range(10000,99999).ToString();
            yield return new WaitForSeconds(0.1f);
            uIElements.modeCard.sprite = cardSprites[0];
        }
        yield return new WaitForSeconds(0.8f);
        uIElements.LetsStartBtn.SetActive(true);
        playerElements.playerBotInAnim.sprite = playerElements.playerBotInVsPanel.sprite;
        playerElements.playerBotInJudge.sprite = playerElements.playerBotInVsPanel.sprite;
        playerElements.playerIdInAnim.text = playerElements.playerIdInVsPanel.text;
        oppoElements.oppoBotInAnim.sprite = oppoElements.oppoBotInVsPanel.sprite;
        oppoElements.oppoBotInJudge.sprite = oppoElements.oppoBotInVsPanel.sprite;
        oppoElements.oppoIdInAnim.text = oppoElements.oppoIdInVsPanel.text;
    }

    IEnumerator AddCoins(float delay, int coins)
    {
        yield return new WaitForSeconds(delay);
        if (coinsAdder)
        {
            coinsAdder.addCoins = (GameManager.Instance.selectedLevel + 1) * coins;
            coinsAdder.addNow = true;
        }
    }

    IEnumerator LoadingScene(string str)
    {
        fillBar.fillAmount = 0;
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        SceneManager.LoadScene(str);
    }
    IEnumerator review()
    {
        uIElements.bgImage.sprite = SubmitPanelBGSprites;
        uIElements.SubmitPanel.SetActive(true);
        uIElements.allScroller.SetActive(false);
        uIElements.CoinSlot.SetActive(false);
        uIElements.VideoSlot.SetActive(false);
        uIElements.Preview.SetActive(false);
        uIElements.Home.SetActive(false);
        CharactorMover.Move(new Vector3(-75, -300, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(-75, 200, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(-75, 0, -100), 0.5f, true, false);
    }
    IEnumerator ADTimeCounter()
    {
        yield return new WaitForSeconds(45);
        ADTime = true;
    }
    #endregion

    #region ShowInterstitialAD
    private void CheckInterstitialAD()
    {
        if (MyAdsManager.Instance != null)
        {

            if (MyAdsManager.Instance.IsRewardedAvailable() && canShowInterstitial)
            {
                canShowInterstitial = !canShowInterstitial;
                StartCoroutine(AdDelay(45));
                StartCoroutine(ShowInterstitialAD());
            }
        }
    }

    //private void ShowInterstitial()
    //{
    //    if (MyAdsManager.Instance)
    //    {
    //        MyAdsManager.Instance.ShowInterstitialAds();
    //    }
    //}
    public void ShowAd()
    {
        if (ADTime)
        {
            StartCoroutine(AdDelay());
            ADTime = false;
            StartCoroutine(ADTimeCounter());
        }
    }
    IEnumerator ShowInterstitialAD()
    {
        if (uIElements.AdPenl)
        {
            uIElements.AdPenl.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.AdPenl.SetActive(false);
        }
        ShowInterstitial();
    }
    IEnumerator AdDelay(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        canShowInterstitial = !canShowInterstitial;
    }

    IEnumerator AdDelay()
    {
        if (MyAdsManager.instance)
        {
            if (MyAdsManager.instance.IsRewardedAvailable())
            {
                uIElements.AdPenl.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(1);
                uIElements.AdPenl.gameObject.SetActive(false);
                ShowInterstitial();
            }
        }
    }
    #endregion

    #region EnableOrDisable
    IEnumerator EnableOrDisable(float _Delay, GameObject activateObject, bool isTrue)
    {
        yield return new WaitForSecondsRealtime(_Delay);
        activateObject.SetActive(isTrue);
    }
    IEnumerator EnableAnim(float _Delay, Animator activateObject)
    {
        yield return new WaitForSecondsRealtime(_Delay);
        activateObject.enabled = true;
    }
    #endregion

    // Screenshot saves Android phone photo album
    #region Capture ScreenShot in Gallery
    public void CaptureScreenshot()
    {
        _name = "";
        _name = "Screenshot_" + GetCurTime() + ".jpg";
#if UNITY_STANDALONE_WIN //PC platform
            // under the editor
            // string path = Application.persistentDataPath + "/" + _name;       
            string path = Application.dataPath + "/" + _name;
            ScreenCapture.CaptureScreenshot(path, 0);
            Debug.Log("image save adDress" + path);
​
#elif UNITY_ANDROID //Android Platform
        //Android version
        StartCoroutine(CutImage(_name));
        // Show the path on the phone
        // text.text = "image save adDress" + Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android")) + "/DCIM/Camera/" + _name;
#endif
        ShowToast.showToastMessage("Screenshot Saved in Gallery");
    }

    //Screen capture and save
    IEnumerator CutImage(string name)
    {

        //size of picture     
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        yield return new WaitForEndOfFrame();
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        tex.Apply();
        yield return tex;
        byte[] byt = tex.EncodeToJPG();

        string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android"));
        // File.WriteAllBytes(path + "/DCIM/Camera/" + name, byt); //Save to the Camera folder under DCIM/ on Android phones
        File.WriteAllBytes(path + "/DCIM/Camera/" + name, byt); //Save to the "Screenshot" folder under File Management on Android Phone      
        string[] paths = new string[1];
        paths[0] = path;
        ScanFile(paths);
    }
    //Refresh the image and display it in the album.
    void ScanFile(string[] path)
    {
        using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
            using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
            {
                Conn.CallStatic("scanFile", playerActivity, path, null, null);
            }
        }

    }
    /// <summary>
    /// Get the current year, month, day, hour, minute, second, such as 20181001444
    /// </summary>
    /// <returns></returns>
    string GetCurTime()
    {
        //print(System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm"));
        return System.DateTime.Now.ToString();
    }
    #endregion

    // Screenshot Sharing
    #region Share ScreenShot
    public void Sharing()
    {
        uIElements.PurppleBar.SetActive(false);
        StartCoroutine(TakeScreenShotAndShare());
    }

    IEnumerator TakeScreenShotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tx = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tx.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        tx.Apply();

        string path = Path.Combine(Application.temporaryCachePath, "sharedImage.png");//image name
        File.WriteAllBytes(path, tx.EncodeToPNG());

        Destroy(tx); //to avoid memory leaks

        new NativeShare()
            .AddFile(path)
            //.SetSubject("This is my score")
            //.SetText("share your score with your friends")
            .Share();
        //uIElements.SubmitPanel.SetActive(false); //hide the panel
        uIElements.PurppleBar.SetActive(true);
    }
    #endregion

    #region DressUpOpponent
    private void DressUpOpponent()
    {
        SaveData.Instance.LevelIndex++;
        int randomIndex = 0;
        if(SaveData.Instance.LevelIndex >= 1)
        {          
            if (DressRank > -1)
            {
                randomIndex = Random.Range(0, DressList.Count);
                if (DressList[randomIndex] && oppoElements.oppoDressImage)
                {
                    oppoElements.oppoDressImage.gameObject.SetActive(true);
                    oppoElements.oppoDressImage.sprite = DressSprites[randomIndex];
                }
                DressTotalRank = 10;
                oppoDressTotalRank = GetRank(randomIndex, DressList.Count);
            }

            if (HairRank > -1)
            {
                randomIndex = Random.Range(0, HairList.Count);
                if (HairList[randomIndex] && oppoElements.oppoHairImage)
                {
                    oppoElements.oppoHairImage.gameObject.SetActive(true);
                    oppoElements.oppoHairImage.sprite = HairSprites[randomIndex];
                }
                HairTotalRank = 10;
                oppoHairTotalRank = GetRank(randomIndex, HairList.Count);
            }

            if (LipsRank > -1)
            {
                randomIndex = Random.Range(0, LipsList.Count);
                if (LipsList[randomIndex] && oppoElements.oppoLipsImage)
                {
                    oppoElements.oppoLipsImage.gameObject.SetActive(true);
                    oppoElements.oppoLipsImage.sprite = LipsSprites[randomIndex];
                }
                LipsTotalRank = 10;
                oppoLipsTotalRank = GetRank(randomIndex, LipsList.Count);
            }

        }
        if (SaveData.Instance.LevelIndex >= 2)
        {
            if (BangalRank > -1)
            {
                randomIndex = Random.Range(0, BangalList.Count);
                if (BangalList[randomIndex] && oppoElements.oppoBangalImage)
                {
                    oppoElements.oppoBangalImage.gameObject.SetActive(true);
                    oppoElements.oppoBangalImage.sprite = BangalSprites[randomIndex];
                }
                BangalTotalRank = 10;
                oppoBangalTotalRank = GetRank(randomIndex, BangalList.Count);
            }

            if (NecklesRank > -1)
            {
                randomIndex = Random.Range(0, NecklesList.Count);
                if (NecklesList[randomIndex] && oppoElements.oppoNecklesImage)
                {
                    oppoElements.oppoNecklesImage.gameObject.SetActive(true);
                    oppoElements.oppoNecklesImage.sprite = NecklesSprites[randomIndex];
                }
                NecklesTotalRank = 10;
                oppoNecklesTotalRank = GetRank(randomIndex, NecklesList.Count);
            }

            if (EaringRank > -1)
            {
                randomIndex = Random.Range(0, EaringList.Count);
                if (EaringList[randomIndex] && oppoElements.oppoEaringImage)
                {
                    oppoElements.oppoEaringImage.gameObject.SetActive(true);
                    oppoElements.oppoEaringImage.sprite = EaringSprites[randomIndex];
                }
                EaringTotalRank = 10;
                oppoEaringTotalRank = GetRank(randomIndex, EaringList.Count);
            }
        }
        if (SaveData.Instance.LevelIndex >= 3)
        {
            if (EyeshadeRank > -1)
            {
                randomIndex = Random.Range(0, EyeshadeList.Count);
                if (EyeshadeList[randomIndex] && oppoElements.oppoEyeshadeImage)
                {
                    oppoElements.oppoEyeshadeImage.gameObject.SetActive(true);
                    oppoElements.oppoEyeshadeImage.sprite = EyeshadeSprites[randomIndex];
                }
                EyeshadeTotalRank = 10;
                oppoEyeshadeTotalRank = GetRank(randomIndex, EyeshadeList.Count);
            }

            if (ShoesRank > -1)
            {
                randomIndex = Random.Range(0, ShoesList.Count);
                if (ShoesList[randomIndex] && oppoElements.oppoShoesImage)
                {
                    oppoElements.oppoShoesImage.gameObject.SetActive(true);
                    oppoElements.oppoShoesImage.sprite = ShoesSprites[randomIndex];
                }
                ShoesTotalRank = 10;
                oppoShoesTotalRank = GetRank(randomIndex, ShoesList.Count);
            }

            if (CrownRank > -1)
            {
                randomIndex = Random.Range(0, CrownList.Count);
                if (CrownList[randomIndex] && oppoElements.oppoCrownImage)
                {
                    oppoElements.oppoCrownImage.gameObject.SetActive(true);
                    oppoElements.oppoCrownImage.sprite = CrownSprites[randomIndex];
                }
                CrownTotalRank = 10;
                oppoCrownTotalRank = GetRank(randomIndex, CrownList.Count);
            }
        }

        if (SaveData.Instance.LevelIndex >= 4)
        {
            if (BouqetRank > -1)
            {
                randomIndex = Random.Range(0, BouqetList.Count);
                if (BouqetList[randomIndex] && oppoElements.oppoBouqetImage)
                {
                    oppoElements.oppoBouqetImage.gameObject.SetActive(true);
                    oppoElements.oppoBouqetImage.sprite = BouqetSprites[randomIndex];
                }
                BouqetTotalRank = 10;
                oppoBouqetTotalRank = GetRank(randomIndex, BouqetList.Count);
            }

            if (VeilRank > -1)
            {
                randomIndex = Random.Range(0, VeilList.Count);
                if (VeilList[randomIndex] && oppoElements.oppoVeilImage)
                {
                    oppoElements.oppoVeilImage.gameObject.SetActive(true);
                    oppoElements.oppoVeilImage.sprite = VeilSprites[randomIndex];
                }
                VeilTotalRank = 10;
                oppoVeilTotalRank = GetRank(randomIndex, VeilList.Count);
            }

            if (BagRank > -1)
            {
                randomIndex = Random.Range(0, BagList.Count);
                if (BagList[randomIndex] && oppoElements.oppoBagImage)
                {
                    oppoElements.oppoBagImage.gameObject.SetActive(true);
                    oppoElements.oppoBagImage.sprite = BagSprites[randomIndex];
                }
                BagTotalRank = 10;
                oppoBagTotalRank = GetRank(randomIndex, BagList.Count);
            }

            if (BlushRank > -1)
            {
                randomIndex = Random.Range(0, BlushList.Count);
                if (BlushList[randomIndex] && oppoElements.oppoBlushImage)
                {
                    oppoElements.oppoBlushImage.gameObject.SetActive(true);
                    oppoElements.oppoBlushImage.sprite = BlushSprites[randomIndex];
                }
                BlushTotalRank = 10;
                oppoBlushTotalRank = GetRank(randomIndex, BlushList.Count);
            }

            SaveData.Instance.LevelIndex = 1;
        }
    }
    #endregion

    #region Ranking Formula
    private int GetRank(int selectedCard, int totalItems)
    {
        int rankDivider = 0;
        rankDivider = totalItems / 10;
        if (rankDivider == 0)
        {
            rankDivider += 1;
        }
        if (selectedCard / rankDivider < 10)
        {
            return (selectedCard / rankDivider) + 1;
        }
        else
        {
            return 10;
        }
    }
    private int GetRankValue(int _Rank)
    {
        if (_Rank > -1)
            return _Rank;
        else
            return 0;
    }
    #endregion

    #region Comparing
    IEnumerator StartComparing(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        int totalRank = 0;
        yield return new WaitForSeconds(1);
        CompareAnim.enabled = true;
        CardAnimation.enabled = true;
        Comparetext.text = "Dress";
        AnimatorPlayer();
        Compareing.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(DressRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, DressTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoDressTotalRank, DressTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Veil";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(VeilRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, VeilTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoVeilTotalRank, VeilTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Neckles";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(NecklesRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, NecklesTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoNecklesTotalRank, NecklesTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bouqets";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BouqetRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, BouqetTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoBouqetTotalRank, BouqetTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bangal";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BangalRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, BangalTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoBangalTotalRank, BangalTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Shoes";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(ShoesRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, ShoesTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoShoesTotalRank, ShoesTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Hairs";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(HairRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, HairTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoHairTotalRank, HairTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Lips";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(LipsRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, LipsTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoLipsTotalRank, LipsTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Crowns";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(CrownRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, CrownTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoCrownTotalRank, CrownTotalRank).ToString();
        playerTotal = int.Parse(playerElements.playerVoteText.text);
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Eyeshades";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EyeshadeRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, EyeshadeTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoEyeshadeTotalRank, EyeshadeTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Earings";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EaringRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, EaringTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoEaringTotalRank, EaringTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bag";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BagRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, BagTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoBagTotalRank, BagTotalRank).ToString();
        UpdatingValue();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Blush";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BlushRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, BlushTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoBlushTotalRank, BlushTotalRank).ToString();
        UpdatingValue();

        Compareing.SetActive(false);
        yield return new WaitForSeconds(2f);
        uIElements.ObjectOff.SetActive(false);
        if (playerTotalMarks >= opponentTotalMarks)
        {
            yield return new WaitForSeconds(1f);
            if (WinnerSFX) WinnerSFX.Play();
            CharactorMover.transform.SetSiblingIndex(1);
            yield return new WaitForSeconds(1f);
            OpponentMover.Move(new Vector3(1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            CharactorMover.Move(new Vector3(-85, 83, 0), 0.5f, true, false);
            yield return new WaitForSeconds(1f);
            uIElements.Winner.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.CoinSlot.SetActive(true);
            //yield return new WaitForSeconds(1f);
            StartCoroutine(AddCoins(0.7f, 2000));
            yield return new WaitForSeconds(1f);
            FinalPartical.gameObject.SetActive(true);
            //if (FinalPartical) FinalPartical.Play();
            yield return new WaitForSeconds(7f);
            //FinalPartical.gameObject.SetActive(false);
            //yield return new WaitForSeconds(0.5f);
            uIElements.WinLevelCompletePanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            OpponentMover.transform.SetSiblingIndex(-1);
            if (LoseSFX) LoseSFX.Play();
            yield return new WaitForSeconds(1f);
            CharactorMover.Move(new Vector3(-1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            OpponentMover.Move(new Vector3(40, 83, 0), 0.5f, true, false);
            yield return new WaitForSeconds(1f);
            uIElements.Winner.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.CoinSlot.SetActive(true);
            //yield return new WaitForSeconds(1f);
            StartCoroutine(AddCoins(0.7f, 500));
            yield return new WaitForSeconds(2f);
            uIElements.LoseLevelCompletePanel.SetActive(true);

        }
    }
    #endregion

    public void UpdatingValue()
    {
        playerTotal = int.Parse(playerElements.playerVoteText.text);
        playerTotalMarks += playerTotal;
        PlayerScoreText.text = playerTotalMarks.ToString();
        //PlayerScoreText.text = playerTotal + " / 10".ToString();
        opponentTotal = int.Parse(oppoElements.oppoVoteText.text);
        opponentTotalMarks += opponentTotal;
        OpponentScoreText.text = opponentTotalMarks.ToString();
        //OpponentScoreText.text = opponentTotal + " / 10".ToString();
    }
    public void AnimatorPlayer()
    {
        CompareAnim.Play(0);
        CardAnimation.Play(0);
    }
}
