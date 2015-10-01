using UnityEngine;
using System.Collections;

public class BtnPause : BtnInGame 
{
	public GameObject pauseObj;

	void OnClick()
	{
		if (GameController.Instance.getState == GameState.Begin ||
		    GameController.Instance.getState == GameState.Running)
		{
			GameController.Instance.Pause();
			pauseObj.SetActive(!pauseObj.activeSelf);
		}
	}
}
