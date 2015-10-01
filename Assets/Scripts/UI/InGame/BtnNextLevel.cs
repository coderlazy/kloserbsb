using UnityEngine;
using System.Collections;

public class BtnNextLevel : BtnInGame 
{
	void OnClick()
	{
		GameValue.LevelOrder++;

//		if ((GameValue.LevelOrder - 1) / Constants.CHAPTER_DIVIDE_FACTOR > PlayerPrefsX.GetInt("max-chapter", 0))
//			Application.LoadLevel("Level");
//		else
		Application.LoadLevel("Game");
	}
}
