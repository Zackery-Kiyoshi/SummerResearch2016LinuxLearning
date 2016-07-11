using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*

pwd
cd
ls

*/



public class TerminalControl : MonoBehaviour {
	private int testing;

	private bool active;

	private List<Command> cmdHistory = new List<Command> ();
//	private Command curCommand;
//	private int curCmdNum = -1;
	private string terminal = "";
	private string curLine = "";
	private string curParam = "";

    private List<Command> cmds = new List<Command>();
    private int progress = 0;
    private string error = "";

	public string username = "IDontKnowMakeOne";
	public string comp = "DontCare01";
	public string path = "/";

	public FileSystem fileSystem; 

	private Text terminalObj;

	private int maxLines;
	private int charPerLine;
	private int numLines;
	private float width;
	private float curWidth;
	private Font myFont;
	private float preWidth;
	private Text body;

	private int index = 0;
	private Command tmpCurCmd = new Command ();

	private int tCount = 0;

	public MessageControl mc;
	public sshController sshc;

	// Use this for initialization
	void Start () {

		testing = -1;
		active = false;

		sshc = GameObject.Find ("sshController").GetComponent<sshController> ();

		//making the file system
		LevelLoader l = gameObject.transform.parent.gameObject.GetComponent<LevelLoader>();
		l.load ();
		fileSystem = l.fs;

		path = fileSystem.curFolder.name;
		// need to change this so that it will always be correct for the specific resolution
		body = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ();
		myFont = body.font;
		maxLines = (int) gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ().GetComponent<RectTransform> ().rect.height /myFont.lineHeight;
		maxLines -= 20;
		width = gameObject.transform.Find ("Text").gameObject.GetComponent<Text> ().GetComponent<RectTransform> ().rect.width;
		curWidth = 0f;
		numLines = 0;
		// initialize terminal with username
		terminalObj = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
		terminal += "[" + username + "@" + comp + " " + path + "]$ ";
		terminalObj.text = terminal;

		for (int i = 0; i < terminal.Length; i++) {
			CharacterInfo characterInfo = new CharacterInfo ();
			myFont.GetCharacterInfo (terminal [i], out characterInfo, body.fontSize);
			preWidth += characterInfo.advance;
		}


		// all commands read in from file
		TextAsset theText = Resources.Load ("Text/Commands") as TextAsset;
		string[] lines = theText.text.Split ('\n');
		List<string> tmpOpt = new List<string>();
//		/*
		for (int i = 1; i < lines.Length; i++) {
			Command newCmd = new Command ();
			// first line is the name
			//Debug.Log(lines[i]);
			newCmd.com = lines [i].Trim();
			i++;
			// second line is the possible options
			string[] opts = lines [i].Split (' ');
			for (int j = 0; j < opts.Length; j++) {
				tmpOpt.Add (opts [j]);
			}
			newCmd.options = tmpOpt;
			i++;
			// third line is the number of paramaters
			newCmd.numParams = Int32.Parse (lines [i]);
			i++;
			// fourth line should be <man> 
			i++;
			string man = "";
			bool wloop = true;
			while ( wloop ) {
				man += lines [i] + '\n';
				i++;
				if (lines [i].Length >= 5 && lines [i].Substring (0, 5) == "<man>") {
					man = man.Substring (0, man.Length - 1);
					wloop = false;
				}
			}
			newCmd.man = man;

			//Debug.Log (newCmd.com + " *" + newCmd.options + "* " + newCmd.numParams);
			//Debug.Log (i + ":" + lines.Length);
			cmds.Add (newCmd);
		}


		/*
		// original
        cmds.Add(new Command("pwd",0));
		//cmds.Add(new Command("logname",0));
        //cmds.Add(new Command("cd",0));
		tmpOpt = new List<string> ();
		tmpOpt.Add ("l");
		tmpOpt.Add ("a");
		tmpOpt.Add ("h");
		cmds.Add(new Command("ls",0, tmpOpt ));
		//cmds.Add(new Command("clear",0));
		//cmds.Add(new Command("exit",0));
		*/
    }

	// switched from FixedUpdate to fix lag issues
	void Update () {
		if (active) {
			if (Input.anyKeyDown) {

				// make command to replace current line with a certian command.line
				if(Input.GetKeyDown( KeyCode.UpArrow)){
					// cycle up the command history
					if (index > 0) {
						index--;
						// cmdHistory[index]

					}
				} else if(Input.GetKeyDown( KeyCode.DownArrow )){
					// cycle down the command history
					if (index < cmdHistory.Count - 1) {
						index++;
						// cmdHistory[index]

					} else {
						// tmpCurCmd

					}
				}else {
	                foreach (char c in Input.inputString)
    	            {
						if (c == ' ') {
							curLine = curLine + " ";
							tmpCurCmd.line = curLine;
							terminal += " ";
							tCount = 0;
						} else if (c == "\b" [0]) {
							if (curLine.Length != 0){
								curLine = curLine.Substring (0, curLine.Length - 1);
								tmpCurCmd.line = curLine;
								terminal = terminal.Substring (0, terminal.Length - 1);
								tCount = 0;
							}
						} else if (c == "\n" [0] || c == "\r" [0]) {
							tmpCurCmd = new Command ();
							tCount = 0;
							curWidth = preWidth;
							processCommand (curLine);
						} else if (c == "\t" [0]) {
							// TODO tab completion
							if (curLine.Length > 0) {
								tCount++;
								string[] tmpL = curLine.Split (new[] { ' ' });
								string s = tmpL [tmpL.Length - 1];

								List<KeyValuePair<string,string>> posFill = new List<KeyValuePair<string,string>>();
								if (tmpL.Length == 1) {
									// search throught cmds
									for (int i = 0; i < cmds.Count; i++) {
										bool match = true;
										for (int j = 0; j < s.Length && match; j++) {
											if (s [j] != cmds [i].com [j])
												match = false;
										}
										if (match) {
											posFill.Add (new KeyValuePair<string,string> (cmds [i].com, cmds [i].com.Substring (s.Length - 1)));
										}
									}
								} else {
									// search throught files/directories

								}
								if (posFill.Count > 1 && tCount == 2) {
									// print the results


								} else if (posFill.Count == 1) {
									curLine += posFill [0].Key;
									terminal += posFill [0].Value;
								}

							}
						} else {
							curLine += c;
							curParam += c;
							terminal += c;
							tmpCurCmd.line = curLine;
							tCount = 0;
							CharacterInfo characterInfo = new CharacterInfo ();
							myFont.GetCharacterInfo (c, out characterInfo, body.fontSize);
							curWidth +=  characterInfo.advance;

							if (curWidth >= width - 5) {
								updateTerminal ();

							}
							if (curWidth >= width) {
								curWidth = 0;
							}
						}
	                }
					terminalObj.text = terminal;
				}
			}
		}else{
			//Debug.Log ("NOT ACTIVE???");
		}
	}

