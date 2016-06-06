using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestingInputField : MonoBehaviour {

    public string toPrint = "TEST";
	public bool testing = false;

    private int place = 0;
    private string message = "";
    private Text text;
	private InputField field;
	// Use this for initialization
	void Start () {
		text = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
		field = gameObject.GetComponent<InputField> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator wait(float f){
		yield return new WaitForSeconds (f);
	}

    public void changed(string s)
    {
		if (text.text == "") {
			
		}
			
		if(testing) Debug.Log("Text Changed: " + s);
		if (toPrint.Length > place && Input.anyKeyDown) {
			message = message + toPrint [place];
			place++;
			if (testing)
				Debug.Log (message);

		} else {
			field.interactable = false;
		}

		field.text = message;
		wait (.5f);
    }

    public void end(string s)
    {
		if(testing) Debug.Log("Done Changing: " + s);
    }
}
