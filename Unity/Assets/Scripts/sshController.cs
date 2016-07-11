using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class sshController : MonoBehaviour {

	public class level{
		public string username = "";
		public string port = "";
		public string comp = "";
		public string levelN = "";

		public level(string u, string p, string c, string l){
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

	public int hubNum = -1;

	public int curLevel = 0;
	public string save = "";

	public List<level> curLevels = new List<level>();

	public List<level> allLevels = new List<level> ();
	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (this.gameObject);

		TextAsset theText = Resources.Load ("Text/" + textLoad) as TextAsset;

		string[] tmpFile = theText.text.Split ('\n');

		for (int i = 1; i < tmpFile.Length; i++) {
			string[] tmp = tmpFile [i].Split ('\t');
			string tmpuser = "";
			string tmpport = "";
			if (tmp [0] != "!~")
				tmpuser = tmp [0].Trim();
			if (tmp [1] != "!~")
				tmpport = tmp [1].Trim();
			allLevels.Add( new level(tmpuser, tmpport, tmp[2].Trim(), tmp[3].Trim()) );
			if(testing > 0)Debug.Log (tmpuser +","+ tmpport +","+ tmp[2] +","+ tmp[3]);
			if (tmp [2].Trim () == hubName)
				hubNum = i;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void newGame(){
		curLevel++;
		SceneManager.LoadScene (allLevels [curLevel-1].levelN);
	}


	public string loadSave(string s){

		return s;
	}

	public string setSave(int x, int m, int s){


		return save;
	}

	public void nextLevel(){


		if (curLevel < hubNum) {
			curLevels = new List<level> ();
			curLevels.Add (allLevels [curLevel]);
			curLevel++;
		} else {
			curLevels.Add (allLevels[curLevel]);
			curLevel++;
		}

		if (curLevel > allLevels.Count) {
			curLevel--;
			if (curLevel < hubNum)
				curLevels.Clear ();
		}



		// add the new scene to curlevels
	}




}
