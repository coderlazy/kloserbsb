using UnityEngine;
using System.Collections.Generic;

public class ObjectData
{
	public string objectType;
	public Vector2 position;
	public Vector2 size;
	public float rotation;
	public int waypointIndex;
}

public class LayerData
{
	public string layerType;
	public ObjectData[] objects;
}

public class WaypointData
{
	public string Type;
	public int Index;
	public Vector2 position;
	public Vector2 target;
	public float duration;
	public int linkWaypointIndex;
}

public class LevelData
{
	public int levelOrder;
	public int maxBounce;
	public int minCrystal;
	public Vector2 milestones;
	public ObjectData ballData;
	public List<LayerData> layers;
	public WaypointData[] waypoints;
}