using UnityEngine;
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
	private List<Command> waiting = null;

	private int top = 0;

	public bool f = false;

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
		tc = GameObject.Find ("TerminalPanel").GetComponent<TerminalControl>();
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
			if ( !f ) {
				f = true;
				finished ();
			}
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
		if (!f) {
			GameObject tMsg = l.level [curMsg];
			Message msg = tMsg.GetComponent<Message> ();
			bool done = false;
			if (waiting == null)
				waiting = msg.cmdWait;
			//Debug.Log (msg.cmdWait.com +":"+s.com + "> ");
			//Debug.Log (msg.cmdWait.com == s.com);
			//if(msg.finished || msg.wait > 0){
			if (msg.wait > 0 || msg.cmdWait != null) {
				//Debug.Log ("is waiting");
				Debug.Log ("pre: " + msg.wait);
				Debug.Log (waiting + " : " + waiting.Count);

				if (msg.cmdWait != null) {
					// waiting for cmd
					bool cor = false;

					for (int k = 0; k < waiting.Count && !done; k++) {
						if (!s.error) {
							//Debug.Log ("No error");
							if (waiting [k].com.Trim () == s.com.Trim ()) {
								//Debug.Log ("correct command");
								bool c = true;
								// need to check paramaters
								if (waiting [k].options.Count != 0) {
									for (int i = 0; i < waiting [k].options.Count; i++) {
										bool tmpC = false;
										for (int j = 0; j < s.options.Count; j++) {
											if (waiting [k].options [i].Trim () == s.options [j].Trim ())
												tmpC = true;
										}
										c &= tmpC;
									}
								} else
									c = true;
								//Debug.Log ("options: " + c);

								// need to check options
								if (waiting [k].param.Count != 0) {
									for (int i = 0; i < waiting [k].param.Count; i++) {
										bool tmpC = false;
										for (int j = 0; j < s.param.Count; j++) {
											if (waiting [k].param [i].Trim () == s.param [j].Trim ())
												tmpC = true;
										}
										c &= tmpC;
									}
								} else
									c &= true;
								//Debug.Log ("Params: " + c);

								if (c) {
									// correct cmd
									Debug.Log ("THEY DID IT (waiting !=0)");
									done = true;
									//Debug.Log (curMsg + ": " + l.level.Count);
									int tmpMsgIndex = curMsg - 1;
									while (l.level [curMsg].GetComponent<Message> ().cmdWait == waiting && curMsg < l.level.Count) {
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
									}
									tmp.waitSec (5);
									tmp.start ();
									cor = true;
								} else {
									//msg.wait -= 1;
								}
							}
						} else {
							//msg.wait -= 1;
						}
					}
					if (!cor) {
						msg.wait -= 1;
						Debug.Log ("incorrect command");
					}
				} else {
					// wait fs stuff
					bool c = true;

					// parsing the 
					string cur = l.level [curMsg].GetComponent<Message> ().fsWait;
					string[] spCur = cur.Split ('\t');
					string[] path;
					FileSystem fs = tc.fileSystem;

					if (spCur [0] == "curFolder") {
						// travers the path to get to curfolder

						// check the curfolder

					} else if (spCur [0] == "rm") {
						for (int i = 1; i < spCur.Length; i++) {
							path = spCur [i].Split ('/');
							// traverse path to see if the file/folder dont exist

						}

					} else if (spCur [0] == "add") {
						for (int i = 1; i < spCur.Length && c; i++) {
							path = spCur [i].Split ('/');
							// traverse path to see if the file/folder exist
							Folder curFold = fs.root;
							for (int j = 0; j < path.Length - 1 && c; j++) {
								bool search = false;
								for (int k = 0; k < curFold.contentFolders.Count && !search; k++) {
									if (curFold.contentFolders [k].name.Trim () == path [j].Trim ()) {
										search = true;
										curFold = curFold.contentFolders [k];
									}
								}
								if (!search) {
									c = false;
								}
							}
							// check for the actual new file/folder that 
							if (c) {
								bool search = false;



								if (!search) {
									c = false;
								}
							}
						}

					}


					if (c) {
						// correct cmd
						Debug.Log ("THEY DID IT " + waiting[0].com);
						done = true;
						//Debug.Log (curMsg + ": " + l.level.Count);
						int tmpMsgIndex = curMsg - 1;
						while (l.level [curMsg].GetComponent<Message> ().fsWait == cur && curMsg < l.level.Count) {
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
						}
						tmp.waitSec (5);
						tmp.start ();

					} else {
						msg.wait -= 1;

					}
				}
			} else {
				
				Debug.Log ("waiting == 0");
				tMsg.SetActive (true);
				if (waiting != null) {
					for (int k = 0; k < waiting.Count && !done; k++) {
						if (waiting [k].com == s.com) {
							//Debug.Log ("correct command");
							bool c = true;
							// need to check paramaters
							if (waiting [k].options.Count != 0) {
								for (int i = 0; i < waiting [k].options.Count; i++) {
									bool tmpC = false;
									for (int j = 0; j < s.options.Count; j++) {
										if (waiting [k].options [i] == s.options [j])
											tmpC = true;
									}
									c &= tmpC;
								}
							} else
								c = true;
							// need to check options
							if (waiting [k].param.Count != 0) {
								for (int i = 0; i < waiting [k].param.Count; i++) {
									bool tmpC = false;
									for (int j = 0; j < s.param.Count; j++) {
										if (waiting [k].param [i] == s.param [j])
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
								done = true;
							}
						} else {


						}
					}
				}
			}
			if(msg.wait == 0){
				if (curMsg + 1 < l.level.Count && tMsg.GetComponent<Message> ().finished) {
					if (l.level [curMsg + 1].GetComponent<Message> ().cmdWait == waiting || waiting == null) {
						Debug.Log ("next msg is waiting as well for the same commands or not waiting");
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
						if (tmp.wait != 0) {
							waiting = tmp.cmdWait;
						} else
							waiting = null;
						tmp.waitSec (5);
						tmp.start ();
						// check that the command is correct

					}
				}
			}
			Debug.Log ("post: " + msg.wait);
		}
	}


	void finished(){
		Debug.Log("FINISHED all messages");

		tc.sshc.nextLevel ();

		f = true;
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
