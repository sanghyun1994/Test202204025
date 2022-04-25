using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // ���� ��� ����
    public Transform targetTr;

    // ī�޶� tf 
    private Transform camTr;

    // ���� ������κ����� �Ÿ�
    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    // y�� ����
    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    // ���� �ӵ�
    public float damping = 10.0f;

    // lookoffset��
    public float targetoffset = 2.0f;

    // smoothdamp���� ����� ����
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // ī�޶� tr���� ����
        camTr = GetComponent<Transform>();
    }


    void LateUpdate()
    {
        Vector3 pos = targetTr.position + (-targetTr.forward * distance) + (Vector3.up * height);
        // �����������
        //camTr.position = Vector3.Slerp(camTr.position, pos, Time.deltaTime * damping);

        camTr.position = Vector3.SmoothDamp(camTr.position, pos, ref velocity, damping);

        camTr.LookAt(targetTr.position + (targetTr.up * targetoffset));
    }

}
