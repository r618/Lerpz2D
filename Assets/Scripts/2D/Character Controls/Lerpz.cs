using UnityEngine;

public class Lerpz : MonoBehaviour
{

    void OnDeath()
    {
        GameData.Instance.Deaths += 1; 
        gameObject.GetComponent<PlatformerController>().Spawn();
    }
}
