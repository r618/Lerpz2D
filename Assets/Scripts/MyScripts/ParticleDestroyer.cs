using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour
{

    void Update()
    {
        if (!gameObject.particleSystem.IsAlive())
            Destroy(gameObject);
    }

    public void Stop()
    {
        gameObject.particleSystem.Stop();
    }
}
