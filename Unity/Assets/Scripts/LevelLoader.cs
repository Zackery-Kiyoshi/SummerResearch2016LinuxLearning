using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class LevelLoader : MonoBehaviour {

	TextAsset theText = Resources.Load ("Text/TestLevel.txt") as TextAsset;
	string[] lines = theText.text.Split ('\n');

	List<Message> level;




	// Use this for initialization
	void Start () {
		
		for(int i=0; i< lines.Length; i++){
			string[] tmp = lines [i].Split ('\t');
			if (lines [i] [0] == 1) {
				// isPersons message
				Message tmpMsg = new Message (true, tmp [1]);
				level.Add (tmpMsg);
			} else if (lines [i] [0] == 0) {
				// isAI message
				Message tmpMsg = new Message (false, tmp [1]);
				level.Add (tmpMsg);
			} else if (lines [i] [0] == 'w') {
				// encode wait for command
				Command cmd = new Command(tmp[1],0);
				i++;
				while (lines [i] [0] == '<') {
					// encode the 
					tmp = lines [i].Split ('\t');
					Message tmpMsg = new Message (false, tmp [2]);
					tmpMsg.wait = tmp [1];
					tmpMsg.cmdWait = cmd;
					i++;
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
