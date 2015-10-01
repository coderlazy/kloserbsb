using UnityEngine;
using UnityEditor;

public class ChangeVersion : MonoBehaviour {

	[MenuItem("Version/Change To FREE")]
	static void ChangeToFree () {
		PlayerSettings.productName = "Ball Eat Ball - FREE";
		PlayerSettings.bundleIdentifier = "com.morbling.game.beb";
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "");

		Analytics analyticObj = Object.FindObjectOfType<Analytics>();
		if (analyticObj != null)
		{
			analyticObj.trackingID = "UA-58477001-1";
			analyticObj.appName = "Ball Eat Ball - FREE";
		};
	}

	[MenuItem("Version/Change To PAID")]
	static void ChangeToPaid () {
		PlayerSettings.productName = "Ball Eat Ball";
		PlayerSettings.bundleIdentifier = "com.morbling.game.beb.paid";
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "NO_ADS");
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "NO_ADS");

		Analytics analyticObj = Object.FindObjectOfType<Analytics>();
		if (analyticObj != null)
		{
			analyticObj.trackingID = "UA-58477001-2";
			analyticObj.appName = "Ball Eat Ball";
		}
	}
}
