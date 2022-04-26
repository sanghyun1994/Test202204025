using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �ݵ�� �ʿ��� ������Ʈ�� ǥ���� �ش� ������Ʈ�� ������� ���� ������
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    
    // �Ѿ� ������(�ݵ�� ������ ��ǥ�� ������ ��. null ���� ����)
    public GameObject bullet;
    // �Ѿ� �߻� ��ǥ
    public Transform firepos;
    // �ѼҸ� ����
    public AudioClip fireSfx;

    private new AudioSource audio;
    private MeshRenderer muzzleFlash;

    private RaycastHit hit;


    void Start()
    {
        audio = GetComponent<AudioSource>();

        // firepos������ muzzleflash�� meterial����
        muzzleFlash = firepos.GetComponentInChildren<MeshRenderer>();
        // ù ���� ��Ȱ��ȭ
        muzzleFlash.enabled = false;
    }

    
    void Update()
    {
        // ����ĳ��Ʈ�� �ð������� ǥ���ϱ� ���� ���
        Debug.DrawRay(firepos.position, firepos.forward * 10.0f, Color.green);


        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            // ���̸� ��
            if (Physics.Raycast(firepos.position, firepos.forward, out hit, 10.0f, 1 << 6))
            {
                Debug.Log($"HIT={hit.transform.name}");
                hit.transform.GetComponent<MonsterCtrl>()?.OnDamage(hit.point, hit.normal);

            }

        }



    }

    void Fire()
    {
        
        Instantiate(bullet, firepos.position, firepos.rotation);

        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
        
    }

    IEnumerator ShowMuzzleFlash()
    {

        // ������ ��ǥ���� �����Լ��� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        // �ؽ����� ������ �� ����
        muzzleFlash.material.mainTextureOffset = offset;

        // muzzleflash ȸ�� �ݰ�
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // muzzleflash ũ�� ����
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;


    }




}
