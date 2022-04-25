using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 추적 대상 변수
    public Transform targetTr;

    // 카메라 tf 
    private Transform camTr;

    // 추적 대상으로부터의 거리
    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    // y축 높이
    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    // 반응 속도
    public float damping = 10.0f;

    // lookoffset값
    public float targetoffset = 2.0f;

    // smoothdamp에서 사용할 변수
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // 카메라 tr컴포 추출
        camTr = GetComponent<Transform>();
    }


    void LateUpdate()
    {
        Vector3 pos = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        // 선형보간사용
        //camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);

        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        camTr.LookAt(targetTr.position + (targetTr.up * targetoffset));
    }

}
