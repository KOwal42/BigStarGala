using UnityEngine;

public class DetectionIndicator : MonoBehaviour
{
    public float Indicator { get; private set; }
    public IndicatorState State { get; set; }

    public int rate;

    // Use this for initialization
    void Start()
    {
        Indicator = 0;
        State = IndicatorState.Decrement;
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(gameObject.ToString() + "  Indicator = " + Indicator);
        switch (State)
            {
                case IndicatorState.Increment:
                    {
                        Indicator += rate * Time.deltaTime;
                        if (Indicator > 100)
                            Indicator = 100;
                    }
                    break;
                case IndicatorState.Decrement:
                    {
                        Indicator -= rate * Time.deltaTime;
                        if (Indicator < 0)
                            Indicator = 0;
                    }
                    break;
            }
    }
}

public enum IndicatorState
{
    Increment,
    Decrement
}