using UnityEngine;
using System.Collections;

public class MessageControl : MonoBehaviour {

	private bool active;

	// Use this for initialization
	void Start () {
		active = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}

	public void activate(){
		active = true;
	}

	public void deactivate(){
		active = false;
	}
}
