using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public float timeBetweenSpawns;

    void Start()
    {
        InvokeRepeating("Spawn", 0.01f, timeBetweenSpawns);
    }
    void Spawn()
    {
        Instantiate(enemy, transform.position, transform.rotation);
    }
}
