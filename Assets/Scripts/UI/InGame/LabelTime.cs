using UnityEngine;
using System;
using System.Collections;

public class LabelTime : MonoBehaviour 
{
	public UILabel labelSec, labelMillisec;
	private TimeSpan tSpan;

	void Start()
	{
		StartCoroutine(Counting());
	}

	IEnumerator Counting()
	{
		float timeCount = 0;
		float delta = GameValue.GameTime / 40;

		while(timeCount < GameValue.GameTime)
		{
			timeCount += delta;
			tSpan = TimeSpan.FromSeconds(timeCount);
			labelSec.text = string.Format("{0:d2}", tSpan.Seconds);
			labelMillisec.text = string.Format("{0:d2}", tSpan.Milliseconds / 10);

			yield return new WaitForSeconds(0.025f);
		}

		tSpan = TimeSpan.FromSeconds(GameValue.GameTime + 0.009f);
		labelSec.text = string.Format("{0:d2}", tSpan.Seconds);
		labelMillisec.text = string.Format("{0:d2}", tSpan.Milliseconds / 10);
	}

	/*void Update()
	{
		if (GameController.Instance.getState == GameState.Running)
		{
			tSpan = TimeSpan.FromSeconds(GameValue.GameTime);
			label.text = string.Format("{0:d2}:{1:d2}", tSpan.Seconds, tSpan.Milliseconds / 10);
		}
	}*/
}
