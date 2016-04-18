using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonProgramator : MonoBehaviour {

    public Canvas Launch;
    public Canvas Credits;

	// Use this for initialization
	void Start () {
        Launch.enabled = true;
        Credits.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Creditsy()
    {
        Launch.enabled = false;
        Credits.enabled = true;
    }

    public void Back()
    {
        Credits.enabled = false;
        Launch.enabled = true;
    }
}
