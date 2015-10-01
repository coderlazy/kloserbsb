using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour 
{
	public float m_Radius;
	public Vector2 m_Border;
	public Vector2 Velocity;
	public float Speed;
	public TrailRenderer Trail;

	private Rigidbody2D m_Rigid;
	private Vector2 prePosition;
	private float delta;
	private IEnumerator reEnable;

	// Use this for initialization
	void Start () 
	{
		m_Rigid = GetComponent<Rigidbody2D>();

		Velocity = new Vector2();

		prePosition = m_Rigid.position;
	}

	void FixedUpdate()
	{
		// Re-calculate velocity base on physics interactions
		Velocity = (Velocity + (m_Rigid.position - prePosition)).normalized;

		// Pre-calculate ball postion
		prePosition = m_Rigid.position + Velocity * Speed * Time.fixedDeltaTime;

		// If the ball out of border, move it to contact the border
		// Left or right
		if (prePosition.x + m_Radius > m_Border.x)
		{
			delta = prePosition.x + m_Radius - m_Border.x;
			
			prePosition.x -= delta;
			prePosition.y -= Velocity.y * (delta / Velocity.x);
		}
		else if (prePosition.x - m_Radius < - m_Border.x)
		{
			delta = prePosition.x - m_Radius + m_Border.x;
			
			prePosition.x -= delta;
			prePosition.y -= Velocity.y * (delta / Velocity.x);
		}
		// Up or down
		if (prePosition.y + m_Radius > m_Border.y)
		{
			delta = prePosition.y + m_Radius - m_Border.y;
			
			prePosition.y -= delta;
			prePosition.x -= Velocity.x * (delta / Velocity.y);
		}
		else if (prePosition.y - m_Radius < - m_Border.y)
		{
			delta = prePosition.y - m_Radius + m_Border.y;
			
			prePosition.y -= delta;
			prePosition.x -= Velocity.x * (delta / Velocity.y);
		}
		
		BounceWithWall();

		// Move the ball
		m_Rigid.MovePosition(prePosition);
	}

	// Calculate velocity to bounce when contact with border
	void BounceWithWall()
	{
		Vector2 bounced = Vector2.zero;

		// Bounce with left and right
		if (Mathf.Approximately(prePosition.x + m_Radius, m_Border.x) ||
		    Mathf.Approximately(prePosition.x - m_Radius, - m_Border.x))
		{
			Velocity.x *= -1;
			bounced.x = (m_Radius + Constants.BOUNCE_POSITION_FACTOR) * (Velocity.x < 0 ? 1 : -1);
		}

		// Bounce with up and down
		if (Mathf.Approximately(prePosition.y + m_Radius, m_Border.y) ||
		    Mathf.Approximately(prePosition.y - m_Radius, - m_Border.y))
		{
			Velocity.y *= -1;
			bounced.y = (m_Radius + Constants.BOUNCE_POSITION_FACTOR) * (Velocity.y < 0 ? 1 : -1);
		}

		// Running bounce effect and reduce amount of bounce remain
		if (bounced != Vector2.zero)
			GameController.Instance.Bounce(prePosition + bounced);
	}

	// Calculate velocity to reflect when collide with obstacle
	void OnCollisionEnter2D(Collision2D col)
	{
		// No reflection if velocity and normal have similar direction
		if (Velocity.x * col.contacts[0].normal.x <= 0 ||
		    Velocity.y * col.contacts[0].normal.y <= 0)
		{
			Velocity = Vector3.Reflect(Velocity, col.contacts [0].normal);

			// Running bounce effect and reduce amount of bounce remain
			GameController.Instance.Bounce(col.contacts[0].point);
		}
	}

	public void EnterPortal(Vector3 pos)
	{
		m_Rigid.position = pos;
		prePosition = pos;
		Velocity = - Velocity;

		if (reEnable != null)
			StopCoroutine(reEnable);

		reEnable = ReEnableTrail();
		
		StartCoroutine(reEnable);
	}

	IEnumerator ReEnableTrail()
	{
		yield return new WaitForSeconds(0.2f);

		//Trail.time = 0.2f;
		Trail.enabled = true;
	}
}
