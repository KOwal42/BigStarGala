using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public GameObject playerMesh;
    public bool tryChangeId = false;
    private Animator animator;
    public int identity = 1;
    // Use this for initialization
    void Start () {

        animator = GetComponentInChildren<Animator>();
            
    }
	
	// Update is called once per frame
	void Update ()
    {

        //float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        //rotation *= Time.deltaTime;
        //transform.Rotate(0, rotation, 0);
        ///float translationX = Input.GetAxis("Horizontal") * speed;
        //float translationZ = Input.GetAxis("Vertical") * speed;
        //Debug.Log(playerMesh.transform.rotation.ToString());
        
        if (Input.GetKey("w") && Input.GetKey("d"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, -0.9f, 0, -0.3f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("d") && Input.GetKey("s"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.9f, 0, -0.5f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, -0.9f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);


        }
        else if (Input.GetKey("a") && Input.GetKey("w"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, 0.9f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0, 0, 1), Time.deltaTime * 10f);//(0, 0, 0, 1.0)
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 1, 0, 0), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.7f, 0, -0.7f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);


        }
        else if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, -0.7f, 0, -0.7f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);


        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        //playerMesh.GetComponentInChildren<Renderer>().sharedMaterial = obj.GetComponent<Renderer>().material;
        ////playerMesh.transform.rotation = oldRotation;
        //Debug.Log("Changed identity!");
        //identity = obj.GetComponentInChildren<VIPScript>().VIP_ID;

        //Debug.Log(identity);
    }
}
