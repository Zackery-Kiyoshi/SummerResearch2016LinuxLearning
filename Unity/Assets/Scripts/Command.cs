using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Command {

	// the command itself
	public string com = "";
	public string line = "";
    public bool error;
	public int numParams;
	public string man;
	// the options for the command
	public List<string> options = new List<string>();
	// functions to call for specific options (make pair???)
	public List<string> optionFuncs = new List<string>();
	// paramaters to the command
	public List<string> param = new List<string>();
	// the function the command calls (???)

	// Use this for initialization
	void Start () {
	
	}

	public Command()
	{
		com = "MADE WITH NO NAME OR PARAMS";
		numParams = -1;
		error = true;
	}

	public Command(string a, int p)
    {
        com = a;
		numParams = p;
        error = false;
    }

	public Command(string a, int p, List<string> o)
	{
		com = a;
		numParams = p;
		error = false;
		options = o;
	}

	public Command clone(){
		Command ret = new Command(com,numParams);
		ret.com = com;
		ret.line = "";
		ret.numParams = numParams;
		ret.man = man;
		return ret;
	}

    // Update is called once per frame
    void Update () {
	
	}
}
