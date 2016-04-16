using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

    public int celebrityID;
    public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
      }


    void OnTriggerStay(Collider other)
    {
       // Debug.Log("ID" + player.GetComponent<PlayerControler>().identity + " C" + celebrityID);

        if (Input.GetKeyDown(KeyCode.LeftShift) && player.GetComponent<PlayerControler>().identity == celebrityID && other.gameObject.tag == "Player")
        {
            Debug.Log("Attempt to steal VIP's belongings!");
        }
    }
}
