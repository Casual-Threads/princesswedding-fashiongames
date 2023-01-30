using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public void Capture()
    {
        string screenshotname;
        int randomNumber = Random.Range(0, 10000);
        screenshotname = "ScreenShot" + randomNumber + ".png";
        ScreenCapture.CaptureScreenshot(screenshotname);
    }
}
