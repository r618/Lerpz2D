using UnityEngine;
using System.Collections;

public class NailSpikeEnemy : Enemy
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Instantiate(onDeathScript, transform.position, transform.rotation);
        }
    }
}
