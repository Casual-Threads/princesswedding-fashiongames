using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;

[System.Serializable]
public class PartyDressUpElements
{
    [Header("Panels")]
    public GameObject GamePanel;
    public GameObject VsPanel, CoinNotAvailablePanel, VideoNotAvailablePanel, WinLevelCompletePanel, LoseLevelCompletePanel, SubmitPanel, JudgementPanel, AdPenl, ObjectOff;
    [Header("Scrollers")]
    public GameObject allScroller;
    public GameObject categoryScroller, dressUpScroller, makeUpScroller, itemsScroller, DressScroller, EaringScroller, NecklesScroller, BangalScroller, BouqetScroller, ShoesScroller,
                      HairScroller, LipsScroller, ShortScroller, TopScroller, EyeshadeScroller, BagScroller, BlushScroller;
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
public class PartyDressUpPlayerElements
{
    [Header("Player Character")]
    public GameObject PlayerCharactar;
    [Header("Player Images")]
    public Image DressImage;
    public Image EaringImage, NecklesImage, BangalImage, BouqetImage, ShoesImage, HairImage, LipsImage, ShortImage, TopImage, EyeshadeImage, BagImage, BlushImage;
    [Header("Player Bot Images")]
    public Image playerBotInVsPanel;
    public Image playerBotInAnim, playerBotInJudge;
    [Header("Player Text")]
    public Text playerIdInVsPanel;
    public Text playerIdInAnim, playerVoteText;
}

[System.Serializable]
public class PartyDressUpOppnentElements
{
    [Header("Bot & Opponent")]
    public GameObject OpponentCharactar;
    [Header("Opponent Images")]
    public Image oppoDressImage;
    public Image oppoTopImage, oppoNecklesImage, oppoBouqetImage, oppoBangalImage, oppoShoesImage, oppoHairImage, oppoLipsImage,
                 oppoShortImage, oppoEyeshadeImage, oppoEaringImage, oppoBagImage, oppoBlushImage;
    [Header("Opponent Bot Images")]
    public Image oppoBotInVsPanel;
    public Image oppoBotInAnim, oppoBotInJudge;
    [Header("Opponent Text")]
    public Text oppoIdInVsPanel;
    public Text oppoIdInAnim, oppoVoteText;
}

[System.Serializable]
public enum PartyDressUpSelectedItem
{
    Dress, Earing, Bangal, Top, Short, Neckles, Bouqet, Bag, Shoes, Hair, Lips, Eyeshade, Blush
}

public class PartyDressUp : MonoBehaviour
{
    [FoldoutGroup("UI Elements")]
    [HideLabel]
    public PartyDressUpElements uIElements;
    [FoldoutGroup("Player Elements")]
    [HideLabel]
    public PartyDressUpPlayerElements playerElements;
    [FoldoutGroup("Opponent Elements")]
    [HideLabel]
    public PartyDressUpOppnentElements oppoElements;
    public PartyDressUpSelectedItem selectedItem;
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
    public Sprite[] TopSprites;
    public Sprite[] NecklesSprites;
    public Sprite[] BouqetSprites;
    public Sprite[] BangalSprites;
    public Sprite[] ShoesSprites;
    public Sprite[] HairSprites;
    public Sprite[] LipsSprites;
    public Sprite[] ShortSprites;
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
    private List<ItemInfo> TopList = new List<ItemInfo>();
    private List<ItemInfo> NecklesList = new List<ItemInfo>();
    private List<ItemInfo> BouqetList = new List<ItemInfo>();
    private List<ItemInfo> BangalList = new List<ItemInfo>();
    private List<ItemInfo> ShoesList = new List<ItemInfo>();
    private List<ItemInfo> HairList = new List<ItemInfo>();
    private List<ItemInfo> LipsList = new List<ItemInfo>();
    private List<ItemInfo> ShortList = new List<ItemInfo>();
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
    //public AudioSource CompSFX;
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
    public Text /*TotalCoinsLevelComp,*/ PlayerScoreText, OpponentScoreText, Comparetext;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;
    [Header("Different Index")]
    private int selectedIndex/*, bgindex*/;
    [Header("Bool Variable")]
    bool ADTime = true;
    private bool canShowInterstitial;
    bool IsDressTrue, IsHairTrue, IsShoesTrue = false;
    [Header("Different Rank")]
    private int DressRank, TopRank, NecklesRank, BouqetRank, BangalRank, ShoesRank, HairRank, LipsRank, ShortRank, EyeshadeRank, EaringRank, BagRank, BlushRank;
    private int DressTotalRank, TopTotalRank, NecklesTotalRank, BouqetTotalRank, BangalTotalRank, ShoesTotalRank, HairTotalRank,
                LipsTotalRank, ShortTotalRank, EyeshadeTotalRank, EaringTotalRank, BagTotalRank, BlushTotalRank;
    private int oppoDressTotalRank, oppoTopTotalRank, oppoNecklesTotalRank, oppoBouqetTotalRank, oppoBangalTotalRank, oppoShoesTotalRank,
                oppoHairTotalRank, oppoLipsTotalRank, oppoShortTotalRank, oppoEyeshadeTotalRank, oppoEaringTotalRank, oppoBagTotalRank, oppoBlushTotalRank;
    [Header("Int Variable")]
    private int playerTotal = 0;
    private int opponentTotal = 0, playerTotalMarks = 0, opponentTotalMarks = 0;
    private int playerSpriteIndex;
    private int oppoSpriteIndex;
    //private int randomNumber = 0;
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
            GameManager.Instance.randomMode = 2;
        }

