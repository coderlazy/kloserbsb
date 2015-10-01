using UnityEngine;
using System.Collections;

public class FakeEntity : Entity 
{
	void Start()
	{
		Destroy(this);
	}
}
