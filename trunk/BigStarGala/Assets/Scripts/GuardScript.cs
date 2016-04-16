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
        //Debug.Log("Object seen : " + peopleInRange[0].ToString() + " List.Count(): " + peopleInRange.Count);

        //Debug.Log("Guard State : " + CurrentState + "   NavMeshStatus: " + NavMeshPathStatus.PathComplete + "   Waypoint: " + waypointIndex);

        switch (CurrentState)
        {
            case GuardState.Patrol:
                {
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 1f)
                    {
                        isWaiting = false;
                        Debug.Log("Movin B!tch Current Waypoint: " + waypointIndex);
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
                    if(!isCheckingID)
                    {
                        isCheckingID = true;
                        agent.SetDestination(currentVIPPosition.position);
                        if (Vector3.Distance(currentVIPPosition.position, transform.position) <= 3)
                        {
                            agent.Stop();
                            StartCoroutine(CheckID());
                        }
                    }
                }
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
            peopleInRange.Add(collider.gameObject);
        }

        if (collider.tag == "VIP")
        {
            if(!collider.GetComponent<VIPScript>().IsChecked)
                collider.GetComponent<DetectionIndicator>().State = IndicatorState.Increment;
            peopleInRange.Add(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        collider.gameObject.GetComponent<DetectionIndicator>().State = IndicatorState.Decrement;
        peopleInRange.Remove(collider.gameObject);
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
        yield return new WaitForSeconds(5);
        currentVIPPosition.gameObject.GetComponent<VIPScript>().IsChecked = true;
        MoveNext(Command.GetBack);
        Debug.Log("Setting destination: " + Waypoints[waypointIndex].position + "    prevoius destination = " + agent.destination.ToString());
        
        agent.SetDestination(Waypoints[waypointIndex].position);
        agent.Resume();
    }

    void observe()
    {
        if (peopleInRange.Count > 0)
        {
            foreach (GameObject x in peopleInRange)
            {
                if (x.GetComponent<DetectionIndicator>().Indicator >= 100)
                {
                    currentVIPPosition = x.transform;
                    MoveNext(Command.SeeVIP);
                    isCheckingID = false;
                }
            }
        }
    }

    IEnumerator wait()
    {
        isWaiting = true;
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