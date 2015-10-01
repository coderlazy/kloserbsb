using UnityEngine;
using System.Collections;

public class BtnBackToLevel : BtnInGame 
{
	void OnClick()
	{
		Application.LoadLevel("Level");
	}
}
