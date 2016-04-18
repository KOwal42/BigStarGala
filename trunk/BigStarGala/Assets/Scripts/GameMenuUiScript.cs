using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenuUiScript : MonoBehaviour {


    public Text Photo;
    public Text Stage;
    public Text Crowd;

    public Slider Slider;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Photo.text = "Photos \nleft: " + (4 - VictoryConditionManager.Photo);
        Stage.text = "Stage performances \nleft: " + (4 - VictoryConditionManager.Statue);
        Crowd.text = "Fans approval \nleft: " + (4 - VictoryConditionManager.Wave);

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
