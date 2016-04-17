using UnityEngine;

public class DetectionIndicator : MonoBehaviour
{
    public float Indicator { get; private set; }
    public IndicatorState State { get; set; }
    public int IndicatorOffsetX;
    public int IndicatorOffsetY;
    public Texture2D texture;

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

    void OnGUI()
    {

        //        worldPosition = new Vector3(transform.position.x, transform.position.y + adjustment,
        //transform.position.z);
        //        screenPosition = myCamera.WorldToScreenPoint(worldPosition);

        //        GUI.Box(new Rect(screenPosition.x - healthBarLeft / 2,
        //        Screen.height - screenPosition.y - barTop,
        //        100, healthBarHeight), "");
        //        GUI.DrawTexture(new Rect(screenPosition.x - healthBarLeft / 2,
        //        Screen.height - screenPosition.y - barTop,
        //        healthBarLength, healthBarHeight), healthTex);
        //        GUI.Label(new Rect(screenPosition.x - labelWidth / 2,
        //        Screen.height - screenPosition.y - labelTop,
        //        labelWidth, labelHeight), playerName, myStyle);
        if (Indicator > 0 && gameObject.tag == "Player")
        {
            if (Camera.current != null)
            {
                Vector3 screenPosition = Camera.current.WorldToScreenPoint(transform.position);// gets screen position.
                screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
                GUI.DrawTexture(new Rect(screenPosition.x - IndicatorOffsetX / 2, Screen.height - screenPosition.y - 10, Indicator, 25), texture);
            }
            //GUI.DrawTextureWithTexCoords(new Rect(screenPosition.x - IndicatorOffsetX / 2, Screen.height - screenPosition.y - 10, Indicator, 25), texture, new Rect(0, 0, 640, 640));
        }

    }
}

public enum IndicatorState
{
    Increment,
    Decrement
}