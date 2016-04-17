using UnityEngine;
using System.Collections;

public class VIPController : MonoBehaviour {


    [SerializeField]
    private GameObject player;
    public GameObject parent;
    public float distance = 2f;
    private float distanceBetweenPlayer;
    public bool playerIsNoticed = false;


	// Use this for initialization
	void Start () {
        playerIsNoticed = false;
	}
	
	// Update is called once per frame
	void Update () {
        distanceBetweenPlayer = Vector3.Distance(transform.position, player.transform.position);
       // Debug.Log(distanceBetweenPlayer);
       // Debug.Log(player.GetComponent<PlayerControler>().tryChangeId.ToString() + playerIsNoticed.ToString());
        if(distanceBetweenPlayer <= distance && player.GetComponent<PlayerControler>().tryChangeId == true && playerIsNoticed == false)
        {
            Debug.Log("triggg");
            player.GetComponent<PlayerControler>().changeIdentity(parent);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsNoticed = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsNoticed = false;
        }
    }
    
}
