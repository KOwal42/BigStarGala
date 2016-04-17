using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public GameObject playerMesh;
    public bool tryChangeId = false;
<<<<<<< HEAD
    public int identity;
=======

    private Animator animator;
>>>>>>> e2024a74bb8b20ce6cc9b819b246042a357aa39e
    // Use this for initialization
    void Start () {

        animator = GetComponentInChildren<Animator>();
            
    }
	
	// Update is called once per frame
	void Update ()
    {
        animator.SetFloat("Speed", 10f);
        
        //float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        //rotation *= Time.deltaTime;
        //transform.Rotate(0, rotation, 0);
        ///float translationX = Input.GetAxis("Horizontal") * speed;
        //float translationZ = Input.GetAxis("Vertical") * speed;
        if (Input.GetKey("w") && Input.GetKey("d"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, 0.9f), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("d") && Input.GetKey("s"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.9f, 0, 0.4f), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.9f, 0, -0.4f), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("a") && Input.GetKey("w"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, -0.9f), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 1, 0, 0), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0, 0, 1), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.7f, 0, 0.7f), Time.deltaTime * 10f);
        }
        else if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.7f, 0, -0.7f), Time.deltaTime * 10f);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            tryChangeId = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            tryChangeId = false;
        }


        //Debug.Log("Rotation : " + transform.rotation.ToString());
        //if (Input.GetAxis("Vertical") > 0)
        //    playerMesh.transform.rotation = new Quaternion(0, 0, 0, 1);
        //if (Input.GetAxis("Vertical") < 0)
        //    playerMesh.transform.rotation = new Quaternion(0, 1, 0, 0); ;
        //if (Input.GetAxis("Horizontal") > 0)
        //    playerMesh.transform.rotation = new Quaternion(0, 0.7f, 0, 0.7f);
        //if (Input.GetAxis("Horizontal") < 0)
        //    playerMesh.transform.rotation = new Quaternion(0, 0.7f, 0, -0.7f);

        //translationZ *= Time.deltaTime;
        //translationX *= Time.deltaTime;
        //transform.Translate(translationX, 0, translationZ);
    }
    public void changeIdentity(GameObject obj)
    {
        playerMesh.GetComponent<MeshFilter>().sharedMesh = obj.GetComponent<MeshFilter>().mesh;
        identity = obj.GetComponentInChildren<VIPController>().VIP_ID;

        Debug.Log(obj.GetComponentInChildren<VIPController>().VIP_ID);
    }
}
