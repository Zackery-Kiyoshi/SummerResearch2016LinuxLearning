using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Command {

	// the command itself
	public string com;
    public bool error;
	public int numParams;
	// the options for the command

	// paramaters to the command
	public List<string> param = new List<string>();
	// the function the command calls (???)

	// Use this for initialization
	void Start () {
	
	}

	public Command(string a, int p)
    {
        com = a;
		numParams = p;
        error = false;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
