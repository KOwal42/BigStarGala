using UnityEngine;
using System.Collections.Generic;

public class SpecialPlace : MonoBehaviour {


    public int Id;
    public string action;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            if (collider.GetComponent<PlayerControler>().ID == Id)
                collider.GetComponent<PlayerControler>().animator.SetTrigger(action);

        if (collider.tag == "VIP")
            if (collider.GetComponent<VIPController>().ID == Id)
            {
                collider.GetComponent<VIPScript>().animator.SetTrigger(action);
                Debug.Log("DOING STUFFF");
                collider.GetComponent<VIPScript>().State = VIPState.DoStuff;
            }
    }
}

