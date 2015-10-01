using UnityEngine;
using System.Collections;

public class ResultStar : MonoBehaviour 
{
	public int starValue;

	private UITweener[] Tweens;

	// Use this for initialization
	void Start () 
	{
		if (starValue > GameValue.GameStar)
			gameObject.SetActive(false);
		else
		{
			Tweens = GetComponents<UITweener>();

			for (int i = 0; i < Tweens.Length; i++)
			{
				Tweens[i].ResetToBeginning();
				Tweens[i].PlayForward();
			}
		}
	}
}
