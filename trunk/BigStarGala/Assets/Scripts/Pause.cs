using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{

    private bool pauseEnabled;

    void Start()
    {
        pauseEnabled = false;
        Time.timeScale = 1;
        //  AudioListener.volume = 1;
        //Cursor.visible = false;
    }

    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            if (pauseEnabled == true)
            {
                pauseEnabled = false;
                Time.timeScale = 1;
                //   AudioListener.volume = 1;
                //Cursor.visible = false;
            }

            else if (pauseEnabled == false)
            {
                pauseEnabled = true;
                //  AudioListener.volume = 0;
                Time.timeScale = 0;
                //Cursor.visible = true;
            }
        }
    }
}