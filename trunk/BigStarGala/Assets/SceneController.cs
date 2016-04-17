using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

    public int celebrityID;
    public GameObject player;
    public bool canSteal = true;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerStay(Collider other)
    {
        Debug.Log("ID" + player.GetComponent<PlayerControler>().identity + " C" + celebrityID);

        if (other.gameObject.tag == "VIP" && other.GetComponent<VIPScript>().VIP_ID == celebrityID)
        {
            canSteal = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.GetComponent<PlayerControler>().identity == celebrityID && other.gameObject.tag == "Player")
        {
            if (canSteal)
                Debug.Log("Attempt to steal VIP's belongings!");
        }
    }
}
