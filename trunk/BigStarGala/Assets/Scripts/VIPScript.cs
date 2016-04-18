using System.Collections;
using UnityEngine;

public class VIPScript : MonoBehaviour
{

    private float timeSinceLastChecked;
    private NavMeshAgent agent;
    private int currentIndex;
    private bool coroutineStareted;
    public Animator animator;
    private bool setPath;

    public GameObject guard;
    public VIPState State { get; set; }
    public float CoolDown;
    public SpecialPlace[] Waypoints;
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
                    if (Waypoints.Length > 1)
                    {
                        animator.SetFloat("Speed", 10);
                        agent.Resume();

                        while (CheckIfAvaible(currentIndex) && Waypoints[currentIndex].IdRef != GetComponent<VIPController>().ID)
                        {
                            currentIndex = RandomizeWaypoint();
                            setPath = true;
                        }

                        if (setPath)
                        {
                            Waypoints[currentIndex].SetMyId(GetComponent<VIPController>().ID);
                            agent.SetDestination(Waypoints[currentIndex].transform.position);
                            setPath = false;
                        }

                        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.15f)
                            State = VIPState.Wait;
                    }
                    else
                    {
                        agent.SetDestination(Waypoints[0].transform.position);
                    }
                }
                break;
            case VIPState.Wait:
                {
                    if (!coroutineStareted)
                        StartCoroutine(wait());
                    TurnTo(Waypoints[currentIndex].Direction);
                }
                break;
            case VIPState.BeingChecked:
                {
                    agent.Stop();
                    animator.SetFloat("Speed", 0);
                    Vector3 dir = guard.transform.position - transform.position;
                    TurnTo(dir);
                    if (!coroutineStareted && Vector3.Distance(guard.transform.position, transform.position) < 0.5f)
                        StartCoroutine(BeingChecked());
                }
                break;
            case VIPState.DoStuff:
                {
                    //Debug.Log("DO Stuff");
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.15f)
                        if (!coroutineStareted)
                            StartCoroutine(DoStuff());
                    animator.SetFloat("Speed", 0);
                    TurnTo(Waypoints[currentIndex].GetComponent<SpecialPlace>().Direction);
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
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 4; i++)
        {
            switch (GetComponent<VIPController>().ID)
            {
                case 1: { animator.SetTrigger("TakingPhoto"); } break;
                case 2: { animator.SetTrigger("RecievingStatue"); } break;
                case 3: { animator.SetTrigger("Wave"); } break;
            }
            yield return new WaitForSeconds(2);
        }
        State = VIPState.Walking;
        coroutineStareted = false;
    }

    bool CheckIfAvaible(int i)
    {
        return Waypoints[i].IsSomeone;
    }

    int RandomizeWaypoint()
    {
        return UnityEngine.Random.Range(0, Waypoints.Length);
    }

    IEnumerator wait()
    {
        coroutineStareted = true;
        animator.SetFloat("Speed", 0);
        Debug.Log("Czekanie");
        yield return new WaitForSeconds(Waypoints[currentIndex].RandomizeTime());
        currentIndex = RandomizeWaypoint();
        agent.SetDestination(Waypoints[currentIndex].transform.position);
        State = VIPState.Walking;
        coroutineStareted = false;
    }

    void TurnTo(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 10);
        }
    }
}

public enum VIPState
{
    Walking,
    DoStuff,
    BeingChecked,
    Wait
}
