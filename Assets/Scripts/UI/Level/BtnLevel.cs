using UnityEngine;
using System;
using System.Collections;

public class BtnLevel : MonoBehaviour 
{
	private int order;
	private Transform cacheTrans;
	private UILabel label;
	private bool isLock = false;

	void Start()
	{
		order = int.Parse(name);

		cacheTrans = transform;

		label = cacheTrans.FindChild("lb").GetComponent<UILabel>();
		label.text = string.Empty + order;

		Vector2 record = PlayerPrefsX.GetVector2(
			"level-" + order, 
			new Vector2(0, 9999));

		if (record.x > 0)
		{
			GetComponent<UISprite>().spriteName += "-completed";
			label.color = SelectLevel.Instance.completedColor;

			Transform starTrans = cacheTrans.FindChild("star");
			starTrans.gameObject.SetActive(true);

			for (int i = 0; i < starTrans.childCount; i++)
			{
				if (int.Parse(starTrans.GetChild(i).name) > record.x)
					starTrans.GetChild(i).gameObject.SetActive(false);
			}

			TimeSpan tSpan = TimeSpan.FromSeconds(record.y + 0.009f);

			Transform timeTrans = cacheTrans.FindChild("time");
			timeTrans.gameObject.SetActive(true);
			timeTrans.GetComponent<UILabel>().text = string.Format("{0}:{1:D2}", tSpan.Seconds, tSpan.Milliseconds / 10);
		}
		else if (order > SelectLevel.Instance.maxLevel + 1)
		{
			GetComponent<Collider>().enabled = false;
			GetComponent<UIWidget>().alpha = 0.5f;

//			if ((order - 1) / Constants.CHAPTER_DIVIDE_FACTOR > SelectLevel.Instance.maxChapter)
//			{
//				label.text = string.Empty;
//				cacheTrans.FindChild("lock").gameObject.SetActive(true);
//				isLock = true;
//			}
		}
//		else if (order == SelectLevel.Instance.maxLevel + 1 &&
//		         (order - 1) / Constants.CHAPTER_DIVIDE_FACTOR > SelectLevel.Instance.maxChapter)
//		{
//			label.text = string.Empty;
//			cacheTrans.FindChild("lock").gameObject.SetActive(true);
//			isLock = true;
//		}
	}

	void OnClick()
	{
		if (isLock)
		{
			SelectLevel.Instance.ShowAlert();
		}
		else
		{
			GameValue.LevelOrder = order;
			Application.LoadLevel("Game");
		}
	}
}
