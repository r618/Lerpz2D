using UnityEngine;
using System.Collections;

public class SpikeBallMovement : MonoBehaviour
{

    public float attackSpeed;
    public float resetSpeed;
    Vector3 resetPosition;
    bool attack = false;
    bool reset = false;

    void Start()
    {
        resetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack && transform.position.y > resetPosition.y)
        {
            attack = false;
            reset = false;
            GetComponent<Rigidbody>().AddForce(Vector3.down * resetSpeed);
            transform.position = resetPosition;
        }
        if (attack && !reset && transform.localPosition.y < 0)
        {
            reset = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(Vector3.up * (resetSpeed));
            GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !attack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            GetComponent<Rigidbody>().AddForce(Vector3.down * attackSpeed);
            attack = true;
        }
    }

    void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
