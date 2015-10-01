using UnityEngine;
using System.Collections;

public class Hole : Entity 
{
	void OnTriggerEnter2D()
	{
		GameController.Instance.IntoTheHole();
	}
}
