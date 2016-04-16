using System.Collections;
using UnityEngine;

public class VIPScript : MonoBehaviour
{

    private float timeSinceLastChecked;
    private NavMeshAgent agent;
    private int waypointIndex;
    private bool coroutineStareted;

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
    }

    // Update is called once per frame
    void Update()
    {

        switch (State)
        {
            case VIPState.Walking:
                {
                    agent.Resume();
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 1f)
                    {
                        waypointIndex++;
                        if (waypointIndex > 1)
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
                    if (!coroutineStareted)
                        StartCoroutine(BeingChecked());
                }
                break;
            case VIPState.DoStuff:
                {
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
        yield return new WaitForSeconds(3);
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
