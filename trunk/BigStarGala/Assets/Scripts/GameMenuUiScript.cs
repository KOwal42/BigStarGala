using UnityEngine;
using System.Collections;

public class GameMenuUiScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void startGame()
    {
        Debug.Log("d");
        Application.LoadLevel("level");
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
