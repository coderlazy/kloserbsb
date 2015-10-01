using UnityEngine;
using System.Collections;

public class ggan : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AndroidGoogleAnalytics AGA = new AndroidGoogleAnalytics ();
		AGA.StartTracking ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
