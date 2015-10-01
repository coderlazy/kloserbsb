using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnMenuCredit : MonoBehaviour {

	public List<GameObject> relatedObjects;

	public void ExitCredit()
	{
		foreach(var obj in relatedObjects)
			obj.SetActive(true);
		gameObject.SetActive(false);
	}

	public void RateClicked()
	{
#if UNITY_ANDROID
//#if NO_ADS
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.apohub.bouncingballshoot");
//#else
		// có tính phí
		//Application.OpenURL("https://play.google.com/store/apps/details?id=com.apohub.bouncingballshoot.paid");
//#endif
#elif UNITY_IOS
		float osVersion = -1f;
		string versionString = SystemInfo.operatingSystem.Replace("iPhone OS ", "");
		float.TryParse(versionString.Substring(0, 1), out osVersion);

		string appID;
#if NO_ADS
		appID = "954708039";
#else
		appID = "954185763";
#endif

		if (osVersion >= 7)
			Application.OpenURL("itms-apps://itunes.apple.com/app/id=" + appID);
		else
			Application.OpenURL("itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=" + appID);
#endif
	}

	public void FacebookClicked()
	{

		Application.OpenURL("https://www.facebook.com/APOHUB?ref=hl");
	}

	public void TwitterClicked()
	{
		Application.OpenURL("https://twitter.com/ApohubGame");
	}

	public void WebClicked()
	{
		Application.OpenURL("http://apohub.com");
	}
}
