using UnityEngine;
using System.Collections;

public class CircleWaypoint : Waypoint 
{
	public override void Generate()
	{
		base.Generate();

		TweenRotation tr = gameObject.AddComponent<TweenRotation>();
		tr.from = Vector3.zero;
		tr.to = new Vector3(0, 0, Data.duration > 0 ? 360 : -360);
		tr.style = UITweener.Style.Loop;
		tr.duration = Data.duration > 0 ? Data.duration : -Data.duration;
		tr.enabled = false;
		m_Tween = tr;
	}
}