	public void activate(){
		active = true;
	}

	public void deactivate(){
		active = false;
	}

	public void processCommand(string line){

		string[] s = line.Split (new[] { ' ' });

		Command curCommand = new Command();

		if (curLine == "") {
			curCommand.line = curLine;
			cmdHistory.Add (curCommand);
			doCommand (curCommand);
			return;
		}


		//calculate lines
		//numLines += (1 + (line.Length + 19) % charPerLine);

		// processes the string[] into command
		int tmp = -1;
		string opt = "";
		foreach(string a in s){
			if (progress == 0) {
				if(testing >= 1) Debug.Log (":" + curLine + ":");
				for (int i = 0; i < cmds.Count && (tmp < 0); i++) {
					if (cmds [i].com == a) {
						if(testing >= 1) Debug.Log ("ADDED CMD to history: " + cmds [i].com);
						tmp = i;
					}
				}
				if (tmp < 0) {
					curCommand = cmds [cmds.Count-1].clone();
					curCommand.error = true;
				} else {
					curCommand = cmds [tmp].clone();
				}
				if (tmp < 0) {
					error = "-bash: " + curLine + ": command not found (or just not supported)";
				}
				progress++;
			} else if (progress >= 1) {
				// read in options or first paramater
				if (a != "") {
					if (a [0] == '-') {
						if(cmds[tmp].options.Contains(a.Substring(1,a.Length-1))) {
							curCommand.options.Add (a.Substring(1,a.Length-1));
							opt = a.Substring(1,a.Length-1);
						} else{
							for (int i = 1; i < a.Length; i++) {
								if(testing >= 1) Debug.Log ("ADDing option: " + a [i]);
								curCommand.options.Add (a [i] + "");
								opt = a;
							}
						}
						// actuall options happen in processing
					} else {
						if(testing >= 1) Debug.Log ("ADDING A PARAM: " + a);
						if (opt != "") {
							curCommand.param.Add ("-" + opt + " " + a);
							opt = "";
						} else {
							curCommand.param.Add (a);
						}
					}
					progress++;
				}
			}
		}
		curCommand.line = curLine;
		cmdHistory.Add (curCommand);
		doCommand (curCommand);
	}

