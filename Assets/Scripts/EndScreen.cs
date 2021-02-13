using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Application.LoadLevel("EndScreen");
        }
    }
}
