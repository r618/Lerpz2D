using UnityEngine;
using System.Collections;

public class SpikeTrigger : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        transform.Find("SpikeBall").SendMessage("OnTriggerEnter", other, SendMessageOptions.RequireReceiver);
    }
}
