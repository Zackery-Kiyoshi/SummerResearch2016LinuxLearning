﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*

pwd
cd
ls

*/



public class TerminalControl : MonoBehaviour {

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

	private FileSystem fileSystem = new FileSystem (); 

	private Text terminalObj;

	// Use this for initialization
	void Start () {
		active = false;

		//making the file system

		fileSystem.root.addFile ("FirstTest0","TESTING0");
		fileSystem.root.addFile ("FirstTest1","TESTING1");
		fileSystem.root.addFile ("FirstTest2","TESTING2");
		fileSystem.root.addFile ("FirstTest3","TESTING3");

		fileSystem.root.addFolder ("FirstFolder");
		fileSystem.root.addFolder ("Downloads");
		fileSystem.root.addFolder ("Documents");

		// initialize terminal with username
		terminalObj = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
		terminal += "[" + username + "@" + comp + " " + path + "]$ ";
		terminalObj.text = terminal;


		List<string> tmpOpt;
		// all commands
        cmds.Add(new Command("pwd",0));
        cmds.Add(new Command("cd",0));
		tmpOpt = new List<string> ();
		tmpOpt.Add ("l");
		tmpOpt.Add ("a");
		tmpOpt.Add ("h");
		cmds.Add(new Command("ls",0, tmpOpt ));
		cmds.Add(new Command("clear",0));

    }
	
	// switched from FixedUpdate to fix lag issues
	void Update () {
		if (active) {
			if (Input.anyKeyDown) {
                foreach (char c in Input.inputString)
                {
					if (c == ' ') {
						curLine = curLine + " ";
						terminal += " ";
					} else if (c == "\b" [0]) {
						if (curLine.Length != 0){
							curLine = curLine.Substring (0, curLine.Length - 1);
							terminal = terminal.Substring (0, terminal.Length - 1);
						}
					} else if (c == "\n" [0] || c == "\r" [0]) {

						processCommand (curLine);

					} else if (c == "\t" [0]) {
					} else {
						curLine += c;
						curParam += c;
						terminal += c;
					}
                }
				terminalObj.text = terminal;
			}
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

		// processes the string[] into command
		foreach(string a in s){
			if (progress == 0) {
				int tmp = -1;
				Debug.Log (":" + curLine + ":");
				for (int i = 0; i < cmds.Count && (tmp < 0); i++) {
					if (cmds [i].com == a) {
						Debug.Log ("ADDED CMD to history: " + cmds [i].com);
						tmp = i;
					}
				}
				if (tmp < 0) {
					curCommand = cmds [0];
					curCommand.error = true;
				} else {
					curCommand = cmds [tmp];
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
							Debug.Log ("ADDing option: " + a [i]);
							curCommand.options.Add (a [i] + "");
						}
						// actuall options happen in processing
					} else {
						Debug.Log ("ADDING A PARAM: " + a);
						curCommand.param.Add (a);
					}
					progress++;
				}
			}
		}

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
			if (curCommand == null) {
				Debug.Log ("curCommand is null");
			} else if (curCommand.com == "pwd") {
				terminal += "\n";
				terminal += fileSystem.curPath;
				terminal += "\n";
			}
			else if (curCommand.com == "cd") {
				Debug.Log ("ITS PROCESSING CD");
				// check for path
				if (curCommand.param.Count < 1) {
					// return to root TODO

					Debug.Log("returning to default location");
				} else {
					if (fileSystem.checkPath (curCommand.param [0])) {
						fileSystem.moveTo (curCommand.param [0]);
						path = fileSystem.curFolder.name;
					} else {
						// error invalid directory
						terminal += "\n -bash: " + curLine + ": Not a directory";
					}
				}
				terminal += "\n";
			}
			else if (curCommand.com == "ls") {
				terminal += "\n";
				terminal += "<color=blue>";
				for (int i = 0; i < fileSystem.curFolder.contentFolders.Count; i++) {
					terminal += "[ " + fileSystem.curFolder.contentFolders [i].name + " ]\n";
				}
				terminal += "</color>";
				for (int i = 0; i < fileSystem.curFolder.contentFiles.Count; i++) {
					terminal += fileSystem.curFolder.contentFiles [i].name + "\n";
				}
				terminal += "\n";
			}
			else if (curCommand.com == "clear") {
				terminal = "";
				//"[" + username + "@" + comp + " " + path + "]$ ";
			}
			Debug.Log("Doing Command");
		}
		curLine = "";
		curParam = "";
		error = "";
		progress = 0;

		terminal += "[" + username + "@" + comp + " " + path + "]$ ";
	}
}
