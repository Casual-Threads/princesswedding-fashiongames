using UnityEngine;
using System.Collections;

public class GameManager
{

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameManager ();
			}
			return instance;
		}
	}

	public bool Initialized = false;
	public bool bannerCalled = false;
	public string GameStatus;
	public int objectivesAchieved;
	public int SessionStatus = 0;
	public bool SessionAd = false;
    public int PlayerBotImage;
    public int OpponentBotImage;
    public int selectedPlayer;
    public int selectedLevel;
	public int randomMode, ModeSelected; 
	public int randomIndex;
	public int totalKills;
    public int selectedTexture;
    public bool stopPlaying;
    public bool showStats;
	public int weddingDress, weddingEaring, weddingNeckles, weddingBangal, weddingBouqet, weddingShoes, weddingHair, weddingLips, weddingCrown,
			   weddingVeil, weddingEyeshade, weddingBag, weddingBlush;
	public int weddingDressIndex, weddingEaringIndex, weddingNecklesIndex, weddingBangalIndex, weddingBouqetIndex, weddingShoesIndex,
			   weddingHairIndex, weddingLipsIndex, weddingCrownIndex, weddingVeilIndex, weddingEyeshadeIndex, weddingBagIndex, weddingBlushIndex;
	public int partyDress, partyShort, partyNeckles, partyBouqet, partyBangal, partyShoes, partyHair, partyLips, partyTop, partyEyeshade, partyEaring, partyBag, partyBlush;
	public int partyDressIndex, partyShortIndex, partyNecklesIndex, partyBouqetIndex, partyBangalIndex, partyShoesIndex, partyHairIndex,
			   partyLipsIndex, partyTopIndex, partyEyeshadeIndex, partyEaringIndex, partyBagIndex, partyBlushIndex;
	public bool wedEaring = false;
	public bool wedNeckles = false;
	public bool wedBangal = false;
	public bool wedBouqet = false;
	public bool wedShoes = false;
	public bool wedCrown = false;
	public bool wedVeil = false;
	public bool wedEyeshade = false;
	public bool wedBag = false;
	public bool wedBlush = false;

	public bool parDress = false;
	public bool parEaring = false;
	public bool parNeckles = false;
	public bool parBangal = false;
	public bool parBouqet = false;
	public bool parShoes = false;
	public bool parShort = false;
	public bool parTop = false;
	public bool parEyeshade = false;
	public bool parBag = false;
	public bool parBlush = false;

	public void resetValue()
	{
		wedEaring = false;
		wedNeckles = false;
		wedBangal = false;
		wedBouqet = false;
		wedShoes = false;
		wedCrown = false;
		wedVeil = false;
		wedEyeshade = false;
		wedBag = false;
		wedBlush = false;

		parDress = false;
		parEaring = false;
		parNeckles = false;
		parBangal = false;
		parBouqet = false;
		parShoes = false;
		parShort = false;
		parTop = false;
		parEyeshade = false;
		parBag = false;
		parBlush = false;

	}
}