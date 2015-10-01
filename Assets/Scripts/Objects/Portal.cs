using UnityEngine;
using System.Collections;

public enum PortalState
{
	Open,
	Close,
	Transfer
}

public class Portal : Entity
{
	public Portal m_Linked;
	public PortalState state;

	private static GameObject[] listPortals;

	void Start()
	{
		state = PortalState.Open;

		if (listPortals == null)
		{
			listPortals = GameObject.FindGameObjectsWithTag("portal");
		}
		

		for (int i = 0; i < listPortals.Length; i++)
		{
			if (listPortals[i].GetInstanceID() != gameObject.GetInstanceID())
			{
				m_Linked = listPortals[i].GetComponent<Portal>();
				break;
			}
		}
	}

	void OnTriggerEnter2D()
	{
		//Debug.LogWarning("enter " + inPortal + "  " );
		if (state == PortalState.Open )
		{
//			Debug.LogWarning("enter");
			state = PortalState.Close;
			m_Linked.state = PortalState.Transfer;

			GameValue.Ball.Trail.enabled = false;
			GameValue.Ball.EnterPortal(m_Linked.m_Trans.position);
		}
	}

	void OnTriggerExit2D()
	{
		//Debug.LogWarning("exit " + inPortal + "  " );
		//if (state == PortalState.Transfer)
		state = PortalState.Open;
//		{
//			Debug.LogWarning("exit");
//
//		}
	}

	void OnDestroy()
	{
		if (listPortals != null)
			listPortals = null;
	}
}
