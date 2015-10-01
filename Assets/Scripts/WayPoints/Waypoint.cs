using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour 
{
	public int Index;

	protected WaypointData Data;
	protected UITweener m_Tween;

	public virtual void Generate() 
	{
		Data = ModelLevel.GetWaypointByIndex(Index);
	}

	public virtual void LinkEntity(GameObject entity) 
	{
		entity.transform.parent = transform;
	}

	public virtual void StartTween() 
	{
		m_Tween.ignoreTimeScale = false;
		m_Tween.ResetToBeginning();
		m_Tween.PlayForward();
	}
}
