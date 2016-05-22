using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*

pwd
cd
ls

*/



public class TerminalControl : MonoBehaviour {

	private bool active;

	private List<string> cmdHistory = new List<string> ();
	private int curCmdNum = 0;
	private string curLine = "";
	private string curParam = "";

	// Use this for initialization
	void Start () {
		active = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active) {
			if (Input.anyKeyDown) {
				
			}
		}
	}

	public void activate(){
		active = true;
	}

	public void deactivate(){
		active = false;
	}
}
