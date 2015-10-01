using UnityEngine;
using System.Collections;

public class Obstacle : Entity 
{
	void OnCollisionEnter2D()
	{
		Hit();
	}

	protected virtual void Hit() {}
}
