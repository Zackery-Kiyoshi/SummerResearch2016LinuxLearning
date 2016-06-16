using UnityEngine;
using System.Collections;

public class StartDebrifing : MonoBehaviour {

	public Message debrif;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (debrif.finished) {
			debrif.gameObject.transform.FindChild ("StartLevel").gameObject.SetActive (true);
		}
	}

	public void startDebrifing(){
		LevelLoader l = gameObject.transform.parent.gameObject.GetComponent<LevelLoader> ();
		l.load ();

		debrif.message = l.description;

		//Debug.Log (l.description);
		//Debug.Log (debrif.message);

		debrif.gameObject.SetActive(true);
		debrif.waitSec (5);
		debrif.start ();
	}
}
