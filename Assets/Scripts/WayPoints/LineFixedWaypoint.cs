using UnityEngine;
using System.Collections;

public class LineFixedWaypoint : Waypoint 
{
	public override void LinkEntity(GameObject entity)
	{
		TweenPosition tp = entity.AddComponent<TweenPosition>();
		tp.from = Data.position;
		tp.to = Data.target;
		tp.style = UITweener.Style.PingPong;
		tp.duration = Data.duration;
		tp.enabled = false;
		m_Tween = tp;
	}
}
