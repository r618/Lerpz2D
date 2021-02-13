using UnityEngine;
using System.Collections;

public class FallingPlatformVertical : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            GetComponent<Collider>().enabled = false;
        }
    }
}
