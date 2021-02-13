using UnityEngine;
using System.Collections;

public class OnDeathDestroy : MonoBehaviour {

    void OnDeath()
    {
        DestroyObject(gameObject);
    }
}
