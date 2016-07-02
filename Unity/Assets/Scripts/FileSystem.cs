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
	public bool checkPath(string p, bool f){
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
			Debug.Log("a:" + tmp[a]);
		}
	//	*/

		while (i<tmp.Length) {
			bool search = false;
			if (tmp [i] == "..") {
				//Debug.Log ("Found ..");
				//if (tmpFold != root) {
					tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
//					Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name.Trim() == tmp [i].Trim()) {
						//Debug.Log (tmpFold.name + ":" +j);
						//Debug.Log ("Found: " + tmpFold.contentFolders [j].name);
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
				if (i == tmp.Length-1 && f) {
					for (int j = 0; j < tmpFold.contentFiles.Count && !search; j++) {
						//Debug.Log ("i: " + i + ", j: " + j);
						//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
						//Debug.Log("i: " + tmp [i]);
				//		Debug.Log("'"+tmpFold.contentFiles [j].name.Trim() +"' : '"+ tmp [i].Trim() +"'");
						if (tmpFold.contentFiles [j].name.Trim() == tmp [i].Trim()) {
							search = true;
							i++;
							break;
						}
					}
				}
				if (!search)
					return false;
			}
		}

		//Debug.Log ("FOUND");
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
						tmpFold = tmpFold.getParent ();
					}
					i++;
				} else {
					for (int j = 0; j < tmpFold.contentFolders.Count; j++) {
						if (tmpFold.contentFolders [j].name.Trim() == tmp [i].Trim()) {
							search = true;
							curFolder = tmpFold.contentFolders [j];
							tmpFold = tmpFold.contentFolders [j];
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
		//Debug.Log ("FOUND");
		return true;
	}


	public string getTime(string p){
		string t = "TIME";


		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		while (i<tmp.Length-1) {
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
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
			}
		}

		for (int j = 0; j < tmpFold.contentFiles.Count; j++) {
			if (tmpFold.contentFiles [j].name == tmp [i]) {
				t = tmpFold.contentFiles [j].time;
			}
		}

		return t;
	}

	public void setTime(string p, string t){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		while (i<tmp.Length-1) {
			bool search = false;
			if (tmp [i] == "..") {
				//Debug.Log ("Found ..");
				//if (tmpFold != root) {
				tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
			}
		}

		for (int j = 0; j < tmpFold.contentFiles.Count; j++) {
			if (tmpFold.contentFiles [j].name == tmp [i]) {
				//Debug.Log ("Pre: " +tmpFold.contentFiles [j].time);
				tmpFold.contentFiles [j].time = t;
				//Debug.Log ("Post: " +tmpFold.contentFiles [j].time);
			}
		}
	}


	public void add(string p, bool f){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		while (i<tmp.Length-1) {
			bool search = false;
			if (tmp [i] == "..") {
				//if (tmpFold != root) {
				tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
			}
		}


		// add file or folder

		if (f) {
			// add file
			tmpFold.addFile (tmp [i], "");
		} else {
			tmpFold.addFolder (tmp [i]);
		}

	}

	/*
	public Folder getParent(string p){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		if (tmp.Length <= 1) {
			return curFolder;
		}

		while (i<tmp.Length-1) {
			if (tmp [i] == "..") {
				//if (tmpFold != root) {
				tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name == tmp [i]) {
						tmpFold =  tmpFold.contentFolders [j];
						i++;
					}
				}
			}
		}
		return tmpFold;
	}
	*/

	public Folder getParent(string p){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		while (i<tmp.Length-1) {
			bool search = false;
			if (tmp [i] == "..") {
				//Debug.Log ("Found ..");
				//if (tmpFold != root) {
				tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//					Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name.Trim() == tmp [i].Trim()) {
						//Debug.Log (tmpFold.name + ":" +j);
						//Debug.Log ("Found: " + tmpFold.contentFolders [j].name);
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
				/*
				if (i == tmp.Length-1 && f) {
					for (int j = 0; j < tmpFold.contentFiles.Count && !search; j++) {
						//Debug.Log ("i: " + i + ", j: " + j);
						//Debug.Log ("j: " + tmpFold.contentFolders [j].name);
						//Debug.Log("i: " + tmp [i]);
						//		Debug.Log("'"+tmpFold.contentFiles [j].name.Trim() +"' : '"+ tmp [i].Trim() +"'");
						if (tmpFold.contentFiles [j].name.Trim() == tmp [i].Trim()) {
							search = true;
							i++;
							break;
						}
					}
					
				}
				*/
				if (!search)
					return null;
			}
		}

		//Debug.Log ("FOUND");
		return tmpFold;

	}


	public Folder getFold(string p){
		string[] tmp = p.Split (new[] { '/' });
		Folder tmpFold;
		if (tmp [0] != "") {
			//relative directory
			tmpFold = curFolder;
		} else {
			tmpFold = root;
		}

		int i = 0;

		while (i<tmp.Length) {
			bool search = false;
			if (tmp [i] == "..") {
				//Debug.Log ("Found ..");
				//if (tmpFold != root) {
				tmpFold = tmpFold.getParent ();
				//	Debug.Log ("Wasn't root");
				//} 
				search = true;
				i++;
			} else {
				for (int j = 0; j < tmpFold.contentFolders.Count && !search; j++) {
					//Debug.Log ("i: " + i + ", j: " + j);
					//					Debug.Log ("j: " + tmpFold.contentFolders [j].name);
					//Debug.Log("i: " + tmp [i]);
					if (tmpFold.contentFolders [j].name.Trim() == tmp [i].Trim()) {
						//Debug.Log (tmpFold.name + ":" +j);
						//Debug.Log ("Found: " + tmpFold.contentFolders [j].name);
						tmpFold = tmpFold.contentFolders [j];
						search = true;
						i++;
						break;
					}
				}
				if (!search)
					return null;
			}
		}

		//Debug.Log ("FOUND");
		return tmpFold;

	}

	public bool changePerms(string p, string newPerms){
		Folder par = getParent (p);
		if (par != null) {
			string[] tmp = p.Split (new[] { '/' });
			bool found = false;
			Debug.Log (newPerms);
			for (int i = 0; i < par.contentFiles.Count && !found; i++) {
				if (par.contentFiles [i].name.Trim() == tmp [tmp.Length - 1].Trim()) {
					found = true;
					List<bool> tmpPer = new List<bool>();
					for(int j=0; j<3; j++){
						if (newPerms [j] == 't')tmpPer.Add( true);
						else if(newPerms[j] == 'f')tmpPer.Add(false);
						else tmpPer.Add(par.contentFiles [j].ownerPermissions[j]);
					}
					par.contentFiles [i].ownerPermissions = tmpPer.ToArray();
					tmpPer.Clear ();
					for(int j=0; j<3; j++){
						if (newPerms [j+3] == 't')tmpPer.Add(true);
						else if(newPerms[j+3] == 'f')tmpPer.Add(false);
						else tmpPer.Add(par.contentFiles [j].ownerPermissions[j]);
					}
					par.contentFiles [i].groupPermissions = tmpPer.ToArray();
					tmpPer.Clear ();
					for(int j=0; j<3; j++){
						if (newPerms [j+6] == 't')tmpPer.Add(true);
						else if(newPerms[j+6] == 'f')tmpPer.Add(false);
						else tmpPer.Add(par.contentFiles [j].ownerPermissions[j]);
					}
					par.contentFiles [i].globalPermissions = tmpPer.ToArray();
					tmpPer.Clear ();
				}
			}
			if (!found) {
				for (int i = 0; i < par.contentFolders.Count && !found; i++) {
					if (par.contentFolders [i].name == tmp [tmp.Length - 1]) {
						found = true;
						bool[] tmpPer = new bool[]{ false, false, false };
						for(int j=0; j<3; j++){
							if (newPerms [j] == 't')tmpPer [j] = true;
							else if(newPerms[j] == 'f')tmpPer[j] = false;
							else tmpPer[j] =par.contentFolders [j].ownerPermissions[j];
						}
						par.contentFolders [i].ownerPermissions = tmpPer;
						for(int j=0; j<3; j++){
							if (newPerms [j+3] == 't')tmpPer [j] = true;
							else if(newPerms[j+3] == 'f')tmpPer[j] = false;
							else tmpPer[j] =par.contentFolders [j].ownerPermissions[j];
						}
						par.contentFolders [i].groupPermissions = tmpPer;
						for(int j=0; j<3; j++){
							if (newPerms [j+6] == 't')tmpPer [j] = true;
							else if(newPerms[j+6] == 'f')tmpPer[j] = false;
							else tmpPer[j] =par.contentFolders [j].ownerPermissions[j];
						}
						par.contentFolders [i].globalPermissions = tmpPer;
					}
				}
			}
			if (found) {
				return true;
			}
		}
		return false;
	}

	public string parsePerms(string s){
		string ret = "";

		if (System.Char.IsNumber (s [0])) {
			for (int i = 0; i<s.Length && i < 3; i++) {
				if (s [i] == '7') {
					ret += "ttt";
				} else if (s [i] == '6') {
					ret +="ttf";
				} else if (s [i] == '5') {
					ret += "tft";
				} else if (s [i] == '4') {
					ret += "tff";
				} else if (s [i] == '3') {
					ret += "ftt";
				} else if (s [i] == '2') {
					ret += "ftf";
				} else if (s [i] == '1') {
					ret += "fft";
				} else if (s [i] == '0') {
					ret += "fff";
				}
			}
			for (int i = s.Length; i < 3; i++) {
				ret += "fff";
			}
		} else {
			string[] tmp = s.Split (',');
			ret = "000000000";
			for (int i = 0; i < tmp.Length; i++) {
				bool u = false;
				bool g = false;
				bool o = false;

				string tmpstr = "000";

				if (tmp [i].Contains ("u")) {
					u = true;
				}
				if (tmp [i].Contains ("g")) {
					g = true;
				}
				if (tmp [i].Contains ("o")) {
					o = true;
				}

				if (tmp [i].Contains ("-")) {
					if (tmp [i].Contains ("r")) {
						tmpstr = "f" + tmpstr[1] + tmpstr[2];
					}
					if (tmp [i].Contains ("w")) {
						tmpstr = tmpstr[1] + "f" + tmpstr[2];
					}
					if (tmp [i].Contains ("x")) {
						tmpstr = tmpstr[1] + tmpstr[2] + "f";
					}
				} else if(tmp[i].Contains("+")){
					if (tmp [i].Contains ("r")) {
						tmpstr = "t" + tmpstr[1] + tmpstr[2];
					}
					if (tmp [i].Contains ("w")) {
						tmpstr = tmpstr[1] + "t" + tmpstr[2];
					}
					if (tmp [i].Contains ("x")) {
						tmpstr = tmpstr[1] + tmpstr[2] + "t";
					}
				} else if(tmp[i].Contains("=")){
					if (tmp [i].Contains ("r")) {
						tmpstr = "t" + tmpstr[1] + tmpstr[2];
					}
					if (tmp [i].Contains ("w")) {
						tmpstr = tmpstr[1] + "t" + tmpstr[2];
					}
					if (tmp [i].Contains ("x")) {
						tmpstr = tmpstr[1] + tmpstr[2] + "t";
					}
				} 

				if (u) {
					ret = tmpstr + ret [3] + ret [4] + ret [5] + ret [6] + ret [7] + ret [8];
				}
				if (g) {
					ret = ret[0] + ret[1] + ret[2] + tmpstr + ret[6] + ret[7] + ret[8];
				}
				if (o) {
					ret = ret[0] + ret[1] + ret[2] + ret[3] + ret[4] + ret[5] + tmpstr;
				}

			}

		}

		return ret;
	}



	public bool rem(string p, bool f, bool ce){
		string[] tmp = p.Split (new[] { '/' });

		Folder fol;
		if (tmp.Length == 1 || p == "" || p[0] == '/') {
			fol = root;
		} else {
			fol = getParent (p);
		}

		if (f) {
			// remove file
			File fl = new File("","","");
			for (int i = 0; i < fol.contentFiles.Count; i++) {
				if (fol.contentFiles [i].name == tmp [tmp.Length - 1]) {
					fl = fol.contentFiles [i];
				}
			}
			fol.contentFiles.Remove (fl);
		} else {
			// remove folder

		//	Debug.Log (fol.name);
			for (int i = 0; i < tmp.Length; i++) {
				for (int j = 0; j < fol.contentFolders.Count; j++) {
					if (tmp [i] == fol.contentFolders [j].name) {
						fol = fol.contentFolders [j];
						break;
					}
				}
			}

			if (ce) {
			//	Debug.Log (fol.name + " : " + p);
				// cares if there are content
				if (fol.contentFiles.Count == 0 && fol.contentFolders.Count == 0) {
					// remove folder fol
					fol.parent.contentFolders.Remove(fol);
				}
			} else {
				// remove folder regardless of contents
				fol.parent.contentFolders.Remove(fol);

			}
		}

		/*
		if (rp) {
			// recursivly go up untill empty or root
			string pt = "";
			// get parent
			for (int i = 0; i < tmp.Length-1; i++) {
				pt += tmp [i] + "/";
				if(i == tmp.Length-1)
					pt = pt.Substring (0, pt.Length - 1);
			}
			if(pt != "/" && pt != ""){ 
				Debug.LogError ("Recursive: '" + pt + "'");
				return rem(pt,false,ce,rp);
			}else{
				return true;
			}
		}
//		*/


		return false;
	}


	public string getContent(string p){
		string ret = "";
		string[] tmp = p.Split (new[] { '/' });
		Folder par = getParent (p);

		for (int i = 0; i < par.contentFiles.Count; i++) {
			if (par.contentFiles [i].name.Trim () == tmp [tmp.Length - 1].Trim ()) {
				ret = par.contentFiles [i].content;
			}
		}

		return ret;
	}

	public bool moveF(string sc, string p){
		Folder tmp = getParent (sc);
		Folder path = getFold (p);

		File toMoveil = null;
		Folder toMoveol = null;
		string[] t = sc.Split ('/');
		for (int i = 0; i < tmp.contentFiles.Count; i++) {
			if (t [t.Length - 1].Trim () == tmp.contentFiles [i].name.Trim ()) {
				toMoveil = tmp.contentFiles [i];
			}
		}
		if (toMoveil == null) {
			for (int i = 0; i < tmp.contentFolders.Count; i++) {
				if (t [t.Length - 1].Trim () == tmp.contentFolders [i].name.Trim ()) {
					toMoveol = tmp.contentFolders [i];
				}
			}
			path.contentFolders.Add (toMoveol);

			tmp.contentFolders.Remove (toMoveol);

		} else {
			path.contentFiles.Add (toMoveil);
			Debug.Log (path.contentFiles [path.contentFiles.Count - 1].name);
			tmp.contentFiles.Remove (toMoveil);

		}

		return false;
	}
}
