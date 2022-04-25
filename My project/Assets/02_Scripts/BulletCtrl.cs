using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // Åº À§·Â
    public float damage = 20.0f;
    // Åº ¹ß»ç Èû
    public float force = 1500.0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * force);

    }


    void Update()
    {
        
    }
}
