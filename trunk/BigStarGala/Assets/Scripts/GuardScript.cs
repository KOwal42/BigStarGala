using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GuardScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private int waypointIndex;
    private int currentIndex;
    public Transform currentVIPPosition;
    private bool coroutineInProgress;
    private List<GameObject> peopleInRange;
    private Animator animator;
    private bool isPlayerInSight;
    private Plane[] planes;
    private GameObject player;
    private CapsuleCollider playerCollider;
    private bool setPath;
    private Vector3 lastSeen;

    public new Camera camera;
    public int Id;
    public Waypoint[] Waypoints;
    public GuardState CurrentState { get; set; }

    // Use this for initialization
    void Start()
    {
        peopleInRange = new List<GameObject>();
        CurrentState = GuardState.Patrol;
        agent = this.GetComponent<NavMeshAgent>();
        waypointIndex = 0;
        if (Waypoints.Length > 1)
        {
            currentIndex = RandomizeWaypoint();
            agent.SetDestination(Waypoints[currentIndex].transform.position);
        }
        else
        {
            agent.SetDestination(Waypoints[0].transform.position);
        }
        coroutineInProgress = false;
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<CapsuleCollider>();
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("name: " + name + "   currentWaypoint: " + currentIndex + "   Waypoint IDref: " + Waypoints[currentIndex].IdRef + "  Id: " + Id + "   Destination: " + agent.destination + "  pathStatus: " + agent.pathStatus + "    remainingDistance: " + agent.remainingDistance);
        switch (CurrentState)
        {
            case GuardState.Patrol:
                {
                    if (Waypoints.Length > 1)
                    {
                        animator.SetFloat("Speed", 10);
                        agent.Resume();

                        while (CheckIfAvaible(currentIndex) && Waypoints[currentIndex].IdRef != Id)
                        {
                            currentIndex = RandomizeWaypoint();
                            setPath = true;
                        }

                        if (setPath)
                        {
                            Waypoints[currentIndex].SetMyId(Id);
                            agent.SetDestination(Waypoints[currentIndex].transform.position);
                            setPath = false;
                        }

                        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.15f)
                            CurrentState = GuardState.Wait;
                    }
                    else
                    {
                        agent.SetDestination(Waypoints[0].transform.position);
                    }
                    observe();
                }
                break;
            case GuardState.Wait:
                {
                    if (!coroutineInProgress)
                        StartCoroutine(wait());
                    TurnTo(Waypoints[currentIndex].Direction);
                    observe();
                }
                break;
            case GuardState.CheckID:
                {
                    if(currentVIPPosition.tag == "VIP")
                        agent.SetDestination(currentVIPPosition.position);
                    else
                        agent.SetDestination(lastSeen);

                    if (agent.remainingDistance < 0.4f)
                    {
                        agent.Stop();
                        if (!coroutineInProgress)
                        {
                            StartCoroutine(CheckID());
                        }
                    }

                    if (currentVIPPosition.tag == "VIP")
                    {
                        Vector3 dir = currentVIPPosition.transform.position - transform.position;
                        TurnTo(dir);
                    } else
                    {
                        Vector3 dir = lastSeen - transform.position;
                        TurnTo(dir);
                    }

                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player" && seePlayer())
        {
            collider.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
            peopleInRange.Add(collider.gameObject);
        }

        if (collider.tag == "VIP")
        {
            if (!collider.GetComponent<VIPScript>().IsChecked && collider.GetComponent<VIPScript>().State != VIPState.DoStuff)
            {
                collider.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
                peopleInRange.Add(collider.gameObject);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player")
        {
            if (seePlayer())
                player.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
            else
                player.GetComponent<DetectionIndicator>().State = IndicatorState.Decrement;

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if ((collider.gameObject.tag == "Player" && seePlayer()) || collider.gameObject.tag == "VIP")
        {
            collider.gameObject.GetComponent<DetectionIndicator>().State = IndicatorState.Decrement;
            if (peopleInRange.Contains(collider.gameObject))
                peopleInRange.Remove(collider.gameObject);
        }
    }



    IEnumerator CheckID()
    {
        coroutineInProgress = true;
        animator.SetFloat("Speed", 0);
        if (currentVIPPosition.gameObject.tag == "VIP")
        {
            currentVIPPosition.gameObject.GetComponent<VIPScript>().IsChecked = true;
            yield return new WaitForSeconds(3);
            CurrentState = GuardState.Patrol;
            agent.SetDestination(Waypoints[currentIndex].transform.position);
        }
        else if (currentVIPPosition.gameObject.tag == "Player")
        {
            if (Vector3.Distance(currentVIPPosition.position, transform.position) < 0.4f)
            {
                GameObject.FindObjectOfType<GameMenuUiScript>().GameOver.enabled = true;
                Time.timeScale = 0;
                currentVIPPosition.GetComponent<PlayerControler>().enabled = false;
            } 
        }
        coroutineInProgress = false;
    }

    void observe()
    {
        if (peopleInRange.Count > 0)
        {
            foreach (GameObject x in peopleInRange)
            {
                switch (x.tag)
                {
                    case "VIP":
                        {
                            if (x.GetComponent<DetectionIndicator>().Indicator >= 100)
                            {
                                currentVIPPosition = x.transform;
                                if (!currentVIPPosition.gameObject.GetComponent<VIPScript>().IsChecked && currentVIPPosition.gameObject.GetComponent<VIPScript>().State != VIPState.BeingChecked)
                                {
                                    currentVIPPosition.gameObject.GetComponent<VIPScript>().State = VIPState.BeingChecked;
                                    currentVIPPosition.gameObject.GetComponent<VIPScript>().guard = this.gameObject;
                                    CurrentState = GuardState.CheckID;
                                }
                            }
                        }
                        break;
                    case "Player":
                        {
                            if (x.GetComponent<DetectionIndicator>().Indicator >= 100)
                            {
                                currentVIPPosition = x.transform;
                                CurrentState = GuardState.CheckID;
                            }
                        }
                        break;
                }
            }
        }
    }

    bool seePlayer()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, player.transform.position - transform.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    lastSeen = hit.transform.position;
                    return true;
                } 
                else
                    return false;
            }
            else
                return false;
        }
        return false;
    }

    IEnumerator wait()
    {
        coroutineInProgress = true;
        animator.SetFloat("Speed", 0);
        //Debug.Log("Czekanie");
        yield return new WaitForSeconds(Waypoints[currentIndex].RandomizeTime());
        currentIndex = RandomizeWaypoint();
        agent.SetDestination(Waypoints[currentIndex].transform.position);
        CurrentState = GuardState.Patrol;
        coroutineInProgress = false;
    }

    int RandomizeWaypoint()
    {
        return UnityEngine.Random.Range(1, Waypoints.Length);
    }

    bool CheckIfAvaible(int i)
    {
        return Waypoints[i].IsSomeone;
    }

    void TurnTo(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 5);
        }
    }
}


public enum GuardState
{
    Patrol,
    Wait,
    CheckID
}
