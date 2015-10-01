using UnityEngine;
using System.Collections;

public class LabelLevel : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		GetComponent<UILabel>().text = "LEVEL " + GameValue.LevelOrder;
	}
}
