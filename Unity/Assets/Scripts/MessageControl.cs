﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageControl : MonoBehaviour {

	public bool started = false;
	public bool ready = false;
	public TerminalControl tc;

	private bool active;
	public LevelLoader l;
	private int curMsg = 0;

	private float size = 560;
	//private float preHeight = 0;

	GameObject msg;
	private Command waiting = null;

	private int top = 0;

	//List<GameObject> msgs = new List<GameObject>();

	// Use this for initialization
	void Start () {
		active = false;
		l = gameObject.transform.parent.gameObject.GetComponent<LevelLoader>();
		l.load ();

		ready = true;

	}

	public void startMsg(){
		GameObject tmpMsg = l.level[curMsg] ;
		tmpMsg.transform.SetParent(gameObject.transform);

		// need to place
		tmpMsg.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
		tmpMsg.GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, -9);
		tmpMsg.GetComponent<RectTransform> ().offsetMin = new Vector2 (9, 515);
		tmpMsg.SetActive (true);

		Message tmp = tmpMsg.GetComponent<Message> ();
		tmp.waitSec (5);
		tmp.wait = 0;
		tmp.start ();
		//msgs.Add (tmpMsg);
		started = true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Debug.Log (curMsg);
		if (started && curMsg < l.level.Count) {
			Message tmpMsg = l.level [curMsg].GetComponent<Message> ();
			if (active && tmpMsg.isPerson) {
				if (Input.anyKeyDown) {
					tmpMsg.pressedKey ();
				}

			} else if (!tmpMsg.isPerson) {


			}
			scroll ();
			//Debug.Log (waiting == null);
			if (tmpMsg.finished && tmpMsg.status.text == "" && waiting == null) {
				//Debug.Log (l.level [curMsg].GetComponent<Message> ().wait);

				if (curMsg + 1 < l.level.Count && l.level [curMsg].GetComponent<Message> ().wait == 0) {

					curMsg++;



					//Debug.Log (curMsg);
					GameObject tMsg = l.level [curMsg];
					tMsg.transform.SetParent (gameObject.transform);
					// need to place
					//curBottom += 
					float preHeight = l.level [curMsg - 1].GetComponent<RectTransform> ().rect.height + 20;

					tMsg.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
					tMsg.GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [curMsg - 1].GetComponent<RectTransform> ().offsetMax.y - preHeight);
					tMsg.GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [curMsg - 1].GetComponent<RectTransform> ().offsetMin.y - 36 - 20);
					size -= preHeight;
					Message tmp = tMsg.GetComponent<Message> ();
					if (l.level [curMsg].GetComponent<Message> ().wait != 0) {
						waiting = l.level [curMsg].GetComponent<Message> ().cmdWait;
					}

					tmp.waitSec (5);
					tmp.start ();

					//msgs.Add (tMsg);
				} else {
					curMsg++;
				}
			}

		} else if (curMsg >= l.level.Count) {
			finished ();
		}

	}

	public void activate(){
		active = true;
	}

	public void deactivate(){
		active = false;
	}

	// need function to decrement waiting for commands

	public void processCmd(Command s){
		GameObject tMsg = l.level [curMsg];
		Message msg = tMsg.GetComponent<Message> ();
		//Debug.Log (msg.cmdWait.com +":"+s.com + "> ");
		//Debug.Log (msg.cmdWait.com == s.com);
		//if(msg.finished || msg.wait > 0){
			if (msg.wait > 0) {
				//Debug.Log ("is waiting");
				if (!s.error) {
					//Debug.Log ("No error");
				if (waiting.com.Trim() == s.com.Trim()) {
						//Debug.Log ("correct command");
						bool c = true;
						// need to check paramaters
						if (waiting.options.Count != 0) {
							for (int i = 0; i < waiting.options.Count; i++) {
								bool tmpC = false;
								for (int j = 0; j < s.options.Count; j++) {
									if (waiting.options [i].Trim() == s.options [j].Trim())
										tmpC = true;
								}
								c &= tmpC;
							}
						} else
							c = true;
						//Debug.Log ("options: " + c);

						// need to check options
						if (waiting.param.Count != 0) {
							for (int i = 0; i < waiting.param.Count; i++) {
								bool tmpC = false;
								for (int j = 0; j < s.param.Count; j++) {
									if (waiting.param [i].Trim() == s.param [j].Trim())
										tmpC = true;
								}
								c &= tmpC;
							}
						} else
							c &= true;
						//Debug.Log ("Params: " + c);

						if (c) {
							// correct cmd
							Debug.Log ("THEY DID IT");
							//Debug.Log (curMsg + ": " + l.level.Count);
							int tmpMsgIndex = curMsg - 1;
							while (l.level [curMsg].GetComponent<Message> ().cmdWait != null && curMsg < l.level.Count) {
								curMsg++;
								//Debug.Log (curMsg + ": " + l.level [curMsg].GetComponent<Message> ().message);
							}
							GameObject m = l.level [curMsg];
							m.transform.SetParent (gameObject.transform);
							// need to place
							float preHeight = l.level [tmpMsgIndex].GetComponent<RectTransform> ().rect.height + 20;

							m.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
							m.GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [tmpMsgIndex].GetComponent<RectTransform> ().offsetMax.y - preHeight);
							m.GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [tmpMsgIndex].GetComponent<RectTransform> ().offsetMin.y - preHeight);
							size -= preHeight;
							Message tmp = m.GetComponent<Message> ();
							if (tmp.wait != 0) {
								waiting = tmp.cmdWait;
							} else
								waiting = null;
							tmp.waitSec (5);
							tmp.start ();

						} else {
							msg.wait -= 1;
						}
					}
				} else {
					msg.wait -= 1;
				}
			} else {
				//Debug.Log ("waiting == 0");
				tMsg.SetActive (true);
			if (curMsg + 1 < l.level.Count && tMsg.GetComponent<Message>().finished) {
					if (l.level [curMsg + 1].GetComponent<Message> ().cmdWait != null || waiting == null) {
						//Debug.Log ("next msg is waiting as well");
						curMsg++;
						GameObject m = l.level [curMsg];
						m.transform.SetParent (gameObject.transform);
						// need to place
						float preHeight = l.level [curMsg - 1].GetComponent<RectTransform> ().rect.height + 20;

						m.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
						m.GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [curMsg - 1].GetComponent<RectTransform> ().offsetMax.y - preHeight);
						m.GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [curMsg - 1].GetComponent<RectTransform> ().offsetMin.y - preHeight);
						size -= preHeight;
						Message tmp = m.GetComponent<Message> ();
						tmp.waitSec (5);
						tmp.start ();
					} else {
						// check that the command is correct
						if (waiting.com == s.com) {
							//Debug.Log ("correct command");
							bool c = true;
							// need to check paramaters
							if (waiting.options.Count != 0) {
								for (int i = 0; i < waiting.options.Count; i++) {
									bool tmpC = false;
									for (int j = 0; j < s.options.Count; j++) {
										if (waiting.options [i] == s.options [j])
											tmpC = true;
									}
									c &= tmpC;
								}
							} else
								c = true;
							// need to check options
							if (waiting.param.Count != 0) {
								for (int i = 0; i < waiting.param.Count; i++) {
									bool tmpC = false;
									for (int j = 0; j < s.param.Count; j++) {
										if (waiting.param [i] == s.param [j])
											tmpC = true;
									}
									c &= tmpC;
								}
							} else
								c &= true;
							//Debug.Log ("HERE" + c);

							if (c) {
								// correct cmd
								Debug.Log ("THEY DID IT");
								//Debug.Log (curMsg + ": " + l.level.Count);
								int tmpMsgIndex = curMsg - 1;
								while (l.level [curMsg].GetComponent<Message> ().cmdWait != null && curMsg < l.level.Count) {
									curMsg++;
									//Debug.Log (curMsg + ": " + l.level [curMsg].GetComponent<Message> ().message);
								}
								GameObject m = l.level [curMsg];
								m.transform.SetParent (gameObject.transform);
								// need to place
								float preHeight = l.level [tmpMsgIndex].GetComponent<RectTransform> ().rect.height + 20;

								m.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
								m.GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [tmpMsgIndex].GetComponent<RectTransform> ().offsetMax.y - preHeight);
								m.GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [tmpMsgIndex].GetComponent<RectTransform> ().offsetMin.y - preHeight);
								size -= preHeight;
								Message tmp = m.GetComponent<Message> ();
								if (tmp.wait != 0) {
									waiting = tmp.cmdWait;
								} else
									waiting = null;
								tmp.waitSec (5);
								tmp.start ();

							}
						}
					}
				}
			//}
		}

	}


	void finished(){
		Debug.Log("FINISHED all messages");
	}

	void scroll(){
		float x = l.level [curMsg].GetComponent<Message> ().size ();
		if (l.level [curMsg].GetComponent<RectTransform> ().offsetMin.y < x + 20) {
			Debug.Log (top + "," + curMsg);
			l.level [top].SetActive (false);
			float up = l.level [top].GetComponent<RectTransform> ().rect.height + 20;

			//l.level [top].GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [top + 1].GetComponent<RectTransform> ().offsetMax.y + up);
			//l.level [top].GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [top + 1].GetComponent<RectTransform> ().offsetMin.y + up);

			for (int i = top; i <= curMsg; i++) {
				l.level [i].GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [i].GetComponent<RectTransform> ().offsetMax.y + up);
				l.level [i].GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [i].GetComponent<RectTransform> ().offsetMin.y + up);
			}
			top++;
			if (l.level [top].GetComponent<RectTransform> ().offsetMax.y < 9) {
				up = l.level [top].GetComponent<RectTransform> ().rect.height + 20;
				l.level [top].SetActive (false);
				for (int i = top; i <= curMsg; i++) {
					l.level [i].GetComponent<RectTransform> ().offsetMax = new Vector2 (-9, l.level [i].GetComponent<RectTransform> ().offsetMax.y + up);
					l.level [i].GetComponent<RectTransform> ().offsetMin = new Vector2 (9, l.level [i].GetComponent<RectTransform> ().offsetMin.y + up);
				}
				top++;
				scroll ();
			}
		}

	}

}
