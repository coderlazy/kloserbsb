using UnityEngine;
using System.Collections;

public class GroundScroll : MonoBehaviour 
{
	public float Speed = 0.25f;
	private Vector2 offset = Vector2.zero;
	private Renderer m_Rend;

	void Awake()
	{
		m_Rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		offset.x += Speed * Time.deltaTime;

		if (offset.x > 0)
		{
			offset.x -= 1;
		}

		m_Rend.sharedMaterial.mainTextureOffset = offset;
	}
}
