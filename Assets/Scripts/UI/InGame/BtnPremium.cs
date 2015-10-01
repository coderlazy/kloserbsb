using UnityEngine;
using System.Collections;

public class BtnPremium : BtnInGame 
{
	void Start()
	{
#if NO_ADS
		gameObject.SetActive(false);
#endif
	}

	void OnClick()
	{
#if UNITY_ANDROID
		Ads.Instance.SuccsesPurchase();
		//Application.OpenURL("https://play.google.com/store/apps/details?id=com.apohub.bouncingballshoot.paid");
#elif UNITY_IOS
		//Application.OpenURL("https://itunes.apple.com/us/app/ball-eat-ball-full/id954708039?ls=1&mt=8");
#endif
	}

}
