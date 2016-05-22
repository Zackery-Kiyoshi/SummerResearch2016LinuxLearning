using UnityEngine;
using System.Collections;

public class PanelControl : MonoBehaviour {


	private MessageControl messaging;
	private TerminalControl terminal;

	private int curActive;

	// Use this for initialization
	void Start () {
		curActive = -1;
		messaging = gameObject.transform.Find ("MessagingPanel").gameObject.GetComponent<MessageControl> ();
		terminal = gameObject.transform.Find ("TerminalPanel").gameObject.GetComponent<TerminalControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActiveMessage(){
		curActive = 0;
		messaging.activate ();
		terminal.deactivate ();
	}

	public void ActiveTerminal(){
		curActive = 1;
		messaging.deactivate ();
		terminal.activate ();
	}

}
