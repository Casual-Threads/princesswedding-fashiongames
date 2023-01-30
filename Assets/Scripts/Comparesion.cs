using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System.IO;

[System.Serializable]
public class ComparesionElements
{
    [Header("Panels")]
    public GameObject GamePanel;
    public GameObject VideoNotAvailablePanel, WinLevelCompletePanel, LoseLevelCompletePanel, JudgementPanel;
    [Header("UI")]
    public GameObject CoinSlot;
    public GameObject ObjectOff, Winner;
    [Header("BG Image")]
    public Image bgImage;
}

[System.Serializable]
public class ComparesionWeddingElements
{
    [Header("Wedding Images")]
    public Image weddingDressImage;
    public Image weddingEaringImage, weddingNecklesImage, weddingBangalImage, weddingBouqetImage, weddingShoesImage, weddingHairImage, weddingLipsImage, weddingCrownImage,
                 weddingVeilImage, weddingEyeshadeImage, weddingBagImage, weddingBlushImage;
    [Header("Player Bot Images")]
    public Image playerBotInJudge;
}

[System.Serializable]
public class ComparesionPartyElements
{
    [Header("Party Images")]
    public Image partyDressImage;
    public Image partyShortImage, partyNecklesImage, partyBouqetImage, partyBangalImage, partyShoesImage, partyHairImage, partyLipsImage,
                 partyTopImage, partyEyeshadeImage, partyEaringImage, partyBagImage, partyBlushImage;
    [Header("Opponent Bot Images")]
    public Image oppoBotInJudge;

}

public class Comparesion : MonoBehaviour
{
    [FoldoutGroup("UI Elements")]
    [HideLabel]
    public ComparesionElements uIElements;
    public ComparesionWeddingElements weddingElements;
    public ComparesionPartyElements partyElements;
    [Header("Loading")]
    public GameObject LoadingPanel;
    public Image fillBar;
    [Header("Mover Item")]
    public MRS_Manager WeddingCharactorMover;
    public MRS_Manager PartyCharactorMover;
    public CoinsAdder coinsAdder;
    [FoldoutGroup("Sprites Arrays")]
    //[HideLabel]
    public Sprite[] playerBotImageSprites;
    public Sprite[] oppoBotImageSprites;
    [Header("Wedding Arrays")]
    public Sprite[] weddingDressSprites;
    public Sprite[] weddingVeilSprites;
    public Sprite[] weddingNecklesSprites;
    public Sprite[] weddingBouqetSprites;
    public Sprite[] weddingBangalSprites;
    public Sprite[] weddingShoesSprites;
    public Sprite[] weddingHairSprites;
    public Sprite[] weddingLipsSprites;
    public Sprite[] weddingCrownSprites;
    public Sprite[] weddingEyeshadeSprites;
    public Sprite[] weddingEaringSprites;
    public Sprite[] weddingBagSprites;
    public Sprite[] weddingBlushSprites;
    [Header("Party Arrays")]
    public Sprite[] partyDressSprites;
    public Sprite[] partyTopSprites;
    public Sprite[] partyNecklesSprites;
    public Sprite[] partyBouqetSprites;
    public Sprite[] partyBangalSprites;
    public Sprite[] partyShoesSprites;
    public Sprite[] partyHairSprites;
    public Sprite[] partyLipsSprites;
    public Sprite[] partyShortSprites;
    public Sprite[] partyEyeshadeSprites;
    public Sprite[] partyEaringSprites;
    public Sprite[] partyBagSprites;
    public Sprite[] partyBlushSprites;
    [Header("Audio")]
    public AudioSource CoinsSFX;
    public AudioSource WinnerSFX;
    public AudioSource LoseSFX;
    [Header("Particals")]
    public GameObject FinalPartical;
    [Header("Player Text")]
    public Text playerVoteText;
    [Header("Opponent Text")]
    public Text oppoVoteText;
    [Header("Text")]
    public Text TotalCoins;
    public Text  PlayerScoreText, OpponentScoreText, Comparetext;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;
    [Header("Different Index")]
    private int selectedIndex;
    [Header("Bool Variable")]
    bool ADTime = true;
    private bool canShowInterstitial;
    [Header("Different Rank")]
    private int DressRank, VeilRank, NecklesRank, BouqetRank, BangalRank, ShoesRank, HairRank, LipsRank, CrownRank, EyeshadeRank, EaringRank, BagRank,
                TopRank, ShortRank, BlushRank;
    private int DressTotalRank, VeilTotalRank, NecklesTotalRank, BouqetTotalRank, BangalTotalRank, ShoesTotalRank, HairTotalRank,
                LipsTotalRank, CrownTotalRank, EyeshadeTotalRank, EaringTotalRank, BagTotalRank, TopTotalRank, ShortTotalRank, BlushTotalRank;
    private int weddingDressTotalRank, weddingNecklesTotalRank, weddingBouqetTotalRank, weddingBangalTotalRank, weddingShoesTotalRank,
                weddingHairTotalRank, weddingLipsTotalRank, weddingEyeshadeTotalRank, weddingEaringTotalRank, weddingBagTotalRank,
                weddingTopTotalRank, weddingShortTotalRank, weddingBlushTotalRank;
    private int partyDressTotalRank, partyNecklesTotalRank, partyBouqetTotalRank, partyBangalTotalRank, partyShoesTotalRank,
                partyHairTotalRank, partyLipsTotalRank, partyEyeshadeTotalRank, partyEaringTotalRank, partyBagTotalRank,
                partyTopTotalRank, partyShortTotalRank, partyBlushTotalRank;
    [Header("Int Variable")]
    private int weddingTotal = 0, partyTotal = 0;
    private int weddingTotalMarks = 0, partyTotalMarks = 0;
    public Animator CompareAnim;
    public Animator CardAnimation;
    public GameObject Compareing;
    public GameObject AdPenl;
    private enum RewardType
    {
        none, Coins, SelectionItem, MultipleOfTwo
    }
    private RewardType rewardType;

