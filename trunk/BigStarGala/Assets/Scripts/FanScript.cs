using UnityEngine;
using System.Collections;

public class FanScript : MonoBehaviour {

    private Animator animator;
    private float currentCooldown;
    private float delta;

    public float JumpCooldownUp;
    public float JumpCooldownDown;

	// Use this for initialization
	void Start () {

        animator = GetComponentInChildren<Animator>();
        currentCooldown = 2;
        delta = 0;
    }
	
	// Update is called once per frame
	void Update () {
        delta += Time.deltaTime;
        //Debug.Log(delta + " " + currentCooldown);


        if (currentCooldown < delta)
        {
            //Debug.Log(currentCooldown + delta);
            animator.SetTrigger("Jump");
            currentCooldown = Random.Range(JumpCooldownDown, JumpCooldownUp);
            delta = 0;
        }
	}
}
