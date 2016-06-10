using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message : MonoBehaviour {


	public string message = "";
	public bool isPerson = false;
	public int wait = 0;
	public Command cmdWait;

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
	private int count = 10;

	private float textWidth = 0;
	private float width;

	// Use this for initialization
	void Start () {
		status = gameObject.transform.Find ("TypingKey").gameObject.GetComponent<Text> ();
		body = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
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
				textWidth = 0;
			}

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

	IEnumerator wait(int i) {
		//print(Time.time);
		yield return new WaitForSeconds(i);
		//print(Time.time);
	}

}
