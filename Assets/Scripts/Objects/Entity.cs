using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
	public Vector2 defaultSize;

	protected Transform m_Trans;

	void Awake()
	{
		m_Trans = transform;
	}

	public void Scale(Vector2 targetSize)
	{
		m_Trans.localScale = new Vector3(targetSize.x / defaultSize.x, targetSize.y / defaultSize.y, 1);
	}

	public void Rotate(float angle)
	{
		if (angle != 0)
			m_Trans.Rotate(new Vector3(0, 0, 1), angle);
	}
}
