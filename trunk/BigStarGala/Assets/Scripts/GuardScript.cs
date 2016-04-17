﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GuardScript : MonoBehaviour
{
    private Dictionary<StateTransition, GuardState> transitions;
    private NavMeshAgent agent;
    private int waypointIndex;
    private Transform currentVIPPosition;
    private bool isCheckingID;
    private bool isWaiting;
    private List<GameObject> peopleInRange;
    private Animator animator;
    private bool isPlayerInSight;
    private Plane[] planes;
    private GameObject player;
    private CapsuleCollider playerCollider;

    public Camera camera;
    public Transform[] Waypoints;
    public int waitingTime;
    public GuardState CurrentState { get; private set; }

    // Use this for initialization
    void Start()
    {
        peopleInRange = new List<GameObject>();
        CurrentState = GuardState.Patrol;
        agent = this.GetComponent<NavMeshAgent>();
        waypointIndex = 0;
        agent.SetDestination(Waypoints[waypointIndex].position);
        isCheckingID = false;
        isWaiting = false;
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<CapsuleCollider>();
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
        animator = GetComponentInChildren<Animator>();

        transitions = new Dictionary<StateTransition, GuardState>
            {
                { new StateTransition(GuardState.Patrol, Command.SeeVIP), GuardState.CheckID },
                { new StateTransition(GuardState.Patrol, Command.Rest), GuardState.Wait },
                { new StateTransition(GuardState.Wait, Command.SeeVIP), GuardState.CheckID },
                { new StateTransition(GuardState.Wait, Command.GetBack), GuardState.Patrol },
                { new StateTransition(GuardState.CheckID, Command.GetBack), GuardState.Patrol },
                { new StateTransition(GuardState.CheckID, Command.Rest), GuardState.Wait }
            };
    }
    // Update is called once per frame
    void Update()
    {

        //if (peopleInRange.Count > 0)
        //    Debug.Log("Object seen : " + peopleInRange[0].ToString() + " List.Count(): " + peopleInRange.Count);

        Debug.Log("Guard State : " + CurrentState + "   NavMeshStatus: " + NavMeshPathStatus.PathComplete + "   Waypoint: " + waypointIndex);

        switch (CurrentState)
        {
            case GuardState.Patrol:
                {
                    agent.Resume();
                    animator.SetFloat("Speed", 10);
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 0.15f)
                    {
                        isWaiting = false;
                        waypointIndex++;
                        if (waypointIndex > 3)
                            waypointIndex = 0;
                        MoveNext(Command.Rest);
                    }
                    observe();
                }
                break;
            case GuardState.Wait:
                {
                    if (!isWaiting)
                        StartCoroutine(wait());
                }
                break;
            case GuardState.CheckID:
                {
                    agent.SetDestination(currentVIPPosition.position);
                    
                    if (agent.remainingDistance < 0.4f)
                    {
                        agent.Stop();

                        Debug.Log(agent.remainingDistance  +  "   " + isCheckingID);
                        if (!isCheckingID)
                        {
                            isCheckingID = true;
                            StartCoroutine(CheckID());
                        }
                    }
                    Vector3 dir = currentVIPPosition.transform.position - transform.position;
                    if (dir != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(dir),
                            Time.deltaTime * 5);
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
        if(collider.tag == "Player")
        {
            if(seePlayer())
                player.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
            else
                player.GetComponent<DetectionIndicator>().State = IndicatorState.Decrement;

        }
    }

    void OnTriggerExit(Collider collider)
    {
        if ((collider.gameObject.tag == "Player" && seePlayer())|| collider.gameObject.tag == "VIP")
        {
            collider.gameObject.GetComponent<DetectionIndicator>().State = IndicatorState.Decrement;
            if (peopleInRange.Contains(collider.gameObject))
                peopleInRange.Remove(collider.gameObject);
        }
    }


    GuardState GetNext(Command command)
    {
        StateTransition transition = new StateTransition(CurrentState, command);
        GuardState nextState;
        if (!transitions.TryGetValue(transition, out nextState))
            throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        return nextState;
    }

    class StateTransition
    {
        readonly GuardState CurrentState;
        readonly Command Command;

        public StateTransition(GuardState currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }

    GuardState MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }

    IEnumerator CheckID()
    {
        animator.SetFloat("Speed", 0);
        Debug.Log("1");
        if (currentVIPPosition.gameObject.tag == "VIP")
        {
            currentVIPPosition.gameObject.GetComponent<VIPScript>().IsChecked = true;
            yield return new WaitForSeconds(3);
            Debug.Log("2");
            MoveNext(Command.GetBack);
            agent.SetDestination(Waypoints[waypointIndex].position);
            isCheckingID = false;
        }
        else if(currentVIPPosition.gameObject.tag == "Player")
        {
            if(Vector3.Distance(currentVIPPosition.position, transform.position) < 0.4f)
            {
                Debug.Log("3");
                currentVIPPosition.GetComponent<PlayerControler>().enabled = false;
            }
        }
        
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
                                if (!currentVIPPosition.gameObject.GetComponent<VIPScript>().IsChecked)
                                {
                                    currentVIPPosition.gameObject.GetComponent<VIPScript>().State = VIPState.BeingChecked;
                                    currentVIPPosition.gameObject.GetComponent<VIPScript>().guard = this.gameObject;
                                    MoveNext(Command.SeeVIP);
                                }
                            }
                        }
                        break;
                    case "Player":
                        {
                            if (x.GetComponent<DetectionIndicator>().Indicator >= 100)
                            {
                                currentVIPPosition = x.transform;
                                MoveNext(Command.SeeVIP);
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
                if (hit.collider.tag == "Player")
                    return true;
                else
                    return false;
            else
                return false;
        }
        return false;
    }

    IEnumerator wait()
    {
        isWaiting = true;
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(waitingTime);
        MoveNext(Command.GetBack);
        agent.SetDestination(Waypoints[waypointIndex].position);
    }
}


public enum GuardState
{
    Patrol,
    Wait,
    CheckID
}

public enum Command
{
    Rest,
    GetBack,
    SeeVIP
}