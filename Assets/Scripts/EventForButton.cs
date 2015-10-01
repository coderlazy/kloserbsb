using UnityEngine;
using System.Collections;

public class EventForButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//SendEmail ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			//Application.LoadLevel(0);
		}
	}
	void SendEmail ()
		
	{
		
		string email = "cuongbui.203@gmail.com";
		
		string subject = MyEscapeURL("Bouncing ball shooter - Feedback");
		
		string body = MyEscapeURL("My Body\r\nFull of non-escaped chars");
		
		
		Application.OpenURL ("mailto:" + email + "?subject=" + subject + "&body=" + body);
		
	}  
	
	string MyEscapeURL (string url)
		
	{
		
		return WWW.EscapeURL(url).Replace("+","%20");
		
	}
}
