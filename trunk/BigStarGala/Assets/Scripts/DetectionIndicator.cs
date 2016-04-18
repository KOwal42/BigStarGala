using UnityEngine;
using UnityEngine.UI;

public class DetectionIndicator : MonoBehaviour
{
    public float Indicator { get; private set; }
    public IndicatorState State { get; set; }
    public int IndicatorOffsetX;
    public int IndicatorOffsetY;

    public Image Eye;

    public int rate;

    // Use this for initialization
    void Start()
    {
        Indicator = 0;
        State = IndicatorState.Decrement;
        //Eye.color = new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Player")
            Eye.color = new Color(1, 1, 1, Indicator / 100.0f);

        //Debug.Log(gameObject.ToString() + "  Indicator = " + Indicator + "  IndicatorState = " + State);
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