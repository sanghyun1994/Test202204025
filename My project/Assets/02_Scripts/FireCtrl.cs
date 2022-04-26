using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 반드시 필요한 컴포넌트를 표기해 해당 컴포넌트가 사라지는 것을 방지함
[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{
    
    // 총알 프리팹(반드시 프리팹 좌표에 연결할 것. null 오류 방지)
    public GameObject bullet;
    // 총알 발사 좌표
    public Transform firepos;
    // 총소리 음원
    public AudioClip fireSfx;

    private new AudioSource audio;
    private MeshRenderer muzzleFlash;

    private RaycastHit hit;


    void Start()
    {
        audio = GetComponent<AudioSource>();

        // firepos하위의 muzzleflash의 meterial추출
        muzzleFlash = firepos.GetComponentInChildren<MeshRenderer>();
        // 첫 시작 비활성화
        muzzleFlash.enabled = false;
    }

    
    void Update()
    {
        // 레이캐스트를 시각적으로 표시하기 위해 사용
        Debug.DrawRay(firepos.position, firepos.forward * 10.0f, Color.green);


        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            // 레이를 쏨
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

        // 오프셋 좌표값을 랜덤함수로 생성
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2) * 0.5f);
        // 텍스쳐의 오프셋 값 설정
        muzzleFlash.material.mainTextureOffset = offset;

        // muzzleflash 회전 반경
        float angle = Random.Range(0, 360);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // muzzleflash 크기 조절
        float scale = Random.Range(1.0f, 2.0f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        muzzleFlash.enabled = true;

        yield return new WaitForSeconds(0.2f);

        muzzleFlash.enabled = false;


    }




}
