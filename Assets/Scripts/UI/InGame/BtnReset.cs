using UnityEngine;
using System.Collections;

public class BtnReset : BtnInGame 
{
	void OnClick()
	{
		Application.LoadLevel("Game");
	}
}
