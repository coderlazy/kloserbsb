using UnityEngine;
using System.Collections;

public class LabelResult : MonoBehaviour 
{
	public AudioClip[] m_AudioResult;
	private UITweener[] Tweens;

	void Start()
	{
		string rank;

		if (GameValue.GameStar > 0)
			rank = "[ff9000]";
		else
			rank = "[00f0ff]";

		switch (GameValue.GameStar)
		{
			case 3:
				rank += "AWESOME!!";
				break;
				
			case 2:
				rank += "AMAZING!";
				break;
				
			case 1:
				rank += "GOOD";
				break;
				
			default:
				rank += "GAME OVER";
				break;
		}

		GetComponent<UILabel>().text = rank + "[-]";

		Tweens = GetComponents<UITweener>();
		
		for (int i = 0; i < Tweens.Length; i++)
		{
			Tweens[i].ResetToBeginning();
			Tweens[i].PlayForward();
		}

		GetComponent<AudioSource>().PlayOneShot(m_AudioResult[GameValue.GameStar]);
	}
}
