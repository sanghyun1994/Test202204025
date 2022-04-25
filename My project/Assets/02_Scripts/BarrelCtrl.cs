using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // ���� ��ƼŬ
    public GameObject expEffect;
    // �������� ������ �ؽ�ó �迭
    public Texture[] textures;
    // mesh renderer ���� ����
    [SerializeField] private new MeshRenderer renderer;

    private Transform tr;
    private Rigidbody rb;

    private int hitCount = 0;       // ���� ��ź Ƚ��
    public float radius = 10.0f;    // ���� �ݰ�



    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // ���� mesh renderer ���� ����
        renderer = GetComponentInChildren<MeshRenderer>();

        // ���� �߻�
        int idx = Random.Range(0, textures.Length);
        // �ؽ�ó ����
        renderer.material.mainTexture = textures[idx];
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        { 
            //���� ��ź Ƚ���� ������Ű�� 3ȸ �̻��� ��� ���� ó���Ѵ�.
            if (++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    void ExpBarrel()
    {
        GameObject exp = Instantiate(expEffect, tr.position, Quaternion.identity);
        Destroy(exp, 5.0f);

        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 1500.0f);

        IndirectDamage(tr.position);

        Destroy(gameObject, 3.0f);

    }

    // ���߷��� ������ �л��Ű�� �Լ�
    void IndirectDamage(Vector3 pos)
    {
        // �ֺ� ��ü�� ����
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach (var coll in colls)
        {
            // ���� ������ ���Ե� barrel�� rb���� ����
            rb = coll.GetComponent<Rigidbody>();

            rb.mass = 1.0f;
            rb.constraints = RigidbodyConstraints.None;     //FreezeRotation ����
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
 

        }

    
    
    
    
    
    
    }










}
