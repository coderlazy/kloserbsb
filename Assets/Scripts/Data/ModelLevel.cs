using UnityEngine;
using System.Collections.Generic;

public static class ModelLevel 
{
	//public static List<LevelData> Levels = new List<LevelData>();

	public static LevelData CurrentLevel;

	public static void LoadLevel(JSONObject levelJSON, int order)
	{
		CurrentLevel = new LevelData();

		// Load level order, bounces, crystals
		CurrentLevel.levelOrder = order;
		CurrentLevel.maxBounce = int.Parse(levelJSON["properties"]["bounce"].str);
		CurrentLevel.minCrystal = int.Parse(levelJSON["properties"]["crystal"].str);

		// Load milestones
		string[] milestones = levelJSON["properties"]["milestone"].str.Split(',');
		CurrentLevel.milestones = new Vector2(float.Parse(milestones[0]), float.Parse(milestones[1]));

		// Load layers
		CurrentLevel.layers = new List<LayerData>();

		for (int i = 1; i < levelJSON["layers"].list.Count; i++)
		{
			if (levelJSON["layers"][i]["name"].str.ToLower().Equals("ball"))
				CurrentLevel.ballData = AddObject(levelJSON["layers"][i]["objects"][0]);
			else if (levelJSON["layers"][i]["name"].str.ToLower().Equals("waypoint"))
				CurrentLevel.waypoints = AddWaypointLayer(levelJSON["layers"][i]["objects"]);
			else
				CurrentLevel.layers.Add(AddLayer(levelJSON["layers"][i]));
		}

		// init empty waypoint array
		if (CurrentLevel.waypoints == null)
			CurrentLevel.waypoints = new WaypointData[0];
	}

	public static WaypointData GetWaypointByIndex(int index)
	{
		for (int i = 0; i < CurrentLevel.waypoints.Length; i++)
		{
			if (CurrentLevel.waypoints[i].Index == index)
				return CurrentLevel.waypoints[i];
		}

		return null;
	}

	private static LayerData AddLayer(JSONObject layerJSON)
	{
		LayerData ly = new LayerData();

		ly.layerType = layerJSON["name"].str.ToLower();
		ly.objects = new ObjectData[layerJSON["objects"].list.Count];

		for (int i = 0; i < layerJSON["objects"].list.Count; i++)
		{
			ly.objects[i] = AddObject(layerJSON["objects"][i]);
		}

		return ly;
	}

	private static WaypointData[] AddWaypointLayer(JSONObject wplJSON)
	{
		WaypointData[] wps = new WaypointData[wplJSON.list.Count];

		for (int i = 0; i < wps.Length; i++)
		{
			wps[i] = AddWaypoint(wplJSON[i]);
		}

		return wps;
	}

	private static ObjectData AddObject(JSONObject objJSON)
	{
		ObjectData obj = new ObjectData();
		obj.objectType = objJSON["name"].str.ToLower();
		obj.rotation = -objJSON["rotation"].f;

		if (objJSON["properties"]["waypoint"])
			obj.waypointIndex = int.Parse(objJSON["properties"]["waypoint"].str);

		Vector4 values = new Vector4(objJSON["x"].f, objJSON["y"].f, objJSON["width"].f, objJSON["height"].f);
		values.x = values.x - Constants.SCREEN_WIDTH_BY_PIXEL / 2;
		values.y = Constants.SCREEN_HEIGHT_BY_PIXEL - values.y - Constants.SCREEN_HEIGHT_BY_PIXEL / 2;

		float alpha = obj.rotation * Mathf.Deg2Rad - Mathf.Atan2(values.w, values.z);
		float h = Mathf.Sqrt(values.z * values.z + values.w * values.w) / 2;

		values.x += Mathf.Cos(alpha) * h;
		values.y += Mathf.Sin(alpha) * h;
		
		values *= Constants.FACTOR_PIXEL_TO_METER;

		obj.position = new Vector2(values.x, values.y);
		obj.size = new Vector2(values.z, values.w);

		return obj;
	}

	private static WaypointData AddWaypoint(JSONObject waypointJSON)
	{
		WaypointData waypoint = new WaypointData();
		waypoint.Type = waypointJSON["type"].str.ToLower();
		waypoint.Index = int.Parse(waypointJSON["name"].str);
		waypoint.duration = float.Parse(waypointJSON["properties"]["time"].str);

		if (waypointJSON["properties"]["waypoint"])
			waypoint.linkWaypointIndex = int.Parse(waypointJSON["properties"]["waypoint"].str);

		Vector4 values;

		switch (waypoint.Type)
		{
			case "line":
			case "line-fixed":
				values = new Vector4(waypointJSON["x"].f, waypointJSON["y"].f, waypointJSON["polyline"][1]["x"].f, waypointJSON["polyline"][1]["y"].f);
				
				values.x = values.x - Constants.SCREEN_WIDTH_BY_PIXEL / 2;
				values.y = Constants.SCREEN_HEIGHT_BY_PIXEL - values.y - Constants.SCREEN_HEIGHT_BY_PIXEL / 2;
				values.z = values.x + values.z ;
				values.w = values.y - values.w;
				
				values *= Constants.FACTOR_PIXEL_TO_METER;
				
				waypoint.position = new Vector2(values.x, values.y);
				waypoint.target = new Vector2(values.z, values.w);
				break;

			case "circle":
				values = new Vector4(waypointJSON["x"].f, waypointJSON["y"].f, waypointJSON["width"].f, waypointJSON["height"].f);
				
				values.x = values.x - Constants.SCREEN_WIDTH_BY_PIXEL / 2;
				values.y = Constants.SCREEN_HEIGHT_BY_PIXEL - values.y - Constants.SCREEN_HEIGHT_BY_PIXEL / 2;
				
				values.x += values.z / 2;
				values.y += - values.w / 2;
				
				values *= Constants.FACTOR_PIXEL_TO_METER;
				
				waypoint.position = new Vector2(values.x, values.y);
				waypoint.target = new Vector2(values.z, values.w);
				break;
		}

		return waypoint;
	}
}
