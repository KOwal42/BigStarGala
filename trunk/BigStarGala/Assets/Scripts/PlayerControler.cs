using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {

    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    private Texture2D texture;

    public SkinnedMeshRenderer rendererMale;
    public SkinnedMeshRenderer rendererFemale;

    public GameObject MaleModel;
    public GameObject FemaleModel;

    public int ID { get; private set; }
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public GameObject playerMesh;
    public bool tryChangeId = false;
    public Animator animator;
    public int identity = 1;
    // Use this for initialization
    void Start () {

        animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        #region = Movement
        animator.SetFloat("Speed", 0f);

        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        
        if (Input.GetKey("w") && Input.GetKey("d"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, -0.9f, 0, -0.3f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("d") && Input.GetKey("s"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.9f, 0, -0.5f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, -0.9f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);


        }
        else if (Input.GetKey("a") && Input.GetKey("w"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.4f, 0, 0.9f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("a"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0, 0, 1), Time.deltaTime * 10f);//(0, 0, 0, 1.0)
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("d"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 1, 0, 0), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);

        }
        else if (Input.GetKey("s"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, 0.7f, 0, -0.7f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);


        }
        else if (Input.GetKey("w"))
        {
            playerMesh.transform.rotation = Quaternion.Lerp(playerMesh.transform.rotation, new Quaternion(0, -0.7f, 0, -0.7f), Time.deltaTime * 10f);
            animator.SetFloat("Speed", 10f);
        }
        #endregion

        #region = Special

        

        #endregion
    }
    public void changeIdentity(int ID, Texture2D texture, Gender gender)
    {
        if( gender == Gender.Male )
        {
            FemaleModel.SetActive(false);
            MaleModel.SetActive(true);
            playerMesh = MaleModel;
            animator = MaleModel.GetComponent<Animator>();
            rendererMale.material.EnableKeyword("_DETAIL_MULX2");
            rendererMale.material.SetTexture("_DetailAlbedoMap", texture);
        } else
        {
            MaleModel.SetActive(false);
            FemaleModel.SetActive(true);
            playerMesh = FemaleModel;
            animator = FemaleModel.GetComponent<Animator>();
            rendererFemale.material.EnableKeyword("_DETAIL_MULX2");
            rendererFemale.material.SetTexture("_DetailAlbedoMap", texture);
        }
        this.ID = ID;
    }
}
