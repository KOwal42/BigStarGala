using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Waypoint : MonoBehaviour
{

    public Vector3 Direction;
    public float minTime;
    public float maxTime;

    public Vector3 Position { get; private set; }
    public int IdRef { get; private set; }
    public bool IsSomeone { get; private set; }
    // Use this for initialization
    void Start()
    {
        IdRef = 0;
        Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Guard")
        {
            IsSomeone = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Guard")
        {
            IdRef = 0;
            IsSomeone = false;
        }
    }

    public void TurnTo(GameObject go)
    {
        if (Direction != Vector3.zero)
        {
            go.transform.rotation = Quaternion.Slerp(
                go.transform.rotation,
                Quaternion.LookRotation(Direction),
                Time.deltaTime * 5);
        }
    }

    public float RandomizeTime()
    {
        return Random.Range(minTime, maxTime);
    }

    public void SetMyId(int Id)
    {
        IdRef = ObjectCopier.Clone<int>(Id);
    }
}
