using UnityEngine;
using System.Collections;

public class NewRecordStamp : MonoBehaviour 
{
	private readonly static float baseDelay = 0.25f;
	private UITweener[] Tweens;

	void Start()
	{
		if (GameValue.GameStar > 0)
		{
			if (GameValue.LevelOrder > PlayerPrefsX.GetInt("max-level", 0))
				PlayerPrefsX.SetInt("max-level", GameValue.LevelOrder);

			Vector2 record = PlayerPrefsX.GetVector2(
				"level-" + GameValue.LevelOrder, 
				new Vector2(0, 9999));

			// Mark record
			if (record.y > GameValue.GameTime)
			{
				PlayerPrefsX.SetVector2("level-" + GameValue.LevelOrder, 
				                        new Vector2(GameValue.GameStar, GameValue.GameTime));
				
				Tweens = GetComponents<UITweener>();
				
				for (int i = 0; i < Tweens.Length; i++)
				{
					Tweens[i].delay = baseDelay * GameValue.GameStar;
					Tweens[i].ResetToBeginning();
					Tweens[i].PlayForward();
				}
			}
			else
				gameObject.SetActive(false);

			// Max chapter base on star
			if (record.x < GameValue.GameStar)
			{
				int totalStar = 0;
				int chapter = (GameValue.LevelOrder - 1) / Constants.CHAPTER_DIVIDE_FACTOR;
				
				for (int i = chapter * Constants.CHAPTER_DIVIDE_FACTOR + 1; i <= (chapter + 1) * Constants.CHAPTER_DIVIDE_FACTOR; i++)
				{
					totalStar += (int)PlayerPrefsX.GetVector2("level-" + i, new Vector2(0, 9999)).x;
				}
				
				if (totalStar == 3 * Constants.CHAPTER_DIVIDE_FACTOR && chapter + 1 > PlayerPrefsX.GetInt("max-chapter", 0))
					PlayerPrefsX.SetInt("max-chapter", (GameValue.LevelOrder - 1) / Constants.CHAPTER_DIVIDE_FACTOR + 1);
			}
		}
		else
			gameObject.SetActive(false);
	}
}
