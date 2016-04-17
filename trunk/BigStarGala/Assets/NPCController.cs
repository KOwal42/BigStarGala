using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Celerity 1 is taking photos");
    }
}
