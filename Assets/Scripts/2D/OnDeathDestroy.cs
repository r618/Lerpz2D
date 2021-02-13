using UnityEngine;

public class OnDeathDestroy : MonoBehaviour
{
    void OnDeath()
    {
        Destroy(gameObject);
    }
}