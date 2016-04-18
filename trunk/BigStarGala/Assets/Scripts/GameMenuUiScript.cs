using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenuUiScript : MonoBehaviour {


    public Text Photo;
    public Text Stage;
    public Text Crowd;

    public Slider Slider;

    public Image buttonImage;
    public Image GameOver;

    public Canvas[] Canvases;
    // Use this for initialization
    void Start () {
        GameOver.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        Photo.text = "Photos \nleft: " + (4 - VictoryConditionManager.Photo);
        Stage.text = "Stage performances \nleft: " + (4 - VictoryConditionManager.Statue);
        Crowd.text = "Fans approval \nleft: " + (4 - VictoryConditionManager.Wave);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

    }

    public void ToStart()
    {
        SceneManager.LoadScene(0);
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
