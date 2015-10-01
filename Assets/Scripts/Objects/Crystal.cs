using UnityEngine;
using System.Collections;

public class Crystal : Entity 
{
	void OnTriggerEnter2D()
	{
		GameValue.CrystalRemain--;
		EffectController.Instance.PlayCrystalEffect(m_Trans.position);
		Destroy(gameObject);
	}
}
