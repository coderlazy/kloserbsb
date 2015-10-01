using UnityEngine;
using System.Collections;

enum InputState
{
	Disable,
	Enable,
	Press
}

public class InputController : MonoBehaviour 
{
	public float MaxSpeed;

	private Vector2 m_StartPos, m_EndPos, m_Velocity;
	private float distant;
	private float m_StartTime, m_EndTime, m_DeltaTime;
	private float speed;

	private InputState launch = InputState.Disable;

	void Start()
	{
		StartCoroutine(Delay());
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(0.1f);

		launch = InputState.Enable;
	}

	void OnMouseDown()
	{
		if (launch == InputState.Enable && Time.timeScale != 0)
		{
			m_StartPos = Input.mousePosition;
			m_StartTime = Time.time;

			launch = InputState.Press;
		}
	}
	
	void OnMouseUp()
	{
		if (launch == InputState.Press && Time.timeScale != 0)
		{
			launch = InputState.Disable;

			m_EndPos = Input.mousePosition;
			m_EndTime = Time.time;
			
			m_Velocity = (m_EndPos - m_StartPos) * 3 / Screen.width;
			m_DeltaTime = m_EndTime - m_StartTime;

			if (m_DeltaTime < Time.fixedDeltaTime)
				m_DeltaTime = Time.fixedDeltaTime;

			distant = m_Velocity.sqrMagnitude;

			speed = distant / m_DeltaTime;

			if (speed < 0.1f)
			{
				launch = InputState.Enable;
				return;
			}
			else if (speed < 1)
				speed = 1;


			if (speed > MaxSpeed)
				speed = MaxSpeed;

			m_Velocity = m_Velocity.normalized;

			GameValue.Ball.Velocity = m_Velocity;
			GameValue.Ball.Speed = speed;

			GameController.Instance.Begin();
		}
	}
}
