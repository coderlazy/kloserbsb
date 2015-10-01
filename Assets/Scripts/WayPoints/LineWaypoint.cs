using UnityEngine;
using System.Collections;

public class LineWaypoint : Waypoint 
{
	public override void Generate()
	{
		base.Generate();

		TweenPosition tp = gameObject.AddComponent<TweenPosition>();
		tp.from = Data.position;
		tp.to = Data.target;
		tp.style = UITweener.Style.PingPong;
		tp.duration = Data.duration;
		tp.enabled = false;
		m_Tween = tp;
	}
}
