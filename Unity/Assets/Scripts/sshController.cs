﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class sshController : MonoBehaviour {

	public class level{
		public bool required = true;
		public bool finished = false;
		public string username = "";
		public string port = "";
		public string comp = "";
		public string levelN = "";

		public level(bool r, string u, string p, string c, string l){
			required = r;
			username = u;
			port = p;
			comp = c;
			levelN = l;
		}
	}

	public int testing = 0;

	public string textLoad = "";
	public string hubName = "hub";
	public int numTutLevels = 0;
	public string username = "";

	public int hubNum = -1;

	public int curLevel = 0;
	public string save = "";

	public List<level> curLevels = new List<level> ();
	public List<level> allLevels = new List<level> ();

	private string hubScene = "";
	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (this.gameObject);

		TextAsset theText = Resources.Load ("Text/" + textLoad) as TextAsset;
		string[] tmpFile = theText.text.Split ('\n');

		for (int i = 1; i < tmpFile.Length; i++) {
			string[] tmp = tmpFile [i].Split ('\t');
			string tmpuser = "";
			string tmpport = "";
			if (tmp [1] != "!~")
				tmpuser = tmp [1].Trim();
			if (tmp [2] != "!~")
				tmpport = tmp [2].Trim();
			allLevels.Add( new level(tmp[0]=="1", tmpuser, tmpport, tmp[3].Trim(), tmp[4].Trim()) );
			if(testing > 0)Debug.Log (tmp[0] + ": '" + tmpuser +"','"+ tmpport +"','"+ tmp[3] +"','"+ tmp[4]+"'");
			if (tmp [3].Trim () == hubName) {
				hubNum = i;
				hubScene = tmp[4].Trim();
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void newGame(){
		string s = GameObject.Find ("UserField").GetComponent<InputField> ().text;
		Debug.Log ("username : " + s);

		if (s != "") {
			curLevel++;
			username = s;
			SceneManager.LoadScene (allLevels [curLevel - 1].levelN);
		}
	}


	public void loadSave(){
		string s = GameObject.Find ("InputField").GetComponent<InputField> ().text;
		Debug.Log (s);

		if (s != "" && s.Length%3 == 0) {

			save = s;

			string tmp = "";

			tmp = s.Substring(s.Length-3,3);
			int tmpLevel = int.Parse(tmp);
			Debug.Log (tmp + ":" + tmpLevel);
			curLevel = tmpLevel;

			tmp = s.Substring (0, s.Length - 3);
			Debug.Log ("username: " + tmp);
			string tmpUser = "";
			for (int i = 0; i < tmp.Length;) {
				string tmpL = "";
				tmpL += tmp [i];
				i++;
				tmpL += tmp [i];
				i++;
				tmpL += tmp [i];
				i++;
				tmpUser += System.Convert.ToChar (int.Parse (tmpL));

				i++;
			}
			Debug.Log(tmpUser);
			//System.Convert.ToChar(num)



			curLevel += hubNum;



			SceneManager.LoadScene (allLevels [hubNum-1].levelN);
		}
	}

	public string setSave(int x, int m, int s){
		int tmpSave = (curLevel - hubNum);
		if (tmpSave < 0)
			tmpSave = 0;
		save = "" + tmpSave;

		if (save.Length == 1) {
			save = "00" + save;
		} else if (save.Length == 2) {
			save = "0" + save;
		}

		string tmp = "";
		int num = 0;
		for (int i = 0; i < username.Length; i++) {
			//username[i] == 'a'
			num = System.Convert.ToInt32(username[i]);
			if (num < 10) {
				tmp += "00" + num;
			} else if (num < 100) {
				tmp += "0" + num;
			}else
				tmp += num;
		}

		save = tmp + save;

		return save;
	}

	public string getFinalProof(){
		// based on username
		string ret = "";

		return ret;
	}

	string getSceneInfo(List<level> t,int i){
		string ret = "";
		if (t [i].username == "NewUser") {
			ret += t [i].port + '\t' + username + '\t' + t [i].comp;
		} else {
			ret += t [i].port + '\t' + t [i].username + '\t' + t [i].comp;
		}
		return ret;
	}

	public string printAvailableLevels(bool f){
		string ret = "";

//		/*
		if (curLevel-1 < hubNum) {
			// still in tutorial
			//need to check if done with current level
			//Debug.Log ("in tutorial");
			if (f) {
				ret += getSceneInfo (allLevels, curLevel-1);
			}
		} else {
			// out of tutorial level
			//Debug.Log("HERES THE FUCK");
			if (SceneManager.GetActiveScene().name == hubScene) {
				// if in hub (show all possible options)
				//Debug.Log(hubNum +" : "+ curLevel);



				for (int i = hubNum; i < curLevel; i++) {
					ret += getSceneInfo (allLevels, i);
					if (i != curLevel - 1) {
						ret += '\n';
					}
				}
			} else {
				// if not show only hub
				Debug.Log (curLevel + ":" +hubNum + " : " );
				ret += getSceneInfo (allLevels, hubNum-1);
				// if done with level show the next level
				if (f && curLevel < allLevels.Count) {
					int tmpLvl = curLevel;
					while (tmpLvl >hubNum && !allLevels [tmpLvl].required) {
						tmpLvl--;
					}

					ret += '\n' + getSceneInfo (allLevels, tmpLvl);
				}
			}
		}
//		*/

		return ret;
	}

	/*
	public string sshopt(bool f){
		string terminal = "";
		if (curLevel < numTutLevels) {
			// still in tutorial
			//need to check if done with current level

			terminal += curLevels[curLevel].port + '\t' + curLevels [curLevel].username + '\t' + curLevels [curLevel].comp;
		} else {
			// out of tutorial level

			if (SceneManager.GetActiveScene().name == hubName) {
				// if in main menu (show all possible options)
				for (int i = hubNum; i < curLevels.Count; i++) {
					terminal += curLevels[i].port + '\t' + curLevels [i].username + '\t' + curLevels [i].comp;
					if (i != curLevels.Count - 1) {
						terminal += '\n';
					}
				}
			} else {
				// check if in tutorial
				if (curLevel < hubNum) {
					Debug.Log ("in tutorial");
					if (f) {
						terminal += allLevels [curLevel-1].port + '\t' + allLevels [curLevel-1].username + '\t' + allLevels [curLevel-1].comp;
					}
				} else {
					// if not show only hub
					Debug.Log (curLevel + ":" +hubNum + " : " + curLevels.Count);
					terminal += allLevels [hubNum].port + '\t' + allLevels [hubNum].username + '\t' + allLevels [hubNum].comp;
					// if done with level show the next level
					if (f) {
						terminal += '\n' + allLevels [curLevel].port + '\t' + allLevels [curLevel].username + '\t' + allLevels [curLevel].comp;
					}
				}
			}
		}
		return terminal;
	}
//	*/
	
	public void nextLevel(){
		// need to check that not in hub
		if (SceneManager.GetActiveScene ().name != hubScene) {
			if (curLevel < hubNum) {
				Debug.Log (curLevel + ":" + hubNum);
				curLevels = new List<level> ();
				curLevels.Add (allLevels [curLevel]);
				curLevel++;
				setSave (curLevel - hubNum, 0, 0);
			} else {
				//Debug.Log ("out of tutorial");

				// add the new scene to curlevels
				curLevels.Add (allLevels [curLevel]);
				curLevel++;

				// have gotten to hub

				setSave (curLevel - hubNum, 0, 0);
				if (curLevel < allLevels.Count) {
					Debug.Log ("pre:" + curLevel + " " + allLevels[curLevel].levelN);
					bool move = false;
					while ( curLevel < allLevels.Count && !allLevels [curLevel].required ) {
						move = true;
						Debug.Log (allLevels [curLevel].levelN);
						if (curLevel < allLevels.Count) {
							curLevels.Add (allLevels [curLevel]);
							curLevel++;
						} else {
							curLevel--;
							break;
						}
					}
					if ( curLevel < allLevels.Count ) {
						curLevels.Add (allLevels [curLevel]);
						curLevel++;
					}
					Debug.Log ("post:" + curLevel);
				}
			}

			if (curLevel > allLevels.Count) {
				curLevel--;
				if (curLevel < hubNum)
					curLevels.Clear ();
			}

		} else {
			Debug.Log ("in hub");
			if(!curLevels.Contains(allLevels[hubNum-1])){
				curLevels.Add(allLevels[hubNum-1]);
				Debug.Log("hub added");
				setSave (0, 0, 0);
			}
			if (curLevel < allLevels.Count) {
				Debug.Log ("pre:" + curLevel + " " + allLevels[curLevel].levelN);
				bool move = false;
				while ( curLevel < allLevels.Count && !allLevels [curLevel].required ) {
					move = true;
					Debug.Log (allLevels [curLevel].levelN);
					if (curLevel < allLevels.Count) {
						curLevels.Add (allLevels [curLevel]);
						curLevel++;
					} else {
						curLevel--;
						break;
					}
				}
				if ( curLevel < allLevels.Count ) {
					curLevels.Add (allLevels [curLevel]);
					curLevel++;
				}
				Debug.Log ("post:" + curLevel);
			}
		}


	}




}
