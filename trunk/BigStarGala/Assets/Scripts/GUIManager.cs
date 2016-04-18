using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour
{

    public Texture2D texture;
    public Texture2D GREEN;
    public Texture2D RED;
    public Texture2D BLUE;

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (VictoryConditionManager.photo && VictoryConditionManager.statue && VictoryConditionManager.wave)
        {
            GUI.Label(new Rect(Screen.height - 10, Screen.width - 40, 20, 80), "You WIN!");
            if (GUI.Button(new Rect(Screen.height + 40, Screen.width - 80, 20, 20), "Play Again?")) 
            Application.LoadLevel(1);
            if (GUI.Button(new Rect(Screen.height + 40, Screen.width + 60, 20, 20), "Continue"));
        }
    }


}