    #region start
    private void Start()
    {
        ShowInterstitial();
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            GSF_SaveLoad.LoadProgress();
        }
        if (GameManager.Instance.randomMode == 1)
        {
            PartyCharactorMover.transform.localScale = new Vector3(-1, 1, 1);
            weddingElements.weddingDressImage.sprite = weddingDressSprites[GameManager.Instance.weddingDress];
            weddingElements.weddingEaringImage.sprite = weddingEaringSprites[GameManager.Instance.weddingEaring];
            weddingElements.weddingNecklesImage.sprite = weddingNecklesSprites[GameManager.Instance.weddingNeckles];
            weddingElements.weddingBangalImage.sprite = weddingBangalSprites[GameManager.Instance.weddingBangal];
            weddingElements.weddingBouqetImage.sprite = weddingBouqetSprites[GameManager.Instance.weddingBouqet];
            weddingElements.weddingShoesImage.sprite = weddingShoesSprites[GameManager.Instance.weddingShoes];
            weddingElements.weddingHairImage.sprite = weddingHairSprites[GameManager.Instance.weddingHair];
            weddingElements.weddingLipsImage.sprite = weddingLipsSprites[GameManager.Instance.weddingLips];
            weddingElements.weddingCrownImage.sprite = weddingCrownSprites[GameManager.Instance.weddingCrown];
            weddingElements.weddingVeilImage.sprite = weddingVeilSprites[GameManager.Instance.weddingVeil];
            weddingElements.weddingEyeshadeImage.sprite = weddingEyeshadeSprites[GameManager.Instance.weddingEyeshade];
            weddingElements.weddingBagImage.sprite = weddingBagSprites[GameManager.Instance.weddingBag];
            weddingElements.weddingBlushImage.sprite = weddingBlushSprites[GameManager.Instance.weddingBlush];
            
            DressRank = GameManager.Instance.weddingDressIndex;
            VeilRank = GameManager.Instance.weddingVeilIndex;
            NecklesRank = GameManager.Instance.weddingNecklesIndex;
            BouqetRank = GameManager.Instance.weddingBouqetIndex;
            BangalRank = GameManager.Instance.weddingBangalIndex;
            ShoesRank = GameManager.Instance.weddingShoesIndex;
            HairRank = GameManager.Instance.weddingHairIndex;
            LipsRank = GameManager.Instance.weddingLipsIndex;
            CrownRank = GameManager.Instance.weddingCrownIndex;
            EyeshadeRank = GameManager.Instance.weddingEyeshadeIndex;
            EaringRank = GameManager.Instance.weddingEaringIndex;
            BagRank = GameManager.Instance.weddingBagIndex;
            BlushRank = GameManager.Instance.weddingBlushIndex;

            weddingElements.playerBotInJudge.sprite = playerBotImageSprites[GameManager.Instance.PlayerBotImage];
            partyElements.oppoBotInJudge.sprite = oppoBotImageSprites[GameManager.Instance.OpponentBotImage];
            weddingPlayerDressUp();
            PartyOppoDressUp();
            StartCoroutine(StartComparingWedding(3f));
        }
        else if (GameManager.Instance.randomMode == 2)
        {
            WeddingCharactorMover.transform.localScale = new Vector3(-1, 1, 1);
            PartyCharactorMover.Move(new Vector3(-273, -39, 0), 0.5f, true, false);
            WeddingCharactorMover.Move(new Vector3(407, 35, 0), 0.5f, true, false);
            partyElements.partyDressImage.sprite = partyDressSprites[GameManager.Instance.partyDress];
            partyElements.partyEaringImage.sprite = partyEaringSprites[GameManager.Instance.partyEaring];
            partyElements.partyNecklesImage.sprite = partyNecklesSprites[GameManager.Instance.partyNeckles];
            partyElements.partyBangalImage.sprite = partyBangalSprites[GameManager.Instance.partyBangal];
            partyElements.partyBouqetImage.sprite = partyBouqetSprites[GameManager.Instance.partyBouqet];
            partyElements.partyShoesImage.sprite = partyShoesSprites[GameManager.Instance.partyShoes];
            partyElements.partyHairImage.sprite = partyHairSprites[GameManager.Instance.partyHair];
            partyElements.partyLipsImage.sprite = partyLipsSprites[GameManager.Instance.partyLips];
            partyElements.partyTopImage.sprite = partyTopSprites[GameManager.Instance.partyTop];
            partyElements.partyShortImage.sprite = partyShortSprites[GameManager.Instance.partyShort];
            partyElements.partyEyeshadeImage.sprite = partyEyeshadeSprites[GameManager.Instance.partyEyeshade];
            partyElements.partyBagImage.sprite = partyBagSprites[GameManager.Instance.partyBag];
            partyElements.partyBlushImage.sprite = partyBlushSprites[GameManager.Instance.partyBlush];

            DressRank = GameManager.Instance.partyDressIndex; 
            TopRank = GameManager.Instance.partyTopIndex;
            ShortRank = GameManager.Instance.partyShortIndex;
            NecklesRank = GameManager.Instance.partyNecklesIndex;
            BouqetRank = GameManager.Instance.partyBouqetIndex;
            BangalRank = GameManager.Instance.partyBangalIndex;
            ShoesRank = GameManager.Instance.partyShoesIndex;
            HairRank = GameManager.Instance.partyHairIndex;
            LipsRank = GameManager.Instance.partyLipsIndex;
            EyeshadeRank = GameManager.Instance.partyEyeshadeIndex;
            EaringRank = GameManager.Instance.partyEaringIndex;
            BagRank = GameManager.Instance.partyBagIndex;
            BlushRank = GameManager.Instance.partyBlushIndex;

            print(GameManager.Instance.parShoes);

            weddingElements.playerBotInJudge.sprite = playerBotImageSprites[GameManager.Instance.PlayerBotImage];
            partyElements.oppoBotInJudge.sprite = oppoBotImageSprites[GameManager.Instance.OpponentBotImage];
            partyPlayerDressUp();
            WeddingOppoDressUp();
            StartCoroutine(StartComparingParty(3f));
        }
        TotalCoins.text = SaveData.Instance.Coins.ToString();
        SaveData.instance.LevelsUnlocked++;

