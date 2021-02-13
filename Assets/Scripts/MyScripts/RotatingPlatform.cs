using UnityEngine;

public class RotatingPlatform : MonoBehaviour {

    public float RotationSpeed;
	
	// Update is called once per frame
	void Update () 
    {
        if (GameData.Instance.Paused)
            return;
        transform.Rotate(Vector3.back, RotationSpeed * Time.deltaTime);   
	}
}
