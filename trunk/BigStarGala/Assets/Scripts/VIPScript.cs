using UnityEngine;

public class VIPScript : MonoBehaviour {

    private float timeSinceLastChecked;

    public float CoolDown;
    public bool IsChecked { get; set; }

	// Use this for initialization
	void Start () {
        timeSinceLastChecked = 0;
        IsChecked = false;
	}
	
	// Update is called once per frame
	void Update () {

        if(IsChecked)
            timeSinceLastChecked += Time.deltaTime;

        if (timeSinceLastChecked > CoolDown)
            IsChecked = false;
	}
}
