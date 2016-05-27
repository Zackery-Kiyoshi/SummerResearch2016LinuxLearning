using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FileSystem {

	public class Folder {
		public string name;
		public List<Folder> contentFolders;
		public List<File> contentFiles;
		public string path;

		private Folder parent;

		public Folder getParent(){
			return parent;
		}

		public void setParent(Folder f){
			parent = f;
		}

		public Folder(string s, List<Folder> l, List<File> f, string p){
			name = s;
			contentFolders = l;
			contentFiles = f;
			path = p;
		}

		public void addFolder(string s){
			contentFolders.Add (new Folder(s, new List<Folder>(), new List<File>(), path+name));
			contentFolders [contentFolders.Count - 1].setParent (this);
		}

		public void addFile(string s, string c){
			contentFiles.Add (new File(s,c,path+name));
		}

		public bool moveFile(string f){



			return false;
		}

		public bool removeFolder(string f){
			int tmp = -1;
			bool found = false;
			for(int i=0; i< contentFolders.Count && !found; i++){
				if(contentFolders[i].name == f){
					tmp = i;
					found = true;
				}
			}
			if(found){
				if (contentFolders [tmp].contentFolders.Count == 0 && contentFolders [tmp].contentFiles.Count == 0) {
					contentFolders.RemoveAt(tmp);
					return true;
				} else {
					Debug.Log ("Can't remove a non empty folder");
					return false;
				}
			}
			else {
				Debug.Log("Couldn't find folder named: " + f);
				return false;
			}
		}

		public bool removeFile(string f){
			bool found = false;
			for(int i=0; i< contentFiles.Count && !found; i++){
				if(contentFiles[i].name == f){
					contentFiles.RemoveAt(i);
					found = true;
				}
			}
			if(found){
				return true;
			}
			else {
				Debug.Log("Couldn't find file named: " + f);
				return false;
			}
		}
	}

	public class File {
		public string name;
		public string content;
		public string path;

		public File(string s, string c, string p){
			name = s;
			content = c;
			path = p;
		}

		public void changeFile(string c){
			content = c;
		}

		public void renameFile(string n){
			name = n;
		}
	}

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

		for(int a = 0; a < tmp.Length; a++){
			Debug.Log("  " + tmp[a]);
		}
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
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						search = true;
						i++;
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
