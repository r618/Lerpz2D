using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour
{

    void Update()
    {
        if (!gameObject.GetComponent<ParticleSystem>().IsAlive())
            Destroy(gameObject);
    }

    public void Stop()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
