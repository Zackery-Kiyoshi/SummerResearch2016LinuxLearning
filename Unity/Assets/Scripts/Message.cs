using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Message : MonoBehaviour {


	public string message = "";
	public bool isPerson = false;
	public int wait = 0;
	public List<Command> cmdWait = null;
	public string fsWait = null;
	public bool finished = false;
	// width 395
	// height 560

	public Text status;
	private Text body;

	private bool running = false;
	private const int maxPerLine = 27;
	private const int textHeight = 16;
	private int index = 0;
	private int count = 10;

	private float textWidth = 0;
	private float width;

	// Use this for initialization
	void Start () {
		status = gameObject.transform.Find ("TypingKey").gameObject.GetComponent<Text> ();
		body = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		//Debug.Log (status);
		//Debug.Log (body);
		width = body.GetComponent<RectTransform> ().rect.width;

		body.text = "";
		status.text = "";
	}

	public Message(bool ai, string m){
		isPerson = ai;
		message = m;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ( !isPerson +":"+ running +":"+ (wait == 0));
		if (wait != 0) {
			gameObject.SetActive (false);
			//Debug.Log ("HELP!!!");
		}
		else if (!isPerson && running ) {
			gameObject.SetActive (true);
			if (count == 0) {
				pressedKey ();
				count = 10;
			} else {
				count--;
			}
		}

	}

	public void pressedKey(){
		if (index < message.Length) {
			body.text += message [index];

			Font myFont = body.font;  
			CharacterInfo characterInfo = new CharacterInfo();
			myFont.GetCharacterInfo(message [index], out characterInfo, body.fontSize);
			textWidth += characterInfo.advance;

			index++;

			// need to check if the box needs to expand
			if ( textWidth >= width) {
				var t = gameObject.GetComponent<RectTransform> ();
				t.offsetMin = new Vector2(t.offsetMin.x, t.offsetMin.y-textHeight);
				t = status.GetComponent<RectTransform> ();
				t.offsetMin = new Vector2(t.offsetMin.x, t.offsetMin.y-textHeight);
				t.offsetMax = new Vector2(t.offsetMax.x, t.offsetMax.y-textHeight);
				textWidth = 0;
			}

		} else if (index == message.Length) {
			status.text = "Enter";
			index++;
		} else if (index == message.Length + 1) {
			status.text = "";
			running = false;
			finished = true;
			index++;
		}else return;
	}

	public void start(){
		if (body == null) {
			status = gameObject.transform.Find ("TypingKey").gameObject.GetComponent<Text> ();
			body = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		}
		width = body.GetComponent<RectTransform> ().rect.width;
		if (!isPerson) {
			body.alignment = TextAnchor.UpperRight;
			var tmp = 7 * status.GetComponent<RectTransform> ().position.x;
			status.GetComponent<RectTransform> ().position = new Vector3 (
				tmp, 
				status.GetComponent<RectTransform> ().position.y,
				status.GetComponent<RectTransform> ().position.z);
		}
		running = true;
		status.text = "Typing";
	}


	public IEnumerator waitSec(int i) {
		//print(Time.time);
		yield return new WaitForSeconds(i);
		//print(Time.time);
	}

	public float size(){
		float ret = 0;
		Font myFont = body.font;  
		for (int i = 0; i < message.Length; i++) {
			CharacterInfo characterInfo = new CharacterInfo ();
			myFont.GetCharacterInfo (message [i], out characterInfo, body.fontSize);
			ret += characterInfo.advance;
		}
		return ret / width;
	}
}
