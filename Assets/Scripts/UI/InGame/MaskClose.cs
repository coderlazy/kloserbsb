using UnityEngine;
using System.Collections;

public class MaskClose : MonoBehaviour 
{
	public void Close()
	{
		gameObject.SetActive(false);
	}
}
