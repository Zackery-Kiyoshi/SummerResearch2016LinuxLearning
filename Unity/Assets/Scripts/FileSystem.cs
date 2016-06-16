using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSystem {

	public Folder root;
	public Folder curFolder;
	public string curPath;

	public string defaultpath = "/";


	// Use this for initialization
	void Start () {
		root = new Folder ("/",new List<Folder>(),new List<File>(), "/");
		curFolder = root;
		curFolder.setParent (curFolder);
		curPath = "/";
	}

	public FileSystem(){
		root = new Folder ("/",new List<Folder>(),new List<File>(), "/");
		curFolder = root;
		curFolder.setParent (curFolder);
		curPath = "/";
	}

	// Update is called once per frame
	void Update () {
	
	}

	// checks if a path is valid
	public bool checkPath(string p){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		/*
		for(int a = 0; a < tmp.Length; a++){
			Debug.Log("  " + tmp[a]);
		}
		*/

		while (i<tmp.Length) {
			bool search = false;
			if (tmp [i] == "..") {
				Debug.Log ("Found ..");
				//if (tmpFold != root) {
					tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						search = true;
						i++;
						break;
					}
				}
				if (!search)
					return false;
			}
		}

		Debug.Log ("FOUND");
		return true;
	}

	public bool moveTo(string p){
		string[] tmp = p.Split (new[] { '/' });

		if (tmp [0] != "") {
			
			//relative directory
			Folder tmpFold = curFolder;
			int i = 0;
			while (i<tmp.Length) {
				bool search = false;
				if (tmp [i] == "..") {
					if (tmpFold != root) {
						curFolder = tmpFold.getParent ();
						i++;
					}
					i++;
				} else {
					for (int j = 0; j < tmpFold.contentFolders.Count; j++) {
						if (tmpFold.contentFolders [j].name == tmp [i]) {
							search = true;
							curFolder = tmpFold.contentFolders [j];
							i++;
							break;
						}
					}
					if (!search)
						return false;
				}
			}

		} else {



		}
		Debug.Log ("FOUND");
		return true;
	}

}
