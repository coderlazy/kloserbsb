using UnityEngine;
using System.Collections;

public class ButtonGame : MonoBehaviour {
	public GameObject soundOn;
	public GameObject soundOf;
//	public GameObject cam;
	GameObject game;
	GameObject nonDestroy;
	// Use this for initialization
	void Start(){
//		cam = GameObject.Find ("Main Camera");
//		game = GameObject.Find ("Game");
//		nonDestroy = GameObject.Find ("NonDestroyObject");

		if (PlayerPrefs.GetInt ("sound") == 1) {
			SoundOffOnClick ();
		} else {
			SoundOnOnClick();
		}

	}
	void Update(){
//		sound.text = GameObject.Find ("Main Camera").GetComponent<AudioListener> ().enabled + "";
	}
	public void SoundOnOnClick(){
		AudioListener.volume = 0;
		soundOf.SetActive (true);
		soundOn.SetActive (false);
		PlayerPrefs.SetInt ("sound", -1);
	}
	public void SoundOffOnClick(){
		AudioListener.volume = 1;
		soundOf.SetActive (false);
		soundOn.SetActive (true);
		PlayerPrefs.SetInt ("sound", 1);
	}
}