	public void doCommand(Command curCommand){
		bool nline = false;

		if (error != "") {
			// error
			terminal += "\n" + error + "\n";
		} else {
			//Command curCommand = cmdHistory [cmdHistory.Count - 1];
			// actually do command
			terminal += "\n";

			if (curCommand == null) {
				if (testing >= 0)
					Debug.Log ("curCommand is null");
			} else if (curCommand.com == "cat") {
				// TODO
				string ret = "";

				int invalid = -1;
				for (int i = 0; i < curCommand.param.Count && invalid == -1; i++) {
					if (fileSystem.checkPath (curCommand.param [i], true)) {
						ret += fileSystem.getContent (curCommand.param [i]) + "\n";
					} else {
						invalid = i;
					}
				}

				if (invalid != -1) {
					terminal += "";
				} else {
					string[] tret = ret.Split ('`', '\n');
					for (int i = 0; i < tret.Length; i++) {
						terminal += tret [i] + '\n';
					}
				}

				Debug.Log ("cat");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "cd") {
				if (testing >= 0)
					Debug.Log ("ITS PROCESSING CD");
				// check for path
				if (curCommand.param.Count < 1) {
					// return to root TODO

					if (testing >= 0)
						Debug.Log ("returning to default location");
				} else {
					if (fileSystem.checkPath (curCommand.param [0], false)) {
						fileSystem.moveTo (curCommand.param [0]);
						path = fileSystem.curFolder.name;
					} else {
						// error invalid directory
						terminal += "\n -bash: " + curLine + ": Not a directory";
					}
				}
			} else if (curCommand.com == "chmod") {
				// TODO

				bool R = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("R")) {
						R = true;
					}
				}


				string pa = "";
				string tmpPrem = "";

				for (int i = 0; i < curCommand.param.Count; i++) {
					if (curCommand.param [i] != "") {
						if (System.Char.IsNumber (curCommand.param [i] [curCommand.param [i].Length - 1])) {
							if (R) {
								tmpPrem = curCommand.param [i].Split (' ') [1];
							} else
								tmpPrem = curCommand.param [i];
						
						} else if (curCommand.param [i].Contains ("-") ||
						           curCommand.param [i].Contains ("+") ||
						           curCommand.param [i].Contains ("=")) {

							if (R) {
								tmpPrem = curCommand.param [i].Split (' ') [1];
							} else
								tmpPrem = curCommand.param [i];
						} else {
							if (curCommand.param [i].Substring (0, 3) == "--p") {
								pa = curCommand.param [i].Substring (3, curCommand.param [i].Length - 3);

							} else
								pa = curCommand.param [i];
							//Debug.Log ("param: " + pa);
						}
					}
				}

				//Debug.Log ("path: " + pa);
				//Debug.Log ("cgs: " + tmpPrem);
				if (pa != "") {
					if (fileSystem.checkPath (pa, true)) {
						if (R) {
							string perm = fileSystem.parsePerms (tmpPrem);
							fileSystem.changePerms (pa, perm);
							List<Folder> toCh = new List<Folder> { fileSystem.getFold (pa) };
							int i = 0;

							while (i < toCh.Count) {
								Folder tmpRoot = toCh [i];

								for (int j = 0; j < tmpRoot.contentFiles.Count; j++) {
									fileSystem.changePerms (pa + "/" + tmpRoot.contentFiles [j].name, perm);
								}
								for (int j = 0; j < tmpRoot.contentFolders.Count; j++) {
									fileSystem.changePerms (pa + "/" + tmpRoot.contentFolders [j].name, perm);
									toCh.Add (tmpRoot.contentFolders [j]);
								}

								i++;
							}

						} else {
							string perm = fileSystem.parsePerms (tmpPrem);
							fileSystem.changePerms (pa, perm);
						}
					} else {

						terminal += "chmod: cannot access `" + pa + "': No such file or directory";
					}
				} else {
					
					terminal += "chmod: missing operand after `" + tmpPrem + "'";
				}
				Debug.Log ("chmod");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "clear") {
				terminal = "";
				nline = true;
				numLines = 0;
				curWidth = preWidth;
				//terminal += "[" + username + "@" + comp + " " + path + "]$ ";
				//return;
				//"[" + username + "@" + comp + " " + path + "]$ ";
			} else if (curCommand.com == "df") {
				// TODO

				bool h = false;
				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("h")) {
						h = true;
					}
				}
				Debug.Log ("df");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "du") {
				// TODO


				bool h = false;
				bool S = false;
				bool s = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("h")) {
						h = true;
					} else if (curCommand.options [i].Contains ("S")) {
						S = true;
					} else if (curCommand.options [i].Contains ("s")) {
						s = true;
					}
				}

				Debug.Log ("du");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "echo") {

				bool n = false;
				bool e = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("n")) {
						n = true;
					} 
				}
				nline = n;
				string ret = "";

				for (int i = 0; i < curCommand.param.Count; i++) {
					if (n && i == 0) {
						ret += curCommand.param [i].Substring (4, curCommand.param [i].Length - 4) + " ";
					} else
						ret += curCommand.param [i] + " ";
				}
				terminal += ret;

			} else if (curCommand.com == "exit") {
				if (testing >= 1)
					Debug.Log ("Application.Quit ();");
				//Application.OpenURL ("");
				Application.Quit ();
				//	UnityEditor.EditorApplication.isPlaying = false;
			} else if (curCommand.com == "find") {
				// TODO


				bool maxdepth = false;
				bool mindepth = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("maxdepth")) {
						maxdepth = true;
					} else if (curCommand.options [i].Contains ("mindepth")) {
						mindepth = true;
					}
				}

				Debug.Log ("find");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "grep") {
				// TODO


				bool f = false;
				bool v = false;
				bool c = false;
				bool L = false;
				bool l = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("f")) {
						f = true;
					} else if (curCommand.options [i].Contains ("v")) {
						v = true;
					} else if (curCommand.options [i].Contains ("c")) {
						c = true;
					} else if (curCommand.options [i].Contains ("L")) {
						L = true;
					} else if (curCommand.options [i].Contains ("l")) {
						l = true;
					}
				}

				Debug.Log ("grep");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "gzip") {
				// TODO

				Debug.Log ("gzip");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "gunzip") {
				// TODO

				Debug.Log ("gunzip");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "head") {
				bool q = false;
				bool v = false;
				bool n = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("q")) {
						q = true;
					} else if (curCommand.options [i].Contains ("v")) {
						v = true;
					} else if (curCommand.options [i].Contains ("n")) {
						n = true;
					}
				}

				int lines = -1;
				string ret = "";
				string pa = "";

				for (int i = 0; i < curCommand.param.Count; i++) {
					Debug.Log (i + ": '" + curCommand.param [i] + "'");
					if (fileSystem.checkPath (curCommand.param [i], true)) {
						ret = fileSystem.getContent (curCommand.param [i]);
						pa = curCommand.param [i];
						Debug.Log ("none: " + pa);
					} else if (n && lines == -1 && curCommand.param [i].Contains ("-n")) {
						lines = Int32.Parse (curCommand.param [i].Substring (4, curCommand.param [i].Length - 4));
					} else if (v && curCommand.param [i].Contains ("-v")) {
						Debug.Log ("Here");
						pa = curCommand.param [i].Substring (3, curCommand.param [i].Length - 3);
						Debug.Log ("Here!!!!!!!!!!!");
						if (fileSystem.checkPath (pa, true)) {
							ret = fileSystem.getContent (pa);
						}
						Debug.Log ("v: " + pa);
					} else {
						pa = curCommand.param [i];
					}

				}
				if (lines == -1) {
					lines = 10;
				}
				if (!fileSystem.checkPath (pa, true)) {
					terminal += "head: cannot open `" + pa + "' for reading: No such file or directory";
				} else {

					if (v) {
						terminal += "==> " + pa.Split ('/') [pa.Split ('/').Length - 1] + " <== \n";
					}

					string[] tret = ret.Split ('`');
					if (lines < tret.Length) {
						for (int i = 0; i < lines; i++) {
							terminal += tret [i] + '\n';
						}
					} else {
						for (int i = 0; i < tret.Length; i++) {
							terminal += tret [i] + '\n';
						}
					}
				}

			} else if (curCommand.com == "less") {
				// TODO

				Debug.Log ("less");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "logname") {
				terminal += username;
			} else if (curCommand.com == "ls") {

				bool a = false;
				bool l = false;
				bool h = false;
				bool o1 = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					//Debug.Log (i + ": " + curCommand.options.Count );
					if (curCommand.options [i].Contains ("a")) {
						//Debug.Log ("Found a");
						a = true;
					} else if (curCommand.options [i].Contains ("l")) {
						//Debug.Log ("Found l");
						l = true;
					} else if (curCommand.options [i].Contains ("h")) {
						//Debug.Log ("Found h");
						h = true;
					} else if (curCommand.options [i].Contains ("1")) {
						o1 = true;
					}
				}

				if (a) {
					if (l) {
						terminal += fileSystem.curFolder.printPermissions () +
						" " + fileSystem.curFolder.owner +
						" " + fileSystem.curFolder.group +
						" " + 4096 +
						" " + fileSystem.curFolder.time +
						" <color=blue>" + "." + "</color>\n";

						terminal += fileSystem.curFolder.parent.printPermissions () +
						" " + fileSystem.curFolder.parent.owner +
						" " + fileSystem.curFolder.parent.owner +
						" " + 4096 +
						" " + "Jun 8 11:27" +
						" <color=blue>" + ".." + "</color>\n";
					} else {
						if (o1) {
							terminal += "<color=blue> . </color> \n";
							terminal += "<color=blue> .. </color> \n";
						} else {
							terminal += "<color=blue> . </color>   ";
							terminal += "<color=blue> .. </color>   ";
						}
					}
				}

				// if no options
				for (int i = 0; i < fileSystem.curFolder.contentFolders.Count; i++) {
					//Debug.Log (i + ": " + fileSystem.curFolder.contentFolders [i].name);
					if (fileSystem.curFolder.contentFolders [i].hidden && a) {
						if (l) {
							terminal += fileSystem.curFolder.contentFolders [i].printPermissions () +
							" " + fileSystem.curFolder.contentFolders [i].owner +
							" " + fileSystem.curFolder.contentFolders [i].group +
							" " + fileSystem.curFolder.contentFolders [i].printSize (h) +
							" " + fileSystem.curFolder.contentFolders [i].time +
							" <color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color>\n";
						} else {
							if (o1) {
								terminal += "<color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color> \n";
							} else {
								terminal += "<color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color>   ";
							}
						}
					} else if (!fileSystem.curFolder.contentFolders [i].hidden) {
						if (l) {
							terminal += fileSystem.curFolder.contentFolders [i].printPermissions () +
							" " + fileSystem.curFolder.contentFolders [i].owner +
							" " + fileSystem.curFolder.contentFolders [i].group +
							" " + fileSystem.curFolder.contentFolders [i].printSize (h) +
							" " + fileSystem.curFolder.contentFolders [i].time +
							" <color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color>\n";
						} else {
							if (o1) {
								terminal += "<color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color> \n";
							} else {
								terminal += "<color=blue>" + fileSystem.curFolder.contentFolders [i].name + "</color>   ";
							}
						}
					}
				}
				for (int i = 0; i < fileSystem.curFolder.contentFiles.Count; i++) {
					if (fileSystem.curFolder.contentFiles [i].hidden && a) {
						if (l) {
							terminal += fileSystem.curFolder.contentFiles [i].printPermissions () +
							" " + fileSystem.curFolder.contentFiles [i].owner +
							" " + fileSystem.curFolder.contentFiles [i].group +
							" " + fileSystem.curFolder.contentFiles [i].printSize (h) +
							" " + fileSystem.curFolder.contentFiles [i].time +
							" " + fileSystem.curFolder.contentFiles [i].name + "\n";
						} else {
							if (o1) {
								terminal += fileSystem.curFolder.contentFiles [i].name + " \n";
							} else {
								terminal += fileSystem.curFolder.contentFiles [i].name + "   ";
							}
						}
					} else if (!fileSystem.curFolder.contentFiles [i].hidden) {
						if (l) {
							terminal += fileSystem.curFolder.contentFiles [i].printPermissions () +
							" " + fileSystem.curFolder.contentFiles [i].owner +
							" " + fileSystem.curFolder.contentFiles [i].group +
							" " + fileSystem.curFolder.contentFiles [i].printSize (h) +
							" " + fileSystem.curFolder.contentFiles [i].time +
							" " + fileSystem.curFolder.contentFiles [i].name + "\n";
						} else {
							if (o1) {
								terminal += fileSystem.curFolder.contentFiles [i].name + " \n";
							} else {
								terminal += fileSystem.curFolder.contentFiles [i].name + "   ";
							}
						}
					}
				}

			} else if (curCommand.com == "man") {
				if (curCommand.param.Count > 0) {
					Debug.Log ("man: " + curCommand.param [0]);
					terminal += curCommand.man;
				} else {
					terminal += "\n" + "-bash: man : Invalid paramater to man (or not a command supported)" + "\n";
				}
			} else if (curCommand.com == "mkdir") {
				bool m = false;
				bool p = false;
				string perm;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("m")) {
						m = true;
					} else if (curCommand.options [i].Contains ("p")) {
						p = true;
					}
				}

				if (curCommand.param.Count < 0) {
					// error

				} else {
					// need to extract paramaters
					string pa = curCommand.param [0];
					int pindex = 0;

					for (int i = 0; i < curCommand.param.Count; i++) {
						if ((curCommand.param [i].Contains ("-") ||
						    curCommand.param [i].Contains ("+") ||
						    curCommand.param [i].Contains ("=")) &&
						    !curCommand.param [i].Contains ("p")) {
							pindex = i;
						} else {
							if (curCommand.param [i].Length > 3 && curCommand.param [i].Substring (0, 3) == "--p") {
								pa = curCommand.param [i].Substring (3, curCommand.param [i].Length - 3);

							} else
								pa = curCommand.param [i];
							//Debug.Log ("param: " + pa);
						}
					}

					string tmpP = "";

					string[] t = pa.Split ('/');
					int depth = t.Length;
					if (t.Length > 1) {
						for (int i = 0; i < t.Length - 1; i++) {
							tmpP += t [i] + "/";
						}
						tmpP = tmpP.Substring (0, tmpP.Length - 1);

						if (tmpP == "" || fileSystem.checkPath (tmpP.Trim (), false)) {
							// make the new folder;
							fileSystem.add (pa, false);
							if (m) {
								// change the permissions

								perm = fileSystem.parsePerms (curCommand.param [pindex]);
								fileSystem.changePerms (pa, perm);
							}
						} else if (p) {
							Debug.Log ("P");
							// make missing directories
							string p2 = "";
							string use = "";
							for (int i = 0; i < t.Length; i++) {
								p2 += t [i] + "/";
								use = p2.Substring (0, p2.Length - 1);
								if (!fileSystem.checkPath (use, false)) {
									fileSystem.add (use, false);
									if (m) {
										// change the permissions
										perm = fileSystem.parsePerms (curCommand.param [pindex]);
										fileSystem.changePerms (use.Trim (), perm);
									}
								}
							}
						} else {
							Debug.Log ("invalid path");

						}

					} else {
						tmpP = pa;
						Debug.Log ("else " + t.Length + ":" + pa);
						fileSystem.add (pa.Trim (), false);
						if (m) {
							// change the permissions

							perm = fileSystem.parsePerms (curCommand.param [pindex]);
							fileSystem.changePerms (pa, perm);
						}
					}

				}

			} else if (curCommand.com == "mv") {
				// TODO
				bool u = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("u")) {
						u = true;
					} 
				}

				string pa = "";
				string toMv = "";

				for (int i = 0; i < curCommand.param.Count; i++) {
					if (curCommand.param [i].Contains ("-u") || curCommand.param [i].Contains ("-f")) {
						if (toMv == "")
							toMv = curCommand.param [i].Split (' ') [1];
						else
							pa = curCommand.param [i].Split (' ') [1];
					} else {
						if (toMv == "")
							toMv = curCommand.param [i];
						else
							pa = curCommand.param [i];
					}
				}
				if (fileSystem.checkPath (toMv, true) || fileSystem.checkPath (toMv, false)) {
					if (fileSystem.checkPath (pa, false)) {
						string[] tmp = toMv.Split ('/');
						bool mvfl = false;
						Folder par = fileSystem.getParent (toMv);
						File m = null;
						for (int i = 0; i < par.contentFiles.Count; i++) {
							if (par.contentFiles [i].name.Trim () == tmp [tmp.Length - 1].Trim ()) {
								mvfl = true;
								m = par.contentFiles [i];
							}
						}
						Folder mF = null;
						for (int i = 0; i < par.contentFolders.Count; i++) {
							if (par.contentFolders [i].name.Trim () == tmp [tmp.Length - 1].Trim ()) {
								mvfl = false;
								mF = par.contentFolders [i];
							}
						}

						Folder fl = fileSystem.getFold (pa);
						File fil = null;

						for (int i = 0; i < fl.contentFiles.Count; i++) {
							if (fl.contentFiles [i].name.Trim () == tmp [tmp.Length - 1].Trim ()) {
								fil = fl.contentFiles [i];
							}
						}
						if (fil != null) {
							if (!mvfl) {
								terminal += "mv: cannot overwrite non-directory `" + pa + "/" + tmp [tmp.Length - 1].Trim () + "' with directory `" + toMv + "'";
							} else if (u) {
								// only replace if newer
								if (compTime (fil.time, m.time)) {
									fileSystem.moveF (toMv, pa);
								} 
							} else {
								fileSystem.moveF (toMv, pa);
							}
						}
						Folder fol = null;
						for (int i = 0; i < fl.contentFolders.Count; i++) {
							if (fl.contentFolders [i].name.Trim () == tmp [tmp.Length - 1].Trim ()) {
								fol = fl.contentFolders [i];
							}
						}

						if (fol != null) {
							if (mvfl) {
								terminal += "mv: cannot overwrite non-directory `" + pa + "/" + tmp [tmp.Length - 1].Trim () + "' with directory `" + toMv + "'";
							} else if (u) {
								// only replace if newer
								if (compTime (fil.time, mF.time)) {
									fileSystem.moveF (toMv, pa);
								} 
							} else {
								fileSystem.moveF (toMv, pa);
							}
						} else {
							// there is no file or folder with the same name
							fileSystem.moveF (toMv, pa);
						}

					} else {
						// invalid pa
						if (pa.Split (' ').Length > 1) {
							terminal += "mv: cannot stat`" + pa + "' No such file or directory";
						} else {
							// rename

						}

					}
				} else {
					// invalid toMv
					terminal += "mv: cannot stat`" + toMv + "' No such file or directory";
				}
				Debug.Log ("mv");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "pwd") {
				terminal += fileSystem.curFolder.path;
			} else if (curCommand.com == "quota") {
				// TODO

				terminal += "Disk quotas for user " + username + "(uid 31416):" + '\n';
				terminal += '\t' + "Filesystem" + '\t' + "blocks" + '\t' + "quota" + '\t' + "limit" + '\t' + "grace" + '\t' + "files" + '\t' + '\n';
				terminal += '\t' + "F" + '\t' + "b" + '\t' + "q" + '\t' + "l" + '\t' + "g" + '\t' + "f" + '\n';

				Debug.Log ("quota");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "rm") {
				// TODO


				bool r = false;
				bool f = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("r")) {
						r = true;
					} else if (curCommand.options [i].Contains ("f")) {
						f = true;
					}
				}

				string pa = curCommand.param [0];
				if (r && f) {
					pa = curCommand.param [0].Substring (5);
				} else if (curCommand.param [0].Contains ("-r") || curCommand.param [0].Contains ("-f")) {
					pa = curCommand.param [0].Substring (3);
				}

				if (f) {
					fileSystem.rem (pa, true, f);
					fileSystem.rem (pa, false, f);
				} else if (r) {

					if (f) {
						fileSystem.rem (pa, true, false);
						fileSystem.rem (pa, false, false);
					} else {

						List<Folder> toCh = new List<Folder> { fileSystem.getFold (pa) };
						int i = 0;

						while (i < toCh.Count) {
							Debug.Log (i + ", " + toCh.Count);
							Folder tmpRoot = toCh [i];

							for (int j = 0; j < tmpRoot.contentFiles.Count; j++) {
								fileSystem.rem (tmpRoot.contentFiles [j].path, true, f);
								Debug.Log (tmpRoot.contentFiles [j].path);
							}
							for (int j = 0; j < tmpRoot.contentFolders.Count; j++) {
								toCh.Add (tmpRoot.contentFolders [j]);
							}
							i++;
						}

						for (int j = i - 1; j > 0; j--) {
							Debug.Log ("remove: " + toCh [j].name);
							fileSystem.rem (toCh [j].path, false, f);
						}
					}
				} else {
					fileSystem.rem (pa, true, true);
				}

				Debug.Log ("rm");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "rmdir") {

				bool p = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("p")) {
						p = true;
					} 
				}

				string pa = curCommand.param [0];
				if (curCommand.param [0].Contains ("-p")) {
					pa = curCommand.param [0].Substring (4);
				}

				if (fileSystem.checkPath (pa, false)) {
					if (p) {
//						/*
						// remove empty parent directories
						Folder ck = fileSystem.getParent (pa);
						fileSystem.rem (pa, false, true);
						string[] tmp = pa.Split ('/');
						int i = tmp.Length - 1;


						while (ck != null) {
							Folder tmpck = ck.parent;
							Debug.Log ("ckecking: " + ck.name);
							if (ck.name != fileSystem.root.name && ck.contentFiles.Count == 0 && ck.contentFolders.Count == 0) {
								// remove
								string t = "";
								for (int k = 0; k < i; k++) {
									t += tmp [k] + "/";
								}
								t = t.Substring (0, t.Length - 1);
								Debug.Log (ck.path + ":" + t);
								fileSystem.rem (t, false, true);
								ck = tmpck;
							} else {
								ck = null;
							}

						}
//						*/
					} else {
						fileSystem.rem (pa, false, true);
					}
				} else {
					// bad path
					terminal += "rmdir: failed to remove `" + pa + "': No such file or directory\n";
				}
			} else if (curCommand.com == "ssh") {
				// TODO
				string scene = "";
				string port = "";
				string[] param = new string[0];

				bool p = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if(curCommand.options[i].Contains("p")){
						p = true;
					}
				}

				// get the paramater
				Debug.Log("Test");
				for (int i = 0; i < curCommand.param.Count; i++) {
					Debug.Log(i + ": " + curCommand.param [i] );
					if (p && curCommand.param [i].Substring (0, 3) == "--p") {
						port = curCommand.param [i].Substring (4);
						Debug.Log (port);
					} else if(curCommand.param[i] != "") {
						param = curCommand.param [i].Split('@');
					}
					//Debug.Log (curCommand.param [i]);
				}

				string user = "";
				string c = "";

				if (param.Length > 1) {
					// there is a username
					for (int i = 0; i < param.Length; i++) {
						Debug.Log (i + ": " + param [i]);
					}
					user = param[0];
					c = param [1];
				} else {
					c = param [0];
				}

				//Debug.Log ("u: '" + user + "'");
				//Debug.Log ("c: '" + c + "'");

				bool ssh = false;
				for (int i = 0; i < sshc.curLevels.Count; i++) {
					if (p) {
						if (c == sshc.curLevels [i].comp && user == sshc.curLevels[i].username && port == sshc.curLevels[i].port) {
							//Debug.Log ("FOUND: " + c);
							scene = sshc.curLevels [i].levelN;
							ssh = true;
						}
						Debug.Log ( "-p " + sshc.curLevels[i].port + " " + sshc.curLevels [i].username + "@" + sshc.curLevels [i].comp );
						Debug.Log ( "-p " + port + " " + user + "@" + c );
					} else {
						// don't check port
						if (c == sshc.curLevels [i].comp && user == sshc.curLevels[i].username) {
							//Debug.Log ("FOUND: " + c);
							scene = sshc.curLevels [i].levelN;
							ssh = true;
						}
					}

				}


				if (ssh && scene != "") {
					SceneManager.LoadScene (scene);
				} else {
					//Debug.Log ("NO Change");
				}



				Debug.Log ("ssh");
				Debug.Log ("Not Functioning yet");
			}else if (curCommand.com == "sshopt") {

				if (sshc.curLevel < sshc.numTutLevels) {
					// still in tutorial
					//need to check if done with current level

					terminal += sshc.curLevels[sshc.curLevel].port + '\t' + sshc.curLevels [sshc.curLevel].username + '\t' + sshc.curLevels [sshc.curLevel].comp;
				} else {
					// out of tutorial level

					if (SceneManager.GetActiveScene().name == sshc.hubName) {
						// if in main menu (show all possible options)
						for (int i = sshc.hubNum; i < sshc.curLevels.Count; i++) {
							terminal += sshc.curLevels[i].port + '\t' + sshc.curLevels [i].username + '\t' + sshc.curLevels [i].comp;
							if (i != sshc.curLevels.Count - 1) {
								terminal += '\n';
							}
						}
					} else {
						// check if in tutorial
						if (sshc.curLevel < sshc.hubNum) {
							Debug.Log ("in tutorial");
							if (mc.f) {
								terminal += sshc.allLevels [sshc.curLevel-1].port + '\t' + sshc.allLevels [sshc.curLevel-1].username + '\t' + sshc.allLevels [sshc.curLevel-1].comp;
							}
						} else {
							// if not show only hub
							Debug.Log (sshc.curLevel + ":" +sshc.hubNum + " : " + sshc.curLevels.Count);
							terminal += sshc.allLevels [sshc.hubNum].port + '\t' + sshc.allLevels [sshc.hubNum].username + '\t' + sshc.allLevels [sshc.hubNum].comp;
							// if done with level show the next level
							if (mc.f) {
								terminal += '\n' + sshc.allLevels [sshc.curLevel].port + '\t' + sshc.allLevels [sshc.curLevel].username + '\t' + sshc.allLevels [sshc.curLevel].comp;
							}
						}
					}
				}
				Debug.Log ("sshopt");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "tail") {

				bool q = false;
				bool v = false;
				bool n = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("q")) {
						q = true;
					} else if (curCommand.options [i].Contains ("v")) {
						v = true;
					} else if (curCommand.options [i].Contains ("n")) {
						n = true;
					}
				}

				int lines = -1;
				string ret = "";
				string pa = "";

				int invalid = -1;
				for (int i = 0; i < curCommand.param.Count && invalid == -1; i++) {
					Debug.Log (i + ": '" + curCommand.param [i] + "'");
					if (fileSystem.checkPath (curCommand.param [i], true)) {
						ret = fileSystem.getContent (curCommand.param [i]);
						pa = curCommand.param [i];
						Debug.Log ("none: " + pa);
					} else if (n && lines == -1 && curCommand.param [i].Contains ("-n")) {
						lines = Int32.Parse (curCommand.param [i].Substring (4, curCommand.param [i].Length - 4));
					} else if (v && curCommand.param [i].Contains ("-v")) {
						Debug.Log ("Here");
						pa = curCommand.param [i].Substring (3, curCommand.param [i].Length - 3);
						Debug.Log ("Here!!!!!!!!!!!");
						if (fileSystem.checkPath (pa, true)) {
							ret = fileSystem.getContent (pa);
						}
						Debug.Log ("v: " + pa);
					} else {
						pa = curCommand.param [i];
					}

				}
				if (lines == -1) {
					lines = 10;
				}
				if (!fileSystem.checkPath (pa, true)) {
					terminal += "tail: cannot open `" + pa +"' for reading: No such file or directory";
				} else{
					if (v) {
						terminal += "==> " + pa.Split('/')[pa.Split('/').Length-1]  +" <== \n";
					}
					string[] tret = ret.Split ('`');
					if (lines < tret.Length) {
						for (int i = tret.Length-1 - lines; i < tret.Length; i++) {
							terminal += tret [i] + '\n';
						}
					} else {
						for (int i = 0; i < tret.Length; i++) {
							terminal += tret [i] + '\n';
						}
					}
				}

			} else if (curCommand.com == "tar") {
				// TODO


				bool c = false;
				bool x = false;
				bool z = false;
				bool f = false;
				bool v = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("c")) {
						c = true;
					} else if (curCommand.options [i].Contains ("x")) {
						x = true;
					} else if (curCommand.options [i].Contains ("z")) {
						z = true;
					} else if (curCommand.options [i].Contains ("f")) {
						f = true;
					} else if (curCommand.options [i].Contains ("v")) {
						v = true;
					}
				}

				Debug.Log ("tar");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "touch") {
				bool r = false;
				bool t = false;
				string fl = "";
				string time = "";
				string sfl = "";

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i] == ("r")) {
						r = true;
						// 
						for (int j = 0; j < curCommand.param.Count; j++) {
							if (curCommand.param [j].Length > 2 && curCommand.param [j].Substring (0, 2) == "-r") {
								sfl = curCommand.param [j].Split (' ') [1];
								break;
							}
						}
						Debug.Log ("found r ");
					} else if (curCommand.options [i] == ("t")) {
						t = true;
						for (int j = 0; j < curCommand.param.Count; j++) {
							if (curCommand.param [j].Length > 2 && curCommand.param [j].Substring (0, 2) == "-r") {
								sfl = curCommand.param [j].Split (' ') [1];
								break;
							}
						}
						Debug.Log ("found t ");
					}
				}

				for (int i = 0; i < curCommand.param.Count; i++) {
					if (curCommand.param [i] [0] != '-') {
						if (fl == "") {
							fl = curCommand.param [i];
						}
					}
				}

				if (r) {
					if (fileSystem.checkPath (sfl,true)) {
						time = fileSystem.getTime (sfl);
					} else {
						terminal += "touch: failed to get attributes of `" + sfl +"': No such file or directory";
					}
				} else if (t) {
					int month = Int32.Parse(sfl.Substring(4,2)); 
					if (month == 1) {
						time += "Jan";
					} else if (month == 2) {
						time += "Feb";
					} else if (month == 3) {
						time += "Mar";
					} else if (month == 4) {
						time += "Apr";
					} else if (month == 5) {
						time += "May";
					} else if (month == 6) {
						time += "Jun";
					} else if (month == 7) {
						time += "Jul";
					} else if (month == 8) {
						time += "Aug";
					} else if (month == 9) {
						time += "Sept";
					} else if (month == 10) {
						time += "Oct";
					} else if (month == 11) {
						time += "Nov";
					} else if (month == 12) {
						time += "Dec";
					}

					time += " " + sfl.Substring(6,2);
				} else {
					int month = System.DateTime.Now.Month; 
					if (month == 1) {
						time += "Jan";
					} else if (month == 2) {
						time += "Feb";
					} else if (month == 3) {
						time += "Mar";
					} else if (month == 4) {
						time += "Apr";
					} else if (month == 5) {
						time += "May";
					} else if (month == 6) {
						time += "Jun";
					} else if (month == 7) {
						time += "Jul";
					} else if (month == 8) {
						time += "Aug";
					} else if (month == 9) {
						time += "Sept";
					} else if (month == 10) {
						time += "Oct";
					} else if (month == 11) {
						time += "Nov";
					} else if (month == 12) {
						time += "Dec";
					}

					time += " " + System.DateTime.Now.Day;
					time += " " + System.DateTime.Now.TimeOfDay.Hours + ":";
						
					if (System.DateTime.Now.TimeOfDay.Minutes < 10) {
						time += "0"+System.DateTime.Now.TimeOfDay.Minutes;
					} else {
						time += System.DateTime.Now.TimeOfDay.Minutes;
					}
				}

				if (fileSystem.checkPath (fl, true)) {
					fileSystem.setTime (fl, time);
				} else {
					// create file

					string[] tmpsp = fl.Split ('/');
					if (tmpsp.Length < 2) {
						string tmppar = "";
						for (int i = 0; i < tmpsp.Length - 2; i++) {
							tmppar += tmpsp [i] + "/";
						}
						tmppar = tmppar.Substring (0, tmppar.Length - 1);
						if (fileSystem.checkPath (tmppar, false)) {
							Debug.Log ("Adding: " + fl);
							fileSystem.add (fl, true);
							fileSystem.setTime (fl, time);
						} else {
							Debug.Log ("Invalid path");
						}
					} else {
						fileSystem.add (fl, true);
						fileSystem.setTime (fl, time);
					}
					//Debug.Log("File doesn't exist");
				}
				//Debug.Log ("time to set: " + time);
				//Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "zip") {
				// TODO

				Debug.Log ("zip");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "unzip") {
				// TODO

				Debug.Log ("unzip");
				Debug.Log ("Not Functioning yet");
			} 

			if(testing >= 1) Debug.Log("Doing Command");
		}
		curLine = "";
		curParam = "";
		error = "";
		progress = 0;
		if(!nline) terminal += "\n";
		terminal += "[" + username + "@" + comp + " " + path + "]$ ";
		numLines += 1;
		updateTerminal ();
		mc.processCmd (curCommand);
	}



	public void updateTerminal(){
		// when scrolling needs to happen

		string[] tmp = terminal.Split ('\n');
		//Debug.Log ("updating Terminal??? " + tmp.Length);
		if (tmp.Length > maxLines) {
			string tmpTerminal = "";
			int len = tmp.Length - 1;
			for (int i = len-maxLines; i <= len; i++) {
				//Debug.Log (i +"(" + (len-i) + "): " + tmp [(len - i)]);
				tmpTerminal += tmp[i] + "\n";
			}
			terminal = tmpTerminal;
			terminalObj.text = terminal;
		}
	}


	public bool compTime(string pre, string post){
		string[] tmpS = pre.Split(' ',':');
		string[] tmpM = post.Split (' ',':');

		int pe = -1;
		int po = -1;
	
		switch (tmpS[0]) {
		case "Jan":
			pe = 1;
			break;
		case "Feb":
			pe = 2;
			break;
		case "Mar":
			pe = 3;
			break;
		case "Apr":
			pe = 4;
			break;
		case "May":
			pe = 5;
			break;
		case "Jun":
			pe = 6;
			break;
		case "Jul":
			pe = 7;
			break;
		case "Aug":
			pe = 8;
			break;
		case "Sept":
			pe = 9;
			break;
		case "Oct":
			pe = 10;
			break;
		case "Nov":
			pe = 11;
			break;
		case "Dec":
			pe = 12;
			break;
		}

		switch (tmpM[0]) {
		case "Jan":
			po = 1;
			break;
		case "Feb":
			po = 2;
			break;
		case "Mar":
			po = 3;
			break;
		case "Apr":
			po = 4;
			break;
		case "May":
			po = 5;
			break;
		case "Jun":
			po = 6;
			break;
		case "Jul":
			po = 7;
			break;
		case "Aug":
			po = 8;
			break;
		case "Sept":
			po = 9;
			break;
		case "Oct":
			po = 10;
			break;
		case "Nov":
			po = 11;
			break;
		case "Dec":
			po = 12;
			break;
		}

		if (pe <= po) {
			if (Int32.Parse(tmpS [1]) <= Int32.Parse(tmpM [1])) {
				Debug.Log ((tmpS [2]) + ","+ (tmpM [0]));
				if (Int32.Parse(tmpS [2]) <= Int32.Parse(tmpM [0])) {
					if (Int32.Parse(tmpS [3]) < Int32.Parse(tmpM [3])) {
						return true;
					} else {
						//existing is older min
					}
				} else {
					// existing is older hour
				}
			} else {
				// existing is older day
			}
		} else {
			// exsisting is older month
		}
		return false;
	}







}
