using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour{

	public string LoadFile;
	public string description;

	TextAsset theText;
	string[] lines;
	string[] FileLines;
	Folder curFolder;

	public List<GameObject> level = new List<GameObject>();
	public FileSystem fs = new FileSystem();

	private bool loaded = false;

	// Use this for initialization
	void Start () {
		load ();
	}

	public void load(){
		if (!loaded) {
			loaded = true;
			GameObject msg = (GameObject)Resources.Load ("Prefabs/Message");

			theText = Resources.Load ("Text/" + LoadFile) as TextAsset;

			string[] tmpFile = theText.text.Split (new string[]{ "<div>" }, StringSplitOptions.RemoveEmptyEntries);


			description = tmpFile [0];


			FileLines = tmpFile [1].Split ('\n');
			curFolder = fs.root;

			for (int i = 0; i < FileLines.Length - 1; i++) {
				if (FileLines [i] == "") {
					
				}
				else if (FileLines [i] [0] == '{') {
					// enter subfolder
					//Debug.Log("enter subfolder");
					if (curFolder.contentFolders.Count > 0) {
						curFolder = curFolder.contentFolders [curFolder.contentFolders.Count - 1];
					}
				} else if (FileLines [i] [0] == '}') {
					// exit subfolder
					//Debug.Log("exit subfolder");
					if (curFolder.name != "/") {
						curFolder = curFolder.parent;
					}
				} else {
					//Debug.Log (i + ": " + FileLines [i]);

					// just create new file/folder
					string[] cur = FileLines [i].Split ('\t');
					//Debug.Log (FileLines[i] + " : " + cur.Length);
					if(cur.Length == 8)
						curFolder.add (FileLines [i] [0] == 'd', cur [0].Substring (1), cur [1], cur [2], Int32.Parse (cur [3]), cur [4], cur [5], Int32.Parse( cur [6])==1, cur[7]);
				}
			}

			lines = tmpFile [2].Split ('\n');
			/*
		Debug.Log ("Text/" + LoadFile);
		for (int i = 0; i < lines.Length; i++) {
			Debug.Log (i + ": " + lines [i]);
		}
//		*/
			for (int i = 0; i < lines.Length; i++) {
				string[] tmp = lines [i].Split ('\t');
				if (tmp [0] == "1") {
					// isPersons message
					GameObject tmpMsg = (GameObject)Instantiate (msg);
					//new Message (true, tmp [1]);
					tmpMsg.GetComponent<Message> ().isPerson = true;
					tmpMsg.GetComponent<Message> ().message = tmp [1];
					level.Add (tmpMsg);
					//tmpMsg.SetActive (false);
				} else if (tmp [0] == "0") {
					// isAI message
					GameObject tmpMsg = (GameObject)Instantiate (msg);
					//new Message (false, tmp [1]);
					tmpMsg.GetComponent<Message> ().isPerson = false;
					tmpMsg.GetComponent<Message> ().message = tmp [1];
					level.Add (tmpMsg);
					//tmpMsg.SetActive (false);
				} else if (tmp [0] == "wait") {
					// encode wait for command
					List<Command> cmds = new List<Command> ();

					for (int q = 1; q < tmp.Length; q++) {

						Command cmd = new Command (tmp [q], 0);
						q++;
						if (tmp [q] != "!~") {
							string[] a = tmp [q].Split (' ');
							List<string> t = new List<string> ();
							foreach (string f in a) {
								t.Add (f);
							}
							List<string> s = new List<string> ();
							for (int j = 0; j < t.Count; j++) {
								string tmps = t [j];
								for (int k = 1; k < tmps.Length; k++) {
									s.Add ("" + tmps [k]);
								}
							}
							cmd.options = s;
						}
						q++;
						if (tmp [q] [0] != '!' && tmp [q] [1] != '~') {
							string[] a = tmp [q].Split (' ');
							//Debug.Log (tmp [3]);
							//Debug.Log (tmp [3] == "!~");
							List<string> t = new List<string> ();
							foreach (string f in a) {
								t.Add (f);
							}
							cmd.param = t;
						}
						cmds.Add (cmd);
					}
					i++;
					//Debug.Log (i);
					// got command
					while (lines [i] != "" && i < lines.Length && lines [i] [0] == '<') {
						//Debug.Log (i + ": " + lines[i]);
						// encode the 
						tmp = lines [i].Split ('\t');
						GameObject tmpMsg = (GameObject)Instantiate (msg);
						//new Message (false, tmp [2]);
						tmpMsg.GetComponent<Message> ().isPerson = false;
						tmpMsg.GetComponent<Message> ().message = tmp [2];
						if (Int32.Parse (tmp [1]) == 1) {
							tmpMsg.GetComponent<Message> ().wait = Int32.Parse (tmp [1]);
						} else {
							tmpMsg.GetComponent<Message> ().wait = Int32.Parse (tmp [1]) - 1;
						}
						tmpMsg.GetComponent<Message> ().cmdWait = cmds;
						level.Add (tmpMsg);
						//tmpMsg.SetActive (false);
						i++;
					}
					i--;
				} else if (tmp [0] == "waitfs") {
					string cmds = tmp [1];
					i++;
					while (lines [i] != "" && i < lines.Length && lines [i] [0] == '<') {
						//Debug.Log (i + ": " + lines[i]);
						// encode the 
						tmp = lines [i].Split ('\t');
						GameObject tmpMsg = (GameObject)Instantiate (msg);
						//new Message (false, tmp [2]);
						tmpMsg.GetComponent<Message> ().isPerson = false;
						tmpMsg.GetComponent<Message> ().message = tmp [2];
						tmpMsg.GetComponent<Message> ().wait = Int32.Parse (tmp [1]) - 1;
						tmpMsg.GetComponent<Message> ().fsWait = cmds;
						level.Add (tmpMsg);
						//tmpMsg.SetActive (false);
						i++;
					}
					i--;
				}
			}
		}
		// mc.startMsg ();
	}



	// Update is called once per frame
	void Update () {
	
	}
}
