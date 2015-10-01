using UnityEngine;
using System.Collections;

public class BtnSocial : MonoBehaviour 
{
	public string function;

	void OnClick()
	{
		switch (function)
		{
		case "facebook":
			FBSharing();
			break;
		case "twitter":
			//TWSharing();
			break;
		}
	}

	#region FB
	void FBSharing()
	{
		if (!FB.IsInitialized)
			FB.Init(FBInitCallback);
		else FBInitCallback();
	}

	void FBInitCallback()
	{
		if (!FB.IsLoggedIn) {
			FB.Login("email,publish_actions", FBLoginCallback);                                                                                                                                                                
		}
		else 
			StartCoroutine(FBTakeScreenshot());
	}
	
	void FBLoginCallback(FBResult result) {                                                                                          
		FbDebug.Log("LoginCallback");                                                          
		if (FB.IsLoggedIn) {                                                                                      
			Debug.LogWarning("Logged in. ID: " + FB.UserId);
			StartCoroutine(FBTakeScreenshot());
		}                                                                                      
	}
	
	private IEnumerator FBTakeScreenshot()
	{
		yield return new WaitForEndOfFrame();
		
//		var width = Screen.width;
//		var height = Screen.height;
//		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
//		// Read screen contents into the texture
//		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//		tex.Apply();
//		byte[] screenshot = tex.EncodeToPNG();
//		
//		var wwwForm = new WWWForm();
//		wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
//
//		if (GameValue.GameStar > 0)
//			wwwForm.AddField("message", string.Format(Constants.STRING_SHARING, 
//			                                          GameValue.GameStar, 
//			                                          GameValue.GameTime + 0.009f, 
//			                                          GameValue.LevelOrder, Constants.DOWNLOAD_URL));
//		else
//			wwwForm.AddField("message", string.Format(Constants.STRING_LOSE_SHARING,
//			                                          GameValue.LevelOrder, Constants.DOWNLOAD_URL));
//		
//		FB.API("me/photos", Facebook.HttpMethod.POST, FBTakeScreenshotCallback, wwwForm);
		
		//		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/Screenshot.png", screenshot);

		string masage = "";

		if (GameValue.GameStar > 0)
			masage = string.Format(Constants.STRING_SHARING, 
			                       GameValue.GameStar, 
			                       GameValue.GameTime + 0.009f, 
			                       GameValue.LevelOrder, Constants.DOWNLOAD_URL);
		else
			masage = string.Format(Constants.STRING_LOSE_SHARING,
			                       GameValue.LevelOrder, Constants.DOWNLOAD_URL);

		FB.Feed(
			link: Constants.DOWNLOAD_URL,
			linkName: masage,
			linkCaption: "Bouncing Ball Shooter",
//					linkDescription: "There are a lot of larch trees around here, aren't there?",
			picture: "https://farm8.staticflickr.com/7577/16185277116_81d3b8939e_q.jpg"
//			callback: LogCallback
			);
	}
	
	void FBTakeScreenshotCallback(FBResult result) {
		DialogManager.Instance.ShowSubmitDialog("Posted", FBDialogCallback);
	}
	
	void FBDialogCallback(bool value) {}
	#endregion

	void TWSharing()
	{
		string shareStr = "";

		if (GameValue.GameStar > 0)
			shareStr = string.Format(Constants.STRING_SHARING, 
	                                GameValue.GameStar, 
	                                GameValue.GameTime + 0.009f, 
	                                GameValue.LevelOrder, Constants.DOWNLOAD_URL);
		else
			shareStr = string.Format(Constants.STRING_LOSE_SHARING,
	                                GameValue.LevelOrder, Constants.DOWNLOAD_URL);

		Application.OpenURL("http://twitter.com/intent/tweet" +
		                    "?text=" + WWW.EscapeURL(shareStr));// +
//		                    "&amp;url=" + WWW.EscapeURL(url) +
//		                    "&amp;related=" + WWW.EscapeURL(related);
	}

}
