using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

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

	private FileSystem fileSystem; 

	private Text terminalObj;

	private int maxLines;
	private int charPerLine;
	private int numLines;

	private int index = 0;
	private Command tmpCurCmd = new Command ();

	private int tCount = 0;

	public MessageControl mc;

	// Use this for initialization
	void Start () {
		
		testing = -1;
		active = false;

		//making the file system
		LevelLoader l = gameObject.transform.parent.gameObject.GetComponent<LevelLoader>();
		l.load ();
		fileSystem = l.fs;

		path = fileSystem.curFolder.name;
		// need to change this so that it will always be correct for the specific resolution
		maxLines = 20;
		charPerLine = 28;
		numLines = 0;
		// initialize terminal with username
		terminalObj = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
		terminal += "[" + username + "@" + comp + " " + path + "]$ ";
		terminalObj.text = terminal;


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
							if (curLine.Length > charPerLine - 19)
								updateTerminal ();
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

		foreach(string a in s){
			if (progress == 0) {
				int tmp = -1;
				if(testing >= 1) Debug.Log (":" + curLine + ":");
				for (int i = 0; i < cmds.Count && (tmp < 0); i++) {
					if (cmds [i].com == a) {
						if(testing >= 1) Debug.Log ("ADDED CMD to history: " + cmds [i].com);
						tmp = i;
					}
				}
				if (tmp < 0) {
					curCommand = cmds [0].clone();
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
						for (int i = 1; i < a.Length; i++) {
							if(testing >= 1) Debug.Log ("ADDing option: " + a [i]);
							curCommand.options.Add (a [i] + "");
						}
						// actuall options happen in processing
					} else {
						if(testing >= 1) Debug.Log ("ADDING A PARAM: " + a);
						curCommand.param.Add (a);
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
					if (fileSystem.checkPath (curCommand.param [0])) {
						fileSystem.moveTo (curCommand.param [0]);
						path = fileSystem.curFolder.name;
					} else {
						// error invalid directory
						terminal += "\n -bash: " + curLine + ": Not a directory";
					}
				}
			} else if (curCommand.com == "clear") {
				terminal = "";
				numLines = 0;
				terminal += "[" + username + "@" + comp + " " + path + "]$ ";
				return;
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
				// TODO


				bool n = false;
				bool e = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("n")) {
						n = true;
					} else if (curCommand.options [i].Contains ("e")) {
						e = true;
					}
				}

				Debug.Log ("echo");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "exit") {
				if(testing>=1)Debug.Log ("Application.Quit ();");
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
				// TODO

				
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

				Debug.Log ("head");
				Debug.Log ("Not Functioning yet");
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
					if (fileSystem.curFolder.contentFolders [i].hidden && a) {
						if (l) {
							terminal += fileSystem.curFolder.contentFolders [i].printPermissions () +
								" " + fileSystem.curFolder.contentFolders [i].owner +
								" " + fileSystem.curFolder.contentFolders [i].group +
								" " + fileSystem.curFolder.contentFolders [i].printSize(h) +
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
								" " + fileSystem.curFolder.contentFolders [i].printSize(h) +
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
				// TODO


				bool m = false;
				bool p = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("m")) {
						m = true;
					} else if (curCommand.options [i].Contains ("p")) {
						p = true;
					}
				}

				Debug.Log ("mkdir");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "mv") {
				// TODO


				bool u = false;
				bool f = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("u")) {
						u = true;
					} else if (curCommand.options [i].Contains ("f")) {
						f = true;
					}
				}

				Debug.Log ("mv");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "pwd") {
				terminal += fileSystem.curFolder.path;
			} else if (curCommand.com == "quota") {
				// TODO

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

				Debug.Log ("rm");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "rmdir") {
				// TODO


				bool p = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("p")) {
						p = true;
					} 
				}

				Debug.Log ("rmdir");
				Debug.Log ("Not Functioning yet");
			} else if (curCommand.com == "tail") {
				// TODO


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

				Debug.Log ("tail");
				Debug.Log ("Not Functioning yet");
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
				// TODO


				bool r = false;
				bool t = false;

				for (int i = 0; i < curCommand.options.Count; i++) {
					if (curCommand.options [i].Contains ("r")) {
						r = true;
					} else if (curCommand.options [i].Contains ("t")) {
						t = true;
					}
				}

				Debug.Log ("touch");
				Debug.Log ("Not Functioning yet");
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

		terminal += "\n";
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



}
