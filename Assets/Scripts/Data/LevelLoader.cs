using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
	private JSONObject LevelJSON;
	private List<Waypoint> LevelWaypoints;
	public GameObject slideToShoot;
	Vector3 startPos;
	GameObject myBall;

	void Awake()
	{
		if (ModelLevel.CurrentLevel == null || ModelLevel.CurrentLevel.levelOrder != GameValue.LevelOrder)
		{
			TextAsset dataFile = Resources.Load(Constants.PATH_LEVELS + GameValue.LevelOrder) as TextAsset;
			if (dataFile == null)
			{
				Application.LoadLevel("Level");
				return;
			}
			LevelJSON = new JSONObject(dataFile.text);
			ModelLevel.LoadLevel(LevelJSON, GameValue.LevelOrder);
		}
	}

	void Start()
	{
		GameValue.BounceRemain = ModelLevel.CurrentLevel.maxBounce;
		GameValue.CrystalRemain = ModelLevel.CurrentLevel.minCrystal;
		GameValue.GameTime = 0;
		// Create ballGameObject myBall;

		myBall = (GameObject.Instantiate (Resources.Load (
			Constants.PATH_OBJECTS + ModelLevel.CurrentLevel.ballData.objectType, 
			typeof(GameObject)), 
		                                 ModelLevel.CurrentLevel.ballData.position, 
		                                 Quaternion.identity) as GameObject);

		GameValue.Ball = myBall.GetComponent<BallController>();
		startPos = myBall.transform.position;
		StartCoroutine (enableSlideToShoot());
		// Create waypoints
		CreateWaypoints();
		// Create other objects
		CreateEntities();
		// Activate all waypoint
		ActivateWaypoint();
	}
	void Update(){
		if(startPos != myBall.transform.position){
			slideToShoot.SetActive(false);
			//slideToShoot.transform.position = myBall.transform.position;
		}
	}
	void CreateWaypoints()
	{
		if (ModelLevel.CurrentLevel.waypoints.Length > 0)
			LevelWaypoints = new List<Waypoint>();

		for (int i = 0; i < ModelLevel.CurrentLevel.waypoints.Length; i++)
		{
			Waypoint wp = (GameObject.Instantiate(
				Resources.Load(Constants.PATH_WAYPOINTS + ModelLevel.CurrentLevel.waypoints[i].Type), 
				ModelLevel.CurrentLevel.waypoints[i].position,
	            Quaternion.identity) 
               	as GameObject).GetComponent<Waypoint>();

			wp.Index = ModelLevel.CurrentLevel.waypoints[i].Index;
			wp.Generate();

			LevelWaypoints.Add(wp);
		}

		// Link waypoint to waypoint
		for (int i = 0; i < ModelLevel.CurrentLevel.waypoints.Length; i++)
		{
			LinkWaypointToEntity(LevelWaypoints[i].gameObject, ModelLevel.CurrentLevel.waypoints[i].linkWaypointIndex);
		}
	}

	void CreateEntities()
	{
		for (int layerIndex = 0; layerIndex < ModelLevel.CurrentLevel.layers.Count; layerIndex++)
		{
			string layerObjName = ModelLevel.CurrentLevel.layers[layerIndex].layerType;
			//Debug.Log(layerObjName);
			// Load layer object
			GameObject layerObj = Resources.Load(Constants.PATH_OBJECTS + layerObjName) as GameObject;
			
			for (int objIndex = 0; objIndex < ModelLevel.CurrentLevel.layers[layerIndex].objects.Length; objIndex++)
			{
				Entity obj;
				GameObject go;
				// Create entity
				if (ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].objectType.Equals(layerObjName))
					go = (GameObject.Instantiate(layerObj, 
		                      ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].position, 
		                      Quaternion.identity) as GameObject);
				else
					go = (GameObject.Instantiate(Resources.Load(Constants.PATH_OBJECTS + 
                     			ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].objectType), 
                              ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].position, 
                              Quaternion.identity) as GameObject);
				
				// Set entity transform
				Debug.Log(go.name);
				obj = go.GetComponent<Entity>();
				obj.Scale(ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].size);
				obj.Rotate(ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].rotation);

				// Link entity with waypoint if possible
				LinkWaypointToEntity(obj.gameObject,
					ModelLevel.CurrentLevel.layers[layerIndex].objects[objIndex].waypointIndex);
			}
		}
	}

	void LinkWaypointToEntity(GameObject entity, int index)
	{
		if (LevelWaypoints == null)
			return;

		for (int i = 0; i < LevelWaypoints.Count; i++)
		{
			if (LevelWaypoints[i].Index == index)
			{
				LevelWaypoints[i].LinkEntity(entity);
				break;
			}
		}
	}

	void ActivateWaypoint()
	{
		if (LevelWaypoints == null)
			return;

		for (int i = 0; i < LevelWaypoints.Count; i++)
		{
			LevelWaypoints[i].StartTween();
		}
	}
	IEnumerator enableSlideToShoot(){
		yield return new WaitForSeconds(5);
		if(startPos == myBall.transform.position){
			slideToShoot.SetActive(true);
			slideToShoot.transform.position = new Vector3(myBall.transform.position.x, myBall.transform.position.y + 2, slideToShoot.transform.position.z);
		}
	}
}
