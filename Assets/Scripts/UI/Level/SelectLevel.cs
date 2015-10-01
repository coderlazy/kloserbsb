using UnityEngine;
using System.Collections;

public class SelectLevel : MonoSingleton<SelectLevel> 
{
	public UICenterOnChild m_ScrollObj;
	public GameObject m_Alert;
	public Color completedColor;
	public int maxLevel;
	public int maxChapter;
	public UIToggle[] m_ScrollDot;

	void Awake()
	{
		completedColor = new Color(252, 170, 8, 255) / 255;
		maxLevel = PlayerPrefsX.GetInt("max-level", 0);
		maxChapter = PlayerPrefsX.GetInt("max-chapter", 0);

		m_ScrollObj.onFinished += SetScrollDot;
	}

	void Start()
	{
		SpringPanel.Begin(m_ScrollObj.transform.parent.gameObject, 
		                  new Vector3(- ((maxLevel - 1) / Constants.CHAPTER_DIVIDE_FACTOR) * 720, 0, 0), 15);

		StartCoroutine(SimulateTouch());
	}

	IEnumerator SimulateTouch()
	{
		yield return new WaitForSeconds(0.1f);

		m_ScrollObj.Recenter();
	}

	public void ShowAlert()
	{
		m_Alert.SetActive(!m_Alert.activeSelf);
	}

	void SetScrollDot()
	{
		m_ScrollDot[(int)-m_ScrollObj.transform.parent.localPosition.x / 720].value = true;
	}
}
