using UnityEngine;
using System.Collections;

public class BtnCheat : MonoBehaviour 
{
	void OnClick()
	{
		PlayerPrefsX.SetInt("max-level", 60);
		PlayerPrefsX.SetInt("max-chapter", 6);
	}
}
