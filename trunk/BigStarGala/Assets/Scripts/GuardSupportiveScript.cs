using UnityEngine;
using System.Collections;

public class GuardSupportiveScript : MonoBehaviour
{


    private float timeSinceLastSeen;
    private float timeToGiveUp;


    // Use this for initialization
    void Start()
    {

        timeSinceLastSeen = 0;
        timeToGiveUp = 15;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<GuardScript>().CurrentState == GuardState.CheckID && GetComponent<GuardScript>().currentVIPPosition.tag == "Player")
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen > timeToGiveUp)
                GetComponent<GuardScript>().CurrentState = GuardState.Patrol;
        }

    }
}
