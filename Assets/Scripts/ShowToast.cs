using UnityEngine;
using System.Collections;

public static class ShowToast
{


	static string toastString;
	static AndroidJavaObject currentActivity;

	public static void showToastMessage (string toastMsg)
	{
				if (Application.platform != RuntimePlatform.Android) {
					return;
				}
		AndroidJavaClass UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");

		currentActivity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
		toastString = toastMsg;

		currentActivity.Call ("runOnUiThread", new AndroidJavaRunnable (showToast));
	}

	/*public*/ static void showToast ()
	{
		Debug.Log ("Running on UI thread");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject> ("getApplicationContext");
		AndroidJavaClass Toast = new AndroidJavaClass ("android.widget.Toast");
		AndroidJavaObject javaString = new AndroidJavaObject ("java.lang.String", toastString);
		AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject> ("makeText", context, javaString, Toast.GetStatic<int> ("LENGTH_SHORT"));
		toast.Call ("show");
	}

}
