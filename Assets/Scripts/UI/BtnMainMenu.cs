using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BtnMainMenu : MonoBehaviour 
{
	public string function;
	public Texture2D shareTexture;
	public List<GameObject> relatedObjects;
	public GameObject frameObj;

	void OnClick()
	{
		switch (function)
		{
		case "play":
			Application.LoadLevel("Level");
			break;

		case "rank":
			foreach(var obj in relatedObjects)
				obj.SetActive(false);
			frameObj.SetActive(true);
			break;

		case "credit":
#if UNITY_ANDROID
			AndroidSocialGate.SendMail("Hello Share Intent", "text to feedback", "Bouncing ball shooter - Feedback ", "Support@apohub.com", shareTexture);
#elif UNITY_IPHONE

#endif

//			foreach(var obj in relatedObjects)
//				obj.SetActive(false);
//			frameObj.SetActive(true);
			break;
		}
	}




}
