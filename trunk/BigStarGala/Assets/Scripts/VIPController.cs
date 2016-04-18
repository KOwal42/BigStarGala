using UnityEngine;
using System.Collections;

public class VIPController : MonoBehaviour {


    private GameObject player;
    public float distance = 2f;

    public Gender Gender;
    public bool PlayerIsNoticed { get; private set; }
    public int ID;
    public Texture2D skin; 

	// Use this for initialization
	void Start () {
        PlayerIsNoticed = false;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if(Vector3.Distance(transform.position, player.transform.position) <= distance && Input.GetKeyDown(KeyCode.LeftShift) && PlayerIsNoticed == false)
        {
            player.GetComponent<PlayerControler>().changeIdentity(ID, skin, Gender);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerIsNoticed = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerIsNoticed = false;
        }
    }
}


public enum Gender
{
    Female,
    Male
}