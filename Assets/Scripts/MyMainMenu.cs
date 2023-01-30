using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MainMenuElements
{
    [Header("GameObject")]
    public GameObject LoadingPanel;
    //public GameObject ONButton, OFFButton;
    public Image fillbar;
    


}
public class MyMainMenu : MonoBehaviour
{
    public MainMenuElements uIElements;
    private AudioSource[] allAudioSources;
    void Start()
    {
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            GSF_SaveLoad.LoadProgress();
        }
        //StopAllAudio(SaveData.Instance.soundindex);
    }

    //#region Stop all sounds
    //public void StopAllAudio(int Index)
    //{
    //    allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
    //    if (Index == 0)
    //    {
    //        foreach (AudioSource audioS in allAudioSources)
    //        {
    //            audioS.Stop();
    //            uIElements.ONButton.SetActive(true);
    //            uIElements.OFFButton.SetActive(false);
    //        }
    //    }
    //    if (Index == 1)
    //    {
    //        foreach (AudioSource audioS in allAudioSources)
    //        {
    //            audioS.Play();
    //            uIElements.OFFButton.SetActive(true);
    //            uIElements.ONButton.SetActive(false);
    //        }
    //    }
    //    SaveData.Instance.soundindex = Index;
    //    GSF_SaveLoad.SaveProgress();

    //}
    //#endregion

    public void Play(string str)
    {
        uIElements.LoadingPanel.SetActive(true);
        StartCoroutine(LoadingScene(str));
    }
    IEnumerator LoadingScene(string str)
    {
        uIElements.fillbar.fillAmount = 0;
        while (uIElements.fillbar.fillAmount < 1)
        {
            uIElements.fillbar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        SceneManager.LoadScene(str);
        
    }
    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Stylist+Girls+Game+Studio");
    }
    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.sggs.diyjoystick.phonecover.tiedye.games");
    }
    public void PrivacyPolicy()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.sggs.diyjoystick.phonecover.tiedye.games");
    }
}
