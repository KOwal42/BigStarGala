using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour {
    
    [SerializeField]
    GameObject player;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 offset = new Vector3(-5, 15, 0);
        //transform.Translate(player.transform.position.x, 0 , player.transform.position.z) ;
        transform.position = player.transform.position + offset;
	}
}
