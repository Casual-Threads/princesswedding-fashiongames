using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProps
{
    public string playerName;
    public int playerHealth;
    public int playerDamage;
    public int playerRange;
    public bool isLocked = true;
}

[System.Serializable]
public class Modesprops
{
    public bool isLocked;
}

[System.Serializable]
public class WeddingDressUpModeElements
{
    public List<bool> DressLocked = new List<bool>();
    public List<bool> EaringLocked = new List<bool>();
    public List<bool> BangalLocked = new List<bool>();
    public List<bool> VeilLocked = new List<bool>();
    public List<bool> CrownLocked = new List<bool>();
    public List<bool> NecklesLocked = new List<bool>();
    public List<bool> BouqetLocked = new List<bool>();
    public List<bool> BagLocked = new List<bool>();
    public List<bool> ShoesLocked = new List<bool>();
    public List<bool> HairLocked = new List<bool>();
    public List<bool> LipsLocked = new List<bool>();
    public List<bool> EyeshadeLocked = new List<bool>();
    public List<bool> BlushLocked = new List<bool>();
}

[System.Serializable]
public class PartyDressUpModeElements
{
    public List<bool> DressLocked = new List<bool>();
    public List<bool> EaringLocked = new List<bool>();
    public List<bool> BangalLocked = new List<bool>();
    public List<bool> TopLocked = new List<bool>();
    public List<bool> ShortLocked = new List<bool>();
    public List<bool> NecklesLocked = new List<bool>();
    public List<bool> BouqetLocked = new List<bool>();
    public List<bool> BagLocked = new List<bool>();
    public List<bool> ShoesLocked = new List<bool>();
    public List<bool> HairLocked = new List<bool>();
    public List<bool> LipsLocked = new List<bool>();
    public List<bool> EyeshadeLocked = new List<bool>();
    public List<bool> BlushLocked = new List<bool>();
}

[System.Serializable]
public class SaveData
{

    public static SaveData instance;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }
    public bool RemoveAds = false;
    public int soundindex = 1;
    public string PlayerName;
    public int PlayerImageIndex;
    public int LevelIndex = 0;
    public int LevelsUnlocked = 1;
    public int EventsUnlocked = 0;
    public int SelectedAvatar = 0;
    public string ProfileName;
    public bool ProfileCreated = false;
    public bool isModetrue;
    public bool isMode;
    public bool isSound = true, isMusic = true, isVibration = true, isRightControls = true;
    public int Coins = 2000;
    public List<PlayerProps> Players = new List<PlayerProps>();
    public List<Modesprops> ModeProps = new List<Modesprops>();
    public WeddingDressUpModeElements weddingDressUpModeElements = new WeddingDressUpModeElements();
    public PartyDressUpModeElements partyDressUpModeElements = new PartyDressUpModeElements();
    public string hashOfSaveData;

    //Constructor to save actual GameData
    public SaveData() { }

    //Constructor to check any tampering with the SaveData
    public SaveData(bool ads, int levelsUnlocked, int eventsUnlocked, int coins, bool soundOn, bool musicOn, bool vibrationOn, bool rightControls, List<PlayerProps> _players,
                    List<Modesprops> _modeProps, WeddingDressUpModeElements _weddingDressUpModeElements, PartyDressUpModeElements _partyDressUpModeElements)
    {
        RemoveAds = ads;
        LevelsUnlocked = levelsUnlocked;
        EventsUnlocked = eventsUnlocked;
        Coins = coins;
        isSound = soundOn;
        isMusic = musicOn;
        isVibration = vibrationOn;
        isRightControls = rightControls;
        Players = _players;
        ModeProps = _modeProps;
        weddingDressUpModeElements = _weddingDressUpModeElements;
        partyDressUpModeElements = _partyDressUpModeElements;
    }
}