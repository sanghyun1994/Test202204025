using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    //������Ʈ�� ĳ��ó��ȭ�� ����
    [SerializeField] private Transform tr;
    //�ִϸ��̼� ����
    [SerializeField] private Animation ani;
    //�̵� �ӵ�
    public float moveSpeed = 10.0f;
    //ȸ�� �ӵ�
    public float rotSpeed = 80.0f;

    // �ʱ� HP��
    private readonly float initHp = 100.0f;
    // ���� HP��
    public float currHp;
    // HPBAR
    private Image hpBar;

    // ��������Ʈ �� �̺�Ʈ ����
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        // hp�� ����
        hpBar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        // hp�� �ʱ�ȭ
        currHp = initHp;
        

        tr = GetComponent<Transform>();
        ani = GetComponent<Animation>();

        ani.Play("Idle");

        rotSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        rotSpeed = 80.0f;
    }

    
    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        //Debug.Log("h=" + h);
        //Debug.Log("v=" + v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        PlayerAni(h, v);

    }

    void PlayerAni(float h, float v)
    {
        if( v>=0.1f)
        {
            ani.CrossFade("RunF", 0.25f);
        }
        else if (v<= -0.1f)
        {
            ani.CrossFade("RunB", 0.25f);
        }
        else if ( h>= 0.1f)
        {
            ani.CrossFade("RunR", 0.25f);
        }
        else if (h <= -0.1f)
        {
            ani.CrossFade("RunL", 0.25f);
        }
        else
            ani.CrossFade("Idle", 0.25f);

    }

    void OnTriggerEnter(Collider coll)
    {
        // �浹�� collider�� ������ punch�� �ش��� ��� player hp����
        if (currHp >= 0.0f && coll.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            DisplayHealth();


            // plyaer hp�� 0�� �Ǿ��� ��� ��� ó��
            if (currHp <= 0.0f)
            {
                PlayerDie();
            }

        }

    }
    void PlayerDie()
    {
        Debug.Log("�÷��̾ ����߽��ϴ� !!");

        OnPlayerDie();

        GameManager.instance.IsGameOver = true;

    }

    void DisplayHealth()
    {
        hpBar.fillAmount = currHp / initHp;
    }

}
