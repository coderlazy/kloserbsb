using UnityEngine;
using System.Collections;

public enum RespondType
{
	Win,
	Lose,
	Both
}

public class BtnInGame : MonoBehaviour 
{
	public RespondType RespondTo;

	void Start()
	{
		if ((RespondTo == RespondType.Win && GameValue.GameStar == 0) ||
		    RespondTo == RespondType.Lose && GameValue.GameStar > 0)
			gameObject.SetActive(false);
	}
}
