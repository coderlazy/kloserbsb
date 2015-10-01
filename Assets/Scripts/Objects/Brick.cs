using UnityEngine;
using System.Collections;

public class Brick : Obstacle
{
	public int durability;
	public SpriteRenderer sprite;
	public Color[] colors;
	public GameObject[] effectCores;

	private ParticleSystem[] Effects;

	void Start()
	{	
		Effects = new ParticleSystem[effectCores.Length];

		for (int i = 0; i < effectCores.Length; i++)
		{
			Effects[i] = (GameObject.Instantiate(effectCores[i], m_Trans.localPosition, Quaternion.identity) 
			          as GameObject).GetComponent<ParticleSystem>();
		}
	}

	protected override void Hit()
	{
		durability--;

		if (durability <= 0)
		{
			Effects[0].transform.localPosition = m_Trans.position;
			Effects[0].Play();
			Destroy(gameObject, Time.fixedDeltaTime);
		}
		else
		{
			Effects[durability].transform.localPosition = m_Trans.position;
			Effects[durability].Play();
			sprite.color = colors[colors.Length - durability];
		}
	}
}
