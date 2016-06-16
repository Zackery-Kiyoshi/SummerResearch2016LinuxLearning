using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {
	
	public GameObject msgPanel;

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startLevel(){
		gameObject.SetActive (false);
		msgPanel.gameObject.GetComponent<MessageControl> ().startMsg ();
		gameObject.SetActive (false);
	}
}