        StartCoroutine(AdDelay(45));
        //DressRank = HairRank = LipsRank = 1;
        //TopRank = ShortRank = CrownRank = VeilRank = NecklesRank = BouqetRank = BangalRank = ShoesRank = EyeshadeRank = EaringRank = -1;
    }

    private void ShowInterstitial()
    {
        if (MyAdsManager.Instance)
        {
            MyAdsManager.Instance.ShowInterstitialAds();
        }
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

    #region GetRewardedCoins
    public void GetRewardedCoins()
    {
        rewardType = RewardType.Coins;
        CheckVideoStatus();
    }
    #endregion

    #region Btnfunctions
    public void Play(string str)
    {
        if (FinalPartical) FinalPartical.SetActive(false);
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
    #endregion

    #region IEnumerator
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
    IEnumerator ADTimeCounter()
    {
        yield return new WaitForSeconds(45);
        ADTime = true;
    }
    #endregion

    public void ShowAd()
    {
        if (ADTime)
        {
            StartCoroutine(AdDelay());
            ADTime = false;
            StartCoroutine(ADTimeCounter());
        }
    }

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

    IEnumerator ShowInterstitialAD()
    {
        if (AdPenl)
        {
            AdPenl.SetActive(true);
            yield return new WaitForSeconds(2f);
            AdPenl.SetActive(false);
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
                AdPenl.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(1);
                AdPenl.gameObject.SetActive(false);
                ShowInterstitial();
            }
        }
    }
    #endregion

    #region PlayerDressUp
    public void weddingPlayerDressUp()
    {
        if (GameManager.Instance.wedEaring == true)
        {
            weddingElements.weddingEaringImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedNeckles == true)
        {
            weddingElements.weddingNecklesImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedBangal == true)
        {
            weddingElements.weddingBangalImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedBouqet == true)
        {
            weddingElements.weddingBouqetImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedShoes == true)
        {
            weddingElements.weddingShoesImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedCrown == true)
        {
            weddingElements.weddingCrownImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedVeil == true)
        {
            weddingElements.weddingVeilImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedEyeshade == true)
        {
            weddingElements.weddingEyeshadeImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedBag == true)
        {
            weddingElements.weddingBagImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.wedBlush == true)
        {
            weddingElements.weddingBlushImage.gameObject.SetActive(true);
        }
    }
    public void partyPlayerDressUp()
    {
        if (GameManager.Instance.parEaring == true)
        {
            partyElements.partyEaringImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.parNeckles == true)
        {
            partyElements.partyNecklesImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.parBangal == true)
        {
            partyElements.partyBangalImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.parBouqet == true)
        {
            partyElements.partyBouqetImage.gameObject.SetActive(true);
            partyElements.partyBagImage.gameObject.SetActive(false);
        }
        if (GameManager.Instance.parShoes == true)
        {
            partyElements.partyShoesImage.gameObject.SetActive(true);
        }
        if (GameManager.Instance.parShort == true)
        {
            partyElements.partyTopImage.gameObject.SetActive(true);
            partyElements.partyShortImage.gameObject.SetActive(true);
            partyElements.partyDressImage.gameObject.SetActive(false);
        }
        if (GameManager.Instance.parTop == true)
        {
            partyElements.partyTopImage.gameObject.SetActive(true);
            partyElements.partyShortImage.gameObject.SetActive(true);
            partyElements.partyDressImage.gameObject.SetActive(false);
        }
        if (GameManager.Instance.parBag == true)
        {
            partyElements.partyBagImage.gameObject.SetActive(true);
            partyElements.partyBouqetImage.gameObject.SetActive(false);
        }
        if (GameManager.Instance.parBlush == true)
        {
            partyElements.partyBlushImage.gameObject.SetActive(true);
        }
    }
    #endregion

    #region PartyOppoDressup
    private void PartyOppoDressUp()
    {
        print("a");
        SaveData.Instance.LevelIndex++;
        int randomIndex = 0;
        if (SaveData.Instance.LevelIndex >= 1)
        {
            if (DressRank > -1)
            {
                randomIndex = Random.Range(0, partyDressSprites.Length);
                if (partyDressSprites[randomIndex] && partyElements.partyDressImage)
                {
                    partyElements.partyTopImage.gameObject.SetActive(false);
                    partyElements.partyDressImage.gameObject.SetActive(true);
                    partyElements.partyShortImage.gameObject.SetActive(false);
                    partyElements.partyDressImage.sprite = partyDressSprites[randomIndex];

                }
                DressTotalRank = 10;
                partyDressTotalRank = GetRank(randomIndex, partyDressSprites.Length);
            }

            if (HairRank > -1)
            {
                randomIndex = Random.Range(0, partyHairSprites.Length);
                if (partyHairSprites[randomIndex] && partyElements.partyHairImage)
                {
                    partyElements.partyHairImage.gameObject.SetActive(true);
                    partyElements.partyHairImage.sprite = partyHairSprites[randomIndex];
                }
                HairTotalRank = 10;
                partyHairTotalRank = GetRank(randomIndex, partyHairSprites.Length);
            }

            if (LipsRank > -1)
            {
                randomIndex = Random.Range(0, partyLipsSprites.Length);
                if (partyLipsSprites[randomIndex] && partyElements.partyLipsImage)
                {
                    partyElements.partyLipsImage.gameObject.SetActive(true);
                    partyElements.partyLipsImage.sprite = partyLipsSprites[randomIndex];
                }
                LipsTotalRank = 10;
                partyLipsTotalRank = GetRank(randomIndex, partyLipsSprites.Length);
            }
        }
        if (SaveData.Instance.LevelIndex >= 2)
        {
            if (NecklesRank > -1)
            {
                randomIndex = Random.Range(0, partyNecklesSprites.Length);
                if (partyNecklesSprites[randomIndex] && partyElements.partyNecklesImage)
                {
                    partyElements.partyNecklesImage.gameObject.SetActive(true);
                    partyElements.partyNecklesImage.sprite = partyNecklesSprites[randomIndex];
                }
                NecklesTotalRank = 10;
                partyNecklesTotalRank = GetRank(randomIndex, partyNecklesSprites.Length);
            }

            if (BangalRank > -1)
            {
                randomIndex = Random.Range(0, partyBangalSprites.Length);
                if (partyBangalSprites[randomIndex] && partyElements.partyBangalImage)
                {
                    partyElements.partyBangalImage.gameObject.SetActive(true);
                    partyElements.partyBangalImage.sprite = partyBangalSprites[randomIndex];
                }
                BangalTotalRank = 10;
                partyBangalTotalRank = GetRank(randomIndex, partyBangalSprites.Length);
            }

            if (EaringRank > -1)
            {
                randomIndex = Random.Range(0, partyEaringSprites.Length);
                if (partyEaringSprites[randomIndex] && partyElements.partyEaringImage)
                {
                    partyElements.partyEaringImage.gameObject.SetActive(true);
                    partyElements.partyEaringImage.sprite = partyEaringSprites[randomIndex];
                }
                EaringTotalRank = 10;
                partyEaringTotalRank = GetRank(randomIndex, partyEaringSprites.Length);
            }
        }
        if (SaveData.Instance.LevelIndex >= 3)
        {
            if (TopRank > -1)
            {
                randomIndex = Random.Range(0, partyTopSprites.Length);
                if (partyTopSprites[randomIndex] && partyElements.partyTopImage)
                {
                    partyElements.partyTopImage.gameObject.SetActive(true);
                    partyElements.partyShortImage.gameObject.SetActive(true);
                    partyElements.partyDressImage.gameObject.SetActive(false);
                    partyElements.partyTopImage.sprite = partyTopSprites[randomIndex];
                }
                TopTotalRank = 10;
                partyTopTotalRank = GetRank(randomIndex, partyTopSprites.Length);
            }

            if (ShortRank > -1)
            {
                randomIndex = Random.Range(0, partyShortSprites.Length);
                if (partyShortSprites[randomIndex] && partyElements.partyShortImage)
                {
                    partyElements.partyTopImage.gameObject.SetActive(true);
                    partyElements.partyShortImage.gameObject.SetActive(true);
                    partyElements.partyDressImage.gameObject.SetActive(false);
                    partyElements.partyShortImage.sprite = partyShortSprites[randomIndex];
                }
                ShortTotalRank = 10;
                partyShortTotalRank = GetRank(randomIndex, partyShortSprites.Length);
            }

            if (ShoesRank > -1)
            {
                randomIndex = Random.Range(0, partyShoesSprites.Length);
                if (partyShoesSprites[randomIndex] && partyElements.partyShoesImage)
                {
                    partyElements.partyShoesImage.gameObject.SetActive(true);
                    partyElements.partyShoesImage.sprite = partyShoesSprites[randomIndex];
                }
                ShoesTotalRank = 10;
                partyShoesTotalRank = GetRank(randomIndex, partyShoesSprites.Length);
            }
        }
        if (SaveData.Instance.LevelIndex >= 4)
        {
            if (BouqetRank > -1)
            {
                randomIndex = Random.Range(0, partyBouqetSprites.Length);
                if (partyBouqetSprites[randomIndex] && partyElements.partyBouqetImage)
                {
                    partyElements.partyBagImage.gameObject.SetActive(false);
                    partyElements.partyBouqetImage.gameObject.SetActive(true);
                    partyElements.partyBouqetImage.sprite = partyBouqetSprites[randomIndex];
                }
                BouqetTotalRank = 10;
                partyBouqetTotalRank = GetRank(randomIndex, partyBouqetSprites.Length);
            }

            if (EyeshadeRank > -1)
            {
                randomIndex = Random.Range(0, partyEyeshadeSprites.Length);
                if (partyEyeshadeSprites[randomIndex] && partyElements.partyEyeshadeImage)
                {
                    partyElements.partyEyeshadeImage.gameObject.SetActive(true);
                    partyElements.partyEyeshadeImage.sprite = partyEyeshadeSprites[randomIndex];
                }
                EyeshadeTotalRank = 10;
                partyEyeshadeTotalRank = GetRank(randomIndex, partyEyeshadeSprites.Length);
            }

            if (BagRank > -1)
            {
                randomIndex = Random.Range(0, partyBagSprites.Length);
                if (partyBagSprites[randomIndex] && partyElements.partyBagImage)
                {
                    partyElements.partyBagImage.gameObject.SetActive(true);
                    partyElements.partyBouqetImage.gameObject.SetActive(false);
                    partyElements.partyBagImage.sprite = partyBagSprites[randomIndex];
                }
                BagTotalRank = 10;
                partyBagTotalRank = GetRank(randomIndex, partyBagSprites.Length);
            }

            if (BlushRank > -1)
            {
                randomIndex = Random.Range(0, partyBlushSprites.Length);
                if (partyBlushSprites[randomIndex] && partyElements.partyBlushImage)
                {
                    partyElements.partyBlushImage.gameObject.SetActive(true);
                    partyElements.partyBlushImage.sprite = partyBlushSprites[randomIndex];
                }
                BlushTotalRank = 10;
                partyBlushTotalRank = GetRank(randomIndex, partyBlushSprites.Length);
            }
        }
        print("b");

    }
    #endregion

    #region WeddingOppoDressup
    private void WeddingOppoDressUp()
    {
        print("c");
        SaveData.Instance.LevelIndex++;
        int randomIndex = 0;
        if(SaveData.Instance.LevelIndex >= 1)
        {
            if (DressRank > -1)
            {
                randomIndex = Random.Range(0, weddingDressSprites.Length);
                if (weddingDressSprites[randomIndex] && weddingElements.weddingDressImage)
                {
                    weddingElements.weddingDressImage.gameObject.SetActive(true);
                    weddingElements.weddingDressImage.sprite = weddingDressSprites[randomIndex];

                }
                DressTotalRank = 10;
                weddingDressTotalRank = GetRank(randomIndex, weddingDressSprites.Length);
            }

            if (HairRank > -1)
            {
                randomIndex = Random.Range(0, weddingHairSprites.Length);
                if (weddingHairSprites[randomIndex] && weddingElements.weddingHairImage)
                {
                    weddingElements.weddingHairImage.gameObject.SetActive(true);
                    weddingElements.weddingHairImage.sprite = weddingHairSprites[randomIndex];
                }
                HairTotalRank = 10;
                weddingHairTotalRank = GetRank(randomIndex, weddingHairSprites.Length);
            }

            if (LipsRank > -1)
            {
                randomIndex = Random.Range(0, weddingLipsSprites.Length);
                if (weddingLipsSprites[randomIndex] && weddingElements.weddingLipsImage)
                {
                    weddingElements.weddingLipsImage.gameObject.SetActive(true);
                    weddingElements.weddingLipsImage.sprite = weddingLipsSprites[randomIndex];
                }
                LipsTotalRank = 10;
                weddingLipsTotalRank = GetRank(randomIndex, weddingLipsSprites.Length);
            }
        }
        if(SaveData.Instance.LevelIndex >= 2)
        {
            if (NecklesRank > -1)
            {
                randomIndex = Random.Range(0, weddingNecklesSprites.Length);
                if (weddingNecklesSprites[randomIndex] && weddingElements.weddingNecklesImage)
                {
                    weddingElements.weddingNecklesImage.gameObject.SetActive(true);
                    weddingElements.weddingNecklesImage.sprite = weddingNecklesSprites[randomIndex];
                }
                NecklesTotalRank = 10;
                weddingNecklesTotalRank = GetRank(randomIndex, weddingNecklesSprites.Length);
            }

            if (EaringRank > -1)
        {
            randomIndex = Random.Range(0, weddingEaringSprites.Length);
            if (weddingEaringSprites[randomIndex] && weddingElements.weddingEaringImage)
            {
                weddingElements.weddingEaringImage.gameObject.SetActive(true);
                weddingElements.weddingEaringImage.sprite = weddingEaringSprites[randomIndex];
            }
            EaringTotalRank = 10;
            weddingEaringTotalRank = GetRank(randomIndex, weddingEaringSprites.Length);
        }

            if (BangalRank > -1)
            {
                randomIndex = Random.Range(0, weddingBangalSprites.Length);
                if (weddingBangalSprites[randomIndex] && weddingElements.weddingBangalImage)
                {
                    weddingElements.weddingBangalImage.gameObject.SetActive(true);
                    weddingElements.weddingBangalImage.sprite = weddingBangalSprites[randomIndex];
                }
                BangalTotalRank = 10;
                weddingBangalTotalRank = GetRank(randomIndex, weddingBangalSprites.Length);
            }
        }
        if (SaveData.Instance.LevelIndex >= 3)
        {
            if (EyeshadeRank > -1)
        {
            randomIndex = Random.Range(0, weddingEyeshadeSprites.Length);
            if (weddingEyeshadeSprites[randomIndex] && weddingElements.weddingEyeshadeImage)
            {
                weddingElements.weddingEyeshadeImage.gameObject.SetActive(true);
                weddingElements.weddingEyeshadeImage.sprite = weddingEyeshadeSprites[randomIndex];
            }
            EyeshadeTotalRank = 10;
            weddingEyeshadeTotalRank = GetRank(randomIndex, weddingEyeshadeSprites.Length);
        }

            if (ShoesRank > -1)
            {
                randomIndex = Random.Range(0, weddingShoesSprites.Length);
                if (weddingShoesSprites[randomIndex] && weddingElements.weddingShoesImage)
                {
                    weddingElements.weddingShoesImage.gameObject.SetActive(true);
                    weddingElements.weddingShoesImage.sprite = weddingShoesSprites[randomIndex];
                }
                ShoesTotalRank = 10;
                weddingShoesTotalRank = GetRank(randomIndex, weddingShoesSprites.Length);
            }

            if (CrownRank > -1)
            {
                randomIndex = Random.Range(0, weddingCrownSprites.Length);
                if (weddingCrownSprites[randomIndex] && weddingElements.weddingCrownImage)
                {
                    weddingElements.weddingCrownImage.gameObject.SetActive(true);
                    weddingElements.weddingCrownImage.sprite = weddingCrownSprites[randomIndex];
                }
                TopTotalRank = 10;
                weddingTopTotalRank = GetRank(randomIndex, weddingCrownSprites.Length);
            }
        }
        if (SaveData.Instance.LevelIndex >= 4)
        {
            if (BouqetRank > -1)
            {
                randomIndex = Random.Range(0, weddingBouqetSprites.Length);
                if (weddingBouqetSprites[randomIndex] && weddingElements.weddingBouqetImage)
                {
                    weddingElements.weddingBouqetImage.gameObject.SetActive(true);
                    weddingElements.weddingBouqetImage.sprite = weddingBouqetSprites[randomIndex];
                }
                BouqetTotalRank = 10;
                weddingBouqetTotalRank = GetRank(randomIndex, weddingBouqetSprites.Length);
            }

            if (VeilRank > -1)
        {
            randomIndex = Random.Range(0, weddingVeilSprites.Length);
            if (weddingVeilSprites[randomIndex] && weddingElements.weddingVeilImage)
            {
                weddingElements.weddingVeilImage.gameObject.SetActive(true);
                weddingElements.weddingVeilImage.sprite = weddingVeilSprites[randomIndex];
            }
            ShortTotalRank = 10;
            weddingShortTotalRank = GetRank(randomIndex, weddingVeilSprites.Length);
        }

            if (BagRank > -1)
        {
            randomIndex = Random.Range(0, weddingBagSprites.Length);
            if (weddingBagSprites[randomIndex] && weddingElements.weddingBagImage)
            {
                weddingElements.weddingBagImage.gameObject.SetActive(true);
                weddingElements.weddingBagImage.sprite = weddingBagSprites[randomIndex];
            }
            BagTotalRank = 10;
            weddingBagTotalRank = GetRank(randomIndex, weddingBagSprites.Length);
        }

            if (BlushRank > -1)
            {
                randomIndex = Random.Range(0, weddingBlushSprites.Length);
                if (weddingBlushSprites[randomIndex] && weddingElements.weddingBlushImage)
                {
                    weddingElements.weddingBlushImage.gameObject.SetActive(true);
                    weddingElements.weddingBlushImage.sprite = weddingBlushSprites[randomIndex];
                }
                BlushTotalRank = 10;
                weddingBlushTotalRank = GetRank(randomIndex, weddingBlushSprites.Length);
            }
        }
        print("d");
    }
    #endregion

    #region Ranking Formula
    private int GetRank(int selectedCard, int totalItems)
    {
        //print("SelectedCard " + selectedCard);
        //print("TotalItems " + totalItems);
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
        //print(" Rank " + _Rank);
        if (_Rank > -1)
            return _Rank;
        else
            return 0;
    }
    #endregion

    #region Comparing 
    IEnumerator StartComparingWedding(float delay)
    {
        print("f");
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
        //print("TotalRank " + totalRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, DressTotalRank).ToString();
        oppoVoteText.text = GetRank(partyDressTotalRank, DressTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Neckles";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(NecklesRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, NecklesTotalRank).ToString();
        oppoVoteText.text = GetRank(partyNecklesTotalRank, NecklesTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bouqets";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BouqetRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BouqetTotalRank).ToString();
        oppoVoteText.text = GetRank(partyBouqetTotalRank, BouqetTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bangal";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BangalRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BangalTotalRank).ToString();
        oppoVoteText.text = GetRank(partyBangalTotalRank, BangalTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Shoes";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(ShoesRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, ShoesTotalRank).ToString();
        oppoVoteText.text = GetRank(partyShoesTotalRank, ShoesTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Hairs";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(HairRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, HairTotalRank).ToString();
        oppoVoteText.text = GetRank(partyHairTotalRank, HairTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Lips";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(LipsRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, LipsTotalRank).ToString();
        oppoVoteText.text = GetRank(partyLipsTotalRank, LipsTotalRank).ToString();
        UpdatingValueforWedding();


        yield return new WaitForSeconds(1);
        Comparetext.text = "Eyeshades";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EyeshadeRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, EyeshadeTotalRank).ToString();
        oppoVoteText.text = GetRank(partyEyeshadeTotalRank, EyeshadeTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Earings";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EaringRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, EaringTotalRank).ToString();
        oppoVoteText.text = GetRank(partyEaringTotalRank, EaringTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bag";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BagRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BagTotalRank).ToString();
        oppoVoteText.text = GetRank(partyBagTotalRank, BagTotalRank).ToString();
        UpdatingValueforWedding();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Blush";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BlushRank);
        weddingTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BlushTotalRank).ToString();
        oppoVoteText.text = GetRank(partyBlushTotalRank, BlushTotalRank).ToString();
        UpdatingValueforWedding();

        Compareing.SetActive(false);
        yield return new WaitForSeconds(2f);
        uIElements.ObjectOff.SetActive(false);
        if (weddingTotalMarks >= partyTotalMarks)
        {
            yield return new WaitForSeconds(1f);
            if (WinnerSFX) WinnerSFX.Play();
            WeddingCharactorMover.transform.SetSiblingIndex(1);
            yield return new WaitForSeconds(1f);
            PartyCharactorMover.Move(new Vector3(1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            WeddingCharactorMover.Move(new Vector3(-100, 0, 200), 0.5f, true, false);
            yield return new WaitForSeconds(1f);
            uIElements.Winner.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.CoinSlot.SetActive(true);
            //yield return new WaitForSeconds(1f);
            StartCoroutine(AddCoins(0.7f, 2000));
            yield return new WaitForSeconds(1f);
            FinalPartical.gameObject.SetActive(true);
            //if (FinalPartical) FinalPartical.Play();
            //yield return new WaitForSeconds(7f);
            //FinalPartical.gameObject.SetActive(false);
            yield return new WaitForSeconds(7f);
            uIElements.WinLevelCompletePanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (LoseSFX) LoseSFX.Play();
            PartyCharactorMover.transform.SetSiblingIndex(-1);
            yield return new WaitForSeconds(1f);
            WeddingCharactorMover.Move(new Vector3(-1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            PartyCharactorMover.Move(new Vector3(-35, -65, 200), 0.5f, true, false);
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
    IEnumerator StartComparingParty(float delay)
    {
        print("e");
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
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, DressTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingDressTotalRank, DressTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Neckles";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(NecklesRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, NecklesTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingNecklesTotalRank, NecklesTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bouqets";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BouqetRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BouqetTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingBouqetTotalRank, BouqetTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Bangal";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BangalRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BangalTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingBangalTotalRank, BangalTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Shoes";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(ShoesRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, ShoesTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingShoesTotalRank, ShoesTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Hairs";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(HairRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, HairTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingHairTotalRank, HairTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Lips";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(LipsRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, LipsTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingLipsTotalRank, LipsTotalRank).ToString();
        UpdatingValueforParty();


        yield return new WaitForSeconds(1);
        Comparetext.text = "Eyeshades";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EyeshadeRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, EyeshadeTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingEyeshadeTotalRank, EyeshadeTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Earings";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(EaringRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, EaringTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingEaringTotalRank, EaringTotalRank).ToString();
        UpdatingValueforParty();

        yield return new WaitForSeconds(1);
        Comparetext.text = "Blush";
        AnimatorPlayer();
        yield return new WaitForSeconds(0.5f);
        totalRank = GetRankValue(BlushRank);
        partyTotal += totalRank;
        playerVoteText.text = GetRank(totalRank, BlushTotalRank).ToString();
        oppoVoteText.text = GetRank(weddingBlushTotalRank, BlushTotalRank).ToString();
        UpdatingValueforParty();

        Compareing.SetActive(false);
        yield return new WaitForSeconds(2f);
        uIElements.ObjectOff.SetActive(false);
        if (partyTotalMarks >= weddingTotalMarks)
        {
            yield return new WaitForSeconds(1f);
            if (WinnerSFX) WinnerSFX.Play();
            PartyCharactorMover.transform.SetSiblingIndex(1);
            yield return new WaitForSeconds(1f);
            WeddingCharactorMover.Move(new Vector3(1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            PartyCharactorMover.Move(new Vector3(-25, -65, 200), 0.5f, true, false);
            yield return new WaitForSeconds(1f);
            uIElements.Winner.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.CoinSlot.SetActive(true);
            //yield return new WaitForSeconds(1f);
            StartCoroutine(AddCoins(0.7f, 2000));
            yield return new WaitForSeconds(1f);
            FinalPartical.gameObject.SetActive(true);
            //if (FinalPartical) FinalPartical.Play();
            //yield return new WaitForSeconds(5f);
            //FinalPartical.gameObject.SetActive(false);
            yield return new WaitForSeconds(7f);
            uIElements.WinLevelCompletePanel.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (LoseSFX) LoseSFX.Play();
            WeddingCharactorMover.transform.SetSiblingIndex(-1);
            yield return new WaitForSeconds(1f);
            PartyCharactorMover.Move(new Vector3(-1200, 0, 0), 0.5f, true, false);
            yield return new WaitForSeconds(0.3f);
            WeddingCharactorMover.Move(new Vector3(0, 0, 200), 0.5f, true, false);
            yield return new WaitForSeconds(1f);
            uIElements.Winner.SetActive(true);
            yield return new WaitForSeconds(2f);
            uIElements.CoinSlot.SetActive(true);
            yield return new WaitForSeconds(1f);
            StartCoroutine(AddCoins(0.7f, 500));
            yield return new WaitForSeconds(3f);
            uIElements.LoseLevelCompletePanel.SetActive(true);

        }
    }
    #endregion

    public void UpdatingValueforWedding()
    {
        weddingTotal = int.Parse(playerVoteText.text);
        weddingTotalMarks += weddingTotal;
        //print("When Wedding Character is Player  " + weddingTotalMarks);
        PlayerScoreText.text = weddingTotalMarks.ToString();
        //PlayerScoreText.text = weddingTotal + " / 10".ToString();
        partyTotal = int.Parse(oppoVoteText.text);
        partyTotalMarks += partyTotal;
        OpponentScoreText.text = partyTotalMarks.ToString();
        //OpponentScoreText.text = partyTotal + " / 10".ToString();
    }
    public void UpdatingValueforParty()
    {
        partyTotal = int.Parse(playerVoteText.text);
        partyTotalMarks += partyTotal;
        //print("When Party Character is Player  " + partyTotalMarks);
        PlayerScoreText.text = partyTotalMarks.ToString();
        //PlayerScoreText.text = weddingTotal + " / 10".ToString();
        weddingTotal = int.Parse(oppoVoteText.text);
        weddingTotalMarks += weddingTotal;
        OpponentScoreText.text = weddingTotalMarks.ToString();
        //OpponentScoreText.text = partyTotal + " / 10".ToString();
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

}
