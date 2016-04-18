using UnityEngine;
using System.Collections;

public static class VictoryConditionManager{

    private static int Wave = 0;
    private static int Photo = 0;
    private static int Statue = 0;

    public static bool wave = false;
    public static bool photo = false;
    public static bool statue = false;

    public static void UpdateAction(int i)
    {
        
        switch (i)
        {
            case 1: { Photo++;    photo = Photo >= 4 ? true : false;    if (photo) GameObject.FindObjectOfType<GUIManager>().camera3.enabled = false;  } break;
            case 2: { Statue++;   statue = Statue >= 4 ? true : false;  if (statue) GameObject.FindObjectOfType<GUIManager>().camera2.enabled = false; } break;
            case 3: { Wave++;     wave = Wave >= 4 ? true : false;      if (wave) GameObject.FindObjectOfType<GUIManager>().camera1.enabled = false;   } break;
        }
        Debug.Log("Photo: " + Photo + "   Statue: " + Statue + "    Wave: " + Wave);
    }
}