        selectedItem = PartyDressUpSelectedItem.Dress;
        uIElements.DressScroller.SetActive(true);
        TotalCoins.text = SaveData.Instance.Coins.ToString();
        //TotalCoinsLevelComp.text = SaveData.Instance.Coins.ToString();
        StartCoroutine(FindOpponent());
        SetInitialValues();
        GetItemsInfo();
        StartCoroutine(AdDelay(45));
        DressRank = HairRank = LipsRank = EyeshadeRank = 1;
        TopRank = NecklesRank = BouqetRank = BangalRank = ShoesRank = ShortRank = EaringRank = BagRank = BlushRank = -1;
    }
    public void ShowInterstitial()
    {
        if (MyAdsManager.instance)
        {
            MyAdsManager.instance.ShowInterstitialAds();
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.DressLocked, DressList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.EaringLocked, EaringList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.BangalLocked, BangalList);
        SetItemIcon(BangalList, BangalSprites);
        #endregion

        #region Initialing Top
        if (uIElements.TopScroller)
        {
            var TopInfo = uIElements.TopScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < TopInfo.Length; i++)
            {
                TopList.Add(TopInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.partyDressUpModeElements.TopLocked, TopList);
        SetItemIcon(TopList, TopSprites);
        #endregion

        #region Initialing Short
        if (uIElements.ShortScroller)
        {
            var ShortInfo = uIElements.ShortScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < ShortInfo.Length; i++)
            {
                ShortList.Add(ShortInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.partyDressUpModeElements.ShortLocked, ShortList);
        SetItemIcon(ShortList, ShortSprites);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.NecklesLocked, NecklesList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.BouqetLocked, BouqetList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.BagLocked, BagList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.ShoesLocked, ShoesList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.HairLocked, HairList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.LipsLocked, LipsList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.EyeshadeLocked, EyeshadeList);
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
        SetupItemData(SaveData.Instance.partyDressUpModeElements.BlushLocked, BlushList);
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
        if (selectedItem == PartyDressUpSelectedItem.Dress)
        {
            CheckSelectedItem(DressList, DressSprites, playerElements.DressImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Top)
        {
            CheckSelectedItem(TopList, TopSprites, playerElements.TopImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Neckles)
        {
            CheckSelectedItem(NecklesList, NecklesSprites, playerElements.NecklesImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bouqet)
        {
            CheckSelectedItem(BouqetList, BouqetSprites, playerElements.BouqetImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bangal)
        {
            CheckSelectedItem(BangalList, BangalSprites, playerElements.BangalImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Shoes)
        {
            CheckSelectedItem(ShoesList, ShoesSprites, playerElements.ShoesImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Hair)
        {
            CheckSelectedItem(HairList, HairSprites, playerElements.HairImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Lips)
        {
            CheckSelectedItem(LipsList, LipsSprites, playerElements.LipsImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Short)
        {
            CheckSelectedItem(ShortList, ShortSprites, playerElements.ShortImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Eyeshade)
        {
            CheckSelectedItem(EyeshadeList, EyeshadeSprites, playerElements.EyeshadeImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Earing)
        {
            CheckSelectedItem(EaringList, EaringSprites, playerElements.EaringImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bag)
        {
            CheckSelectedItem(BagList, BagSprites, playerElements.BagImage);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Blush)
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
                            if (selectedItem == PartyDressUpSelectedItem.Dress)
                            {
                                IsDressTrue = true;
                                GameManager.Instance.partyDress = selectedIndex;
                                playerElements.DressImage.gameObject.SetActive(true);
                                playerElements.TopImage.gameObject.SetActive(false);
                                playerElements.ShortImage.gameObject.SetActive(false);
                                DressRank = GetRank(selectedIndex, DressList.Count);
                                GameManager.Instance.partyDressIndex = DressRank;
                                GameManager.Instance.parDress = true;
                                //GameManager.Instance.parTop = GameManager.Instance.parTop;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Top)
                            {
                                GameManager.Instance.partyTop = selectedIndex;
                                playerElements.DressImage.gameObject.SetActive(false);
                                playerElements.TopImage.gameObject.SetActive(true);
                                playerElements.ShortImage.gameObject.SetActive(true);
                                TopRank = GetRank(selectedIndex, TopList.Count);
                                GameManager.Instance.partyTopIndex = TopRank;
                                GameManager.Instance.parTop = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Neckles)
                            {
                                GameManager.Instance.partyNeckles = selectedIndex;
                                NecklesRank = GetRank(selectedIndex, NecklesList.Count);
                                GameManager.Instance.partyNecklesIndex = NecklesRank;
                                GameManager.Instance.parNeckles = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Bouqet)
                            {
                                GameManager.Instance.partyBouqet = selectedIndex;
                                playerElements.BouqetImage.gameObject.SetActive(true);
                                playerElements.BagImage.gameObject.SetActive(false);
                                BouqetRank = GetRank(selectedIndex, BouqetList.Count);
                                GameManager.Instance.partyBouqetIndex = BouqetRank;
                                GameManager.Instance.parBouqet = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Bangal)
                            {
                                GameManager.Instance.partyBangal = selectedIndex;
                                BangalRank = GetRank(selectedIndex, BangalList.Count);
                                GameManager.Instance.partyBangalIndex = BangalRank;
                                GameManager.Instance.parBangal = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Shoes)
                            {
                                IsShoesTrue = true;
                                GameManager.Instance.partyShoes = selectedIndex;
                                ShoesRank = GetRank(selectedIndex, ShoesList.Count);
                                GameManager.Instance.partyShoesIndex = ShoesRank;
                                GameManager.Instance.parShoes = true;
                                print(GameManager.Instance.parShoes);
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Hair)
                            {
                                IsHairTrue = true;
                                GameManager.Instance.partyHair = selectedIndex;
                                HairRank = GetRank(selectedIndex, HairList.Count);
                                GameManager.Instance.partyHairIndex = HairRank;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Lips)
                            {
                                GameManager.Instance.partyLips = selectedIndex;
                                LipsRank = GetRank(selectedIndex, LipsList.Count);
                                GameManager.Instance.partyLipsIndex = LipsRank;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Short)
                            {
                                GameManager.Instance.partyShort = selectedIndex;
                                playerElements.DressImage.gameObject.SetActive(false);
                                playerElements.TopImage.gameObject.SetActive(true);
                                playerElements.ShortImage.gameObject.SetActive(true);
                                ShortRank = GetRank(selectedIndex, ShortList.Count);
                                GameManager.Instance.partyShortIndex = ShortRank;
                                GameManager.Instance.parShort = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Eyeshade)
                            {
                                GameManager.Instance.partyEyeshade = selectedIndex;
                                EyeshadeRank = GetRank(selectedIndex, EyeshadeList.Count);
                                GameManager.Instance.partyEyeshadeIndex = EyeshadeRank;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Earing)
                            {
                                GameManager.Instance.partyEaring = selectedIndex;
                                EaringRank = GetRank(selectedIndex, EaringList.Count);
                                GameManager.Instance.partyEaringIndex = EaringRank;
                                GameManager.Instance.parEaring = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Bag)
                            {
                                GameManager.Instance.partyBag = selectedIndex;
                                playerElements.BagImage.gameObject.SetActive(true);
                                playerElements.BouqetImage.gameObject.SetActive(false);
                                BagRank = GetRank(selectedIndex, BagList.Count);
                                GameManager.Instance.partyBagIndex = BagRank;
                                GameManager.Instance.parBag = true;
                            }
                            else if (selectedItem == PartyDressUpSelectedItem.Blush)
                            {
                                GameManager.Instance.partyBlush = selectedIndex;
                                BlushRank = GetRank(selectedIndex, BlushList.Count);
                                GameManager.Instance.partyBlushIndex = BlushRank;
                                GameManager.Instance.parBlush = true;
                            }

                            if (IsDressTrue == true && IsHairTrue == true && IsShoesTrue == true)
                            {
                                if (uIElements.Preview) uIElements.Preview.gameObject.SetActive(true);
                            }
                            itemImage.gameObject.SetActive(false);
                            itemImage.gameObject.SetActive(true);
                            itemImage.sprite = itemSprites[selectedIndex];
                            //if (uIElements.unEquip) uIElements.unEquip.gameObject.SetActive(true);
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
        uIElements.ShortScroller.SetActive(false);
        uIElements.EyeshadeScroller.SetActive(false);
        uIElements.BangalScroller.SetActive(false);
        uIElements.TopScroller.SetActive(false);
        uIElements.EaringScroller.SetActive(false);
        uIElements.BagScroller.SetActive(false);
        uIElements.BlushScroller.SetActive(false);
    }
    public void SelectedCatagory(int index)
    {

        DisableScrollers();
        if (CategorySFX) CategorySFX.Play();
        CategoryImage[index].sprite = CategorySelectedSprites[index];
        if (index == (int)PartyDressUpSelectedItem.Dress)
        {
            selectedItem = PartyDressUpSelectedItem.Dress;
            uIElements.DressScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, 100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Hair)
        {
            selectedItem = PartyDressUpSelectedItem.Hair;
            uIElements.HairScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Bouqet)
        {
            selectedItem = PartyDressUpSelectedItem.Bouqet;
            uIElements.BouqetScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, 100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Neckles)
        {
            selectedItem = PartyDressUpSelectedItem.Neckles;
            uIElements.NecklesScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Lips)
        {
            selectedItem = PartyDressUpSelectedItem.Lips;
            uIElements.LipsScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Shoes)
        {
            selectedItem = PartyDressUpSelectedItem.Shoes;
            uIElements.ShoesScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -300, 0), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Eyeshade)
        {
            selectedItem = PartyDressUpSelectedItem.Eyeshade;
            uIElements.EyeshadeScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Bangal)
        {
            selectedItem = PartyDressUpSelectedItem.Bangal;
            uIElements.BangalScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, -100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Top)
        {
            selectedItem = PartyDressUpSelectedItem.Top;
            uIElements.TopScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, -100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Short)
        {
            selectedItem = PartyDressUpSelectedItem.Short;
            uIElements.ShortScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, -100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Earing)
        {
            selectedItem = PartyDressUpSelectedItem.Earing;
            uIElements.EaringScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Bag)
        {
            selectedItem = PartyDressUpSelectedItem.Bag;
            uIElements.BagScroller.SetActive(true);
            CharactorMover.Move(new Vector3(170, -330, -100), 0.5f, true, false);
        }
        else if (index == (int)PartyDressUpSelectedItem.Blush)
        {
            selectedItem = PartyDressUpSelectedItem.Blush;
            uIElements.BlushScroller.SetActive(true);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);
        }
        GetItemsInfo();
    }
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        if (selectedItem == PartyDressUpSelectedItem.Dress)
        {
            SetItemsInfo(DressList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Hair)
        {
            SetItemsInfo(HairList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bouqet)
        {
            SetItemsInfo(BouqetList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Shoes)
        {
            SetItemsInfo(ShoesList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Neckles)
        {
            SetItemsInfo(NecklesList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Lips)
        {
            SetItemsInfo(LipsList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Eyeshade)
        {
            SetItemsInfo(EyeshadeList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bangal)
        {
            SetItemsInfo(BangalList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Top)
        {
            SetItemsInfo(TopList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Short)
        {
            SetItemsInfo(ShortList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Earing)
        {
            SetItemsInfo(EaringList);
        } 
        else if (selectedItem == PartyDressUpSelectedItem.Bag)
        {
            SetItemsInfo(BagList);
        }
        else if (selectedItem == PartyDressUpSelectedItem.Blush)
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
        if (selectedItem == PartyDressUpSelectedItem.Dress)
        {
            SaveData.Instance.partyDressUpModeElements.DressLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Hair)
        {
            SaveData.Instance.partyDressUpModeElements.HairLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bouqet)
        {
            SaveData.Instance.partyDressUpModeElements.BouqetLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Shoes)
        {
            SaveData.Instance.partyDressUpModeElements.ShoesLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Neckles)
        {
            SaveData.Instance.partyDressUpModeElements.NecklesLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Lips)
        {
            SaveData.Instance.partyDressUpModeElements.LipsLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Eyeshade)
        {
            SaveData.Instance.partyDressUpModeElements.EyeshadeLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bangal)
        {
            SaveData.Instance.partyDressUpModeElements.BangalLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Top)
        {
            SaveData.Instance.partyDressUpModeElements.TopLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Short)
        {
            SaveData.Instance.partyDressUpModeElements.ShortLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Earing)
        {
            SaveData.Instance.partyDressUpModeElements.EaringLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Bag)
        {
            SaveData.Instance.partyDressUpModeElements.BagLocked[selectedIndex] = false;
        }
        else if (selectedItem == PartyDressUpSelectedItem.Blush)
        {
            SaveData.Instance.partyDressUpModeElements.BlushLocked[selectedIndex] = false;
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
        if (FinalPartical) FinalPartical.gameObject.SetActive(false);
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
        uIElements.SubmitPanel.SetActive(false);
        uIElements.allScroller.SetActive(true);
        uIElements.CoinSlot.SetActive(true);
        uIElements.VideoSlot.SetActive(true);
        CharactorMover.Move(new Vector3(53, -92, -100), 0.5f, true, false);
    }
    public void Review()
    {
        ShowInterstitial();
        StartCoroutine(review());

    }
    public void Comparesion()
    {
        //ShowInterstitial();
        //GameManager.Instance.randomMode = 2;
        SceneManager.LoadScene("ComparsionMode");
    }

    public void Submit()
    {
        ShowInterstitial();
        OpponentMover.transform.localScale = new Vector3(-1, 1, 1);
        uIElements.bgImage.sprite = JudgementPanelBGSprites;
        uIElements.JudgementPanel.SetActive(true);
        uIElements.allScroller.SetActive(false);
        uIElements.CoinSlot.SetActive(false);
        uIElements.VideoSlot.SetActive(false);
        uIElements.SubmitPanel.SetActive(false);
        oppoElements.OpponentCharactar.SetActive(true);
        CharactorMover.Move(new Vector3(-270, -110, 100), 0.5f, true, false);
        OpponentMover.Move(new Vector3(270, -110, 100), 0.5f, true, false);
        DressUpOpponent();
        SaveData.instance.LevelsUnlocked++;
        StartCoroutine(StartComparing(3f));

    }
    public void SubmitBtn()
    {
        if (GameManager.Instance.ModeSelected == 0)
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
            selectedItem = PartyDressUpSelectedItem.Hair;
            uIElements.MakeUpBtn.SetActive(false);
            uIElements.DressUpBtn.SetActive(true);
            uIElements.makeUpScroller.SetActive(true);
            uIElements.dressUpScroller.SetActive(false);
            uIElements.HairScroller.SetActive(true);
            uIElements.DressScroller.SetActive(false);
            CharactorMover.Move(new Vector3(17, -527, -900), 0.5f, true, false);

        }
        else if (index == 1)
        {
            selectedItem = PartyDressUpSelectedItem.Dress;
            uIElements.MakeUpBtn.SetActive(true);
            uIElements.DressUpBtn.SetActive(false);
            uIElements.dressUpScroller.SetActive(true);
            uIElements.makeUpScroller.SetActive(false);
            uIElements.HairScroller.SetActive(false);
            uIElements.DressScroller.SetActive(true);
            CharactorMover.Move(new Vector3(53, -92, -100), 0.5f, true, false);

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
        CharactorMover.Move(new Vector3(0, -300, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(0, 200, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(0, 0, -100), 0.5f, true, false);
        yield return new WaitForSeconds(1f);
        uIElements.Winner.SetActive(true);
        yield return new WaitForSeconds(1f);
        uIElements.CoinSlot.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(AddCoins(0.7f, 1000));
        yield return new WaitForSeconds(1f);
        FinalPartical.gameObject.SetActive(true);
        //if (FinalPartical) FinalPartical.Play();
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
            playerSpriteIndex = Random.Range(0, playerBotImageSprites.Length);
            playerElements.playerBotInVsPanel.sprite = playerBotImageSprites[playerSpriteIndex];
            oppoSpriteIndex = Random.Range(0, oppoBotImageSprites.Length);
            oppoElements.oppoBotInVsPanel.sprite = oppoBotImageSprites[oppoSpriteIndex];
            playerElements.playerIdInVsPanel.text = Random.Range(10000,99999).ToString();
            oppoElements.oppoIdInVsPanel.text = Random.Range(10000,99999).ToString();
            yield return new WaitForSeconds(0.1f);
            uIElements.modeCard.sprite = cardSprites[1];
        }
        yield return new WaitForSeconds(0.8f);
        uIElements.LetsStartBtn.SetActive(true);
        GameManager.Instance.PlayerBotImage = playerSpriteIndex;
        playerElements.playerBotInAnim.sprite = playerElements.playerBotInVsPanel.sprite;
        playerElements.playerBotInJudge.sprite = playerElements.playerBotInVsPanel.sprite;
        playerElements.playerIdInAnim.text = playerElements.playerIdInVsPanel.text;
        GameManager.Instance.OpponentBotImage = oppoSpriteIndex;
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
        CharactorMover.Move(new Vector3(0, -300, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(0, 200, -800), 0.7f, true, false);
        yield return new WaitForSeconds(0.71f);
        CharactorMover.Move(new Vector3(0, 0, -100), 0.5f, true, false);
    }
    IEnumerator ObjectActivation(GameObject obj, float delay, bool enable)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(enable);
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

            if (MyAdsManager.Instance.IsInterstitialAvailable() && canShowInterstitial)
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
            if (MyAdsManager.instance.IsInterstitialAvailable())
            {
                uIElements.AdPenl.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(1);
                uIElements.AdPenl.gameObject.SetActive(false);
                ShowInterstitial();
            }
        }
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
                    oppoElements.oppoTopImage.gameObject.SetActive(false);
                    oppoElements.oppoShortImage.gameObject.SetActive(false);
                    oppoElements.oppoDressImage.sprite = DressSprites[randomIndex];

                }
                DressTotalRank = 10;
                oppoDressTotalRank = GetRank(randomIndex, DressList.Count);
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
        }
        if(SaveData.Instance.LevelIndex >= 2)
        {
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
        }
        if(SaveData.Instance.LevelIndex >= 3)
        {
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

            if (TopRank > -1)
            {
                randomIndex = Random.Range(0, TopList.Count);
                if (TopList[randomIndex] && oppoElements.oppoTopImage)
                {
                    oppoElements.oppoTopImage.gameObject.SetActive(true);
                    oppoElements.oppoShortImage.gameObject.SetActive(true);
                    oppoElements.oppoDressImage.gameObject.SetActive(false);
                    oppoElements.oppoTopImage.sprite = TopSprites[randomIndex];
                }
                TopTotalRank = 10;
                oppoTopTotalRank = GetRank(randomIndex, TopList.Count);
            }

            if (ShortRank > -1)
            {
                randomIndex = Random.Range(0, ShortList.Count);
                if (ShortList[randomIndex] && oppoElements.oppoShortImage)
                {
                    oppoElements.oppoTopImage.gameObject.SetActive(true);
                    oppoElements.oppoShortImage.gameObject.SetActive(true);
                    oppoElements.oppoDressImage.gameObject.SetActive(false);
                    oppoElements.oppoShortImage.sprite = ShortSprites[randomIndex];
                }
                ShortTotalRank = 10;
                oppoShortTotalRank = GetRank(randomIndex, ShortList.Count);
            }
        }
        if(SaveData.Instance.LevelIndex >= 4)
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

            if (BouqetRank > -1)
            {
                randomIndex = Random.Range(0, BouqetList.Count);
                if (BouqetList[randomIndex] && oppoElements.oppoBouqetImage)
                {
                    oppoElements.oppoBagImage.gameObject.SetActive(false);
                    oppoElements.oppoBouqetImage.gameObject.SetActive(true);
                    oppoElements.oppoBouqetImage.sprite = BouqetSprites[randomIndex];
                }
                BouqetTotalRank = 10;
                oppoBouqetTotalRank = GetRank(randomIndex, BouqetList.Count);
            }

            if (BagRank > -1)
            {
                randomIndex = Random.Range(0, BagList.Count);
                if (BagList[randomIndex] && oppoElements.oppoBagImage)
                {
                    oppoElements.oppoBagImage.gameObject.SetActive(true);
                    oppoElements.oppoBouqetImage.gameObject.SetActive(false);
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
        Comparetext.text = "Top";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(TopRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, TopTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoTopTotalRank, TopTotalRank).ToString();
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
        Comparetext.text = "Shorts";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(ShortRank);
        playerTotal += totalRank;
        playerElements.playerVoteText.text = GetRank(totalRank, ShortTotalRank).ToString();
        oppoElements.oppoVoteText.text = GetRank(oppoShortTotalRank, ShortTotalRank).ToString();
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
            CharactorMover.Move(new Vector3(0, -30, 100), 0.5f, true, false);
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
            if (LoseSFX) LoseSFX.Play();
            OpponentMover.transform.SetSiblingIndex(-1);
            yield return new WaitForSeconds(1f);
            CharactorMover.Move(new Vector3(-1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            OpponentMover.Move(new Vector3(-67, -30, 100), 0.5f, true, false);
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
        opponentTotal = int.Parse(oppoElements.oppoVoteText.text);
        //PlayerScoreText.text = playerTotal + " / 10".ToString();
        opponentTotalMarks += opponentTotal;
        OpponentScoreText.text = opponentTotalMarks.ToString();
        //OpponentScoreText.text = opponentTotal + " / 10".ToString();
    }
    public void AnimatorPlayer()
    {
        CompareAnim.Play(0);
        CardAnimation.Play(0);
    }

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

}
