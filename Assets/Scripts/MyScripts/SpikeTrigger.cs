using UnityEngine;
using System.Collections;

public class SpikeTrigger : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        transform.FindChild("SpikeBall").SendMessage("OnTriggerEnter", other, SendMessageOptions.RequireReceiver);
    }
}
