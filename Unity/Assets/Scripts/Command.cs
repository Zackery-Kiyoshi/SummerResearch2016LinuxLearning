using UnityEngine;
using System.Collections;

public class Command : MonoBehaviour {

	// the command itself
	public string com;
    public bool error;
	// the options for the command

	// paramaters to the command

	// the function the command calls (???)

	// Use this for initialization
	void Start () {
	
	}

    public Command(string a)
    {
        com = a;
        error = false;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
