using UnityEngine;
using System.Collections;

public class TransparentTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        transform.parent.SendMessage("OnTriggerEnter", collider);
    }
}
