using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ź ����
    public float damage = 20.0f;
    // ź �߻� ��
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
