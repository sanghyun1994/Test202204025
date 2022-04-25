using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            // 충돌 지점 추측 (0)이란 접점이 하나 있다는 소리
            ContactPoint cp = coll.GetContact(0);
            // 탄의 법선벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // 스파크 파티클의 동적 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            Destroy(spark, 0.5f);

            Destroy(coll.gameObject);
        }
    }
}
