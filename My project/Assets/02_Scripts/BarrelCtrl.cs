using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // 폭발 파티클
    public GameObject expEffect;
    // 랜덤으로 적용할 텍스처 배열
    public Texture[] textures;
    // mesh renderer 지정 변수
    [SerializeField] private new MeshRenderer renderer;

    private Transform tr;
    private Rigidbody rb;

    private int hitCount = 0;       // 누적 총탄 횟수
    public float radius = 10.0f;    // 폭발 반경



    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // 하위 mesh renderer 컴포 추출
        renderer = GetComponentInChildren<MeshRenderer>();

        // 난수 발생
        int idx = Random.Range(0, textures.Length);
        // 텍스처 지정
        renderer.material.mainTexture = textures[idx];
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        { 
            //누적 총탄 횟수를 증가시키고 3회 이상일 경우 폭파 처리한다.
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

    // 폭발력을 주위로 분산시키는 함수
    void IndirectDamage(Vector3 pos)
    {
        // 주변 물체의 추출
        Collider[] colls = Physics.OverlapSphere(pos, radius, 1 << 3);

        foreach (var coll in colls)
        {
            // 폭발 범위에 포함된 barrel의 rb컴포 추출
            rb = coll.GetComponent<Rigidbody>();

            rb.mass = 1.0f;
            rb.constraints = RigidbodyConstraints.None;     //FreezeRotation 해제
            rb.AddExplosionForce(1500.0f, pos, radius, 1200.0f);
 

        }

    
    
    
    
    
    
    }










}
