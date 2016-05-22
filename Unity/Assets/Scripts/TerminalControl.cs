using UnityEngine;
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
	private int curCmdNum = 0;
	private string curLine = "";
	private string curParam = "";

    private List<Command> cmds = new List<Command>();
    private int progress = 0;
    private bool error = false;

	// Use this for initialization
	void Start () {
		active = false;

        cmds.Add(new Command("pwd"));
        cmds.Add(new Command("cd"));
        cmds.Add(new Command("ls"));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (active) {
			if (Input.anyKeyDown) {
                foreach (char c in Input.inputString)
                {
                    if (c == ' ')
                    {
                        // process command or prepare for input
                        if (progress == 0)
                        {
                            bool tmp = true;
                            for (int i = 0; i < cmds.Count && !tmp; i++)
                            {
                                if (cmds[i].com == curLine)
                                {
                                    cmdHistory.Add(cmds[i]);
                                    tmp = false;
                                }
                            }
                            cmdHistory[cmdHistory.Count - 1].error = tmp;

                            progress++;
                        }
                        curLine = curLine + " ";
                        curParam = "";
                    }
                    else if (c == "\b"[0])
                    {
                        if (curLine.Length != 0)
                            curLine = curLine.Substring(0, curLine.Length - 1);
                    }
                    else if (c == "\n"[0] || c == "\r"[0])
                    {
                        if (error)
                        {
                            // error
                        }
                        else
                        {
                            // actually do command
                        }
                    }
                    else if (c == "\t"[0]) { }
                    else
                        curLine += c;
                }
			}
		}
	}

	public void activate(){
		active = true;
	}

	public void deactivate(){
		active = false;
	}
}
