using System.Collections;
using UnityEngine;

public class VIPScript : MonoBehaviour
{

    private float timeSinceLastChecked;
    private NavMeshAgent agent;
    private int waypointIndex;
    private bool coroutineStareted;
    public Animator animator;

    public GameObject guard;
    public VIPState State { get; set; }
    public float CoolDown;
    public Transform[] Waypoints;
    public bool IsChecked { get; set; }

    // Use this for initialization
    void Start()
    {
        timeSinceLastChecked = 0;
        IsChecked = false;
        agent = GetComponent<NavMeshAgent>();
        State = VIPState.Walking;
        coroutineStareted = false;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (State)
        {
            case VIPState.Walking:
                {
                    agent.Resume();
                    animator.SetFloat("Speed", 10);
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.15f)
                    {
                        waypointIndex++;
                        if (waypointIndex > Waypoints.Length - 1)
                            waypointIndex = 0;
                        agent.SetDestination(Waypoints[waypointIndex].position);
                    }
                }
                break;
            case VIPState.BeingChecked:
                {
                    agent.Stop();
                    Vector3 dir = guard.transform.position - transform.position;
                    if (dir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(dir),
                            Time.deltaTime * 10);
                    }
                    if (!coroutineStareted && Vector3.Distance(guard.transform.position, transform.position) < 0.5f)
                        StartCoroutine(BeingChecked());
                }
                break;
            case VIPState.DoStuff:
                {
                    agent.Stop();
                    animator.SetFloat("Speed", 0);
                    if(!coroutineStareted)
                        StartCoroutine(DoStuff());
                }
                break;
        }

        if (IsChecked)
            timeSinceLastChecked += Time.deltaTime;

        if (timeSinceLastChecked > CoolDown)
        {
            timeSinceLastChecked = 0;
            IsChecked = false;
        }
    }

    IEnumerator BeingChecked()
    {
        coroutineStareted = true;
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Explain");
        yield return new WaitForSeconds(1.5f);
        animator.SetTrigger("Explain");
        yield return new WaitForSeconds(1.5f);
        State = VIPState.Walking;
        coroutineStareted = false;
    }

    IEnumerator DoStuff()
    {
        coroutineStareted = true;
        yield return new WaitForSeconds(4);
        State = VIPState.Walking;
        coroutineStareted = false;
    }
}

public enum VIPState
{
    Walking,
    DoStuff,
    BeingChecked
}
