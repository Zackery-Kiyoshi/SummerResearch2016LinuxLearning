using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message : MonoBehaviour {


	public string message = "";
	public bool isPerson = false;


	// width 395
	// height 560

	private Text status;
	private Text body;

	private string end = "Enter";
	private string type = "Typing";

	private bool running = false;
	private const int maxPerLine = 27;
	private const int textHeight = 16;
	private int index = 0;
	private int lineTracker = 0;
	private int count = 10;

	// Use this for initialization
	void Start () {
		status = gameObject.transform.Find ("TypingKey").gameObject.GetComponent<Text> ();
		body = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();

		body.text = "";
		status.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPerson && running) {
			if (count == 0) {
				pressedKey ();
				count = 10;
			} else {
				count--;
			}
		}
	}

	public void pressedKey(){
		if (index <= message.Length) {
			body.text += message [index];
			index++;
		} else if (index == message.Length) {
			status.text = end;
			index++;
		} else if (index == message.Length + 1) {
			done ();
			index++;
		}else return;
	}
	public void start(){
		running = true;
		status.text = type;
	}

	public void done(){
		status.text = "";
	}
}
