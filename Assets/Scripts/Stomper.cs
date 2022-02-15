using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour
{
    private Rigidbody2D theRD2D;
    public float bounceForce;

    // Start is called before the first frame update
    void Start()
    {
        theRD2D = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<TrapObject>().Die();
            theRD2D.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
