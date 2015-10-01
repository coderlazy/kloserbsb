using UnityEngine;
using System.Collections;

public class ObjectDontDestroy : MonoBehaviour 
{
//	private float pauseTime;
	bool isQuit;
	void Awake()
	{

		Application.targetFrameRate = 60;
		isQuit = false;
		//Screen.sleepTimeout = SleepTimeout.NeverSleep;

		if (FindObjectsOfType<ObjectDontDestroy>().Length > 1)
			Destroy(gameObject);
		else
			DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(Application.loadedLevelName.Equals("Main")){
				DialogPopUp("Bouncing Ball", "Do you want to exit?");
			}else if(Application.loadedLevelName.Equals("Game")){
				DialogPopUp("Bouncing Ball", "Do you want back to select level?");
			}else{
				DialogPopUp("Bouncing Ball", "Do you want back to menu?");
			}

		}
#endif
	}
#if UNITY_ANDROID
	private void DialogPopUp(string tite, string msg) {
		AndroidDialog dialog = AndroidDialog.Create(tite, msg);
		dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
	}
	private void OnDialogClose(CEvent e) {
		
		//removing listner
		(e.dispatcher as AndroidDialog).removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		
		//parsing result
		switch((AndroidDialogResult)e.data) {
		case AndroidDialogResult.YES:
			if(!Application.loadedLevelName.Equals("Main")){
				Application.LoadLevel(Application.loadedLevel - 1);
			}else{
				Application.Quit();
			}
			break;
		case AndroidDialogResult.NO:

			break;
			
		}
	}
#endif
}
