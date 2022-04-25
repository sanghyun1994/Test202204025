using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;       // �׺� ����� ����ϱ����� �߰��� ���� �����̽�

public class MonsterCtrl : MonoBehaviour


{
    public enum State
    { 
        IDLE,
        TRACE,
        ATTACK,
        DIE

    }

    // ������ ���� ����
    public State state = State.IDLE;
    // ���� ���� �Ÿ�
    public float traceDist = 10.0f;
    // ���� ���� �Ÿ�
    public float attackDist = 2.0f;
    // ������ ��� ����
    public bool isDie = false;

    // ������Ʈ ĳ�� ����
    [SerializeField] private Transform monsterTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    // animator �Ķ������ �ؽð� ����
    [SerializeField] private readonly int hashTrace = Animator.StringToHash("IsTrace");
    [SerializeField] private readonly int hashAttack = Animator.StringToHash("IsAttack");
    [SerializeField] private readonly int hashHit = Animator.StringToHash("Hit");
    [SerializeField] private readonly int HashPlayerDie = Animator.StringToHash("PlayerDie");
    [SerializeField] private readonly int HashDie = Animator.StringToHash("Die");


    [SerializeField] private GameObject bloodEffect;

    private int hp = 100;

    void OnEnable()
    {
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ ȣ��
        StartCoroutine(CheckMonsterState());
        // ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ�ƾ ȣ��
        StartCoroutine(MonsterAction());

    }

    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        
    }

    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();      // ���� ��� tr�Ҵ�
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");               // ���� �������� ����Ʈ ������ �ε� 

    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {

            yield return new WaitForSeconds(0.3f);      // 0.3�� ���� �����ϴ� ���� ������� �޽��������� �纸�Ѵ�

            if (state == State.DIE) yield break;        // ���Ͱ� die������ ��� �ڷ�ƾ�� �����Ѵ�

            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            // ���� ���������� ���Դ��� üũ
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            // ���� ���������� ���Դ��� üũ
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

        }

    }

    // ������ ���¿� ���� ������ �ൿ ��ȭ
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE ����
                case State.IDLE:
                    agent.isStopped = true;     // ���� ����
                    anim.SetBool(hashTrace, false);

                    break;

                // ���� ����
                case State.TRACE:
                    agent.SetDestination(playerTr.position);    // ���� ����� ��ǥ�� �̵�
                    agent.isStopped = false;

                    anim.SetBool(hashTrace, true);

                    anim.SetBool(hashAttack, false);

                    break;

                // ���� ����
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;

                // ���
                case State.DIE:
                    isDie = true;

                    agent.isStopped = true;                             // ���� ����

                    anim.SetTrigger(HashDie);

                    GetComponent<CapsuleCollider>().enabled = false;    // ������ collider ���� ��Ȱ��ȭ

                    // ���� �ð� ��� �� ������Ʈ Ǯ������ ȯ��
                    yield return new WaitForSeconds(3.0f);

                    // ��� �� �ٽ� ����� ���� ���� hp�� �ʱ�ȭ
                    hp = 100;
                    isDie = false;

                    // ������ collider ���� ��Ȱ��
                    GetComponent<CapsuleCollider>().enabled = true;
                    // ���� ��Ȱ��
                    this.gameObject.SetActive(false);

                    break;
            }
            yield return new WaitForSeconds(0.3f);

        }


    }


    void OnDrawGizmos()
        {
            // ���� �����Ÿ� ǥ��
            if (state == State.TRACE)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, traceDist);
            }

            // ���� �����Ÿ� ǥ��
            if (state == State.ATTACK)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, traceDist);

            }

        }

    void OnCollisionEnter(Collision coll)
    {
    
        if(coll.collider.CompareTag("BULLET"))
        {
            // �浹�� �Ѿ� ����
            Destroy(coll.gameObject);
            // �ǰ� �ִϸ��̼� ����
            anim.SetTrigger(hashHit);

            // �Ѿ��� �浹 ����
            Vector3 pos = coll.GetContact(0).point;
            // �浹 ������ ���� ����
            Quaternion rot = Quaternion.LookRotation(-coll.GetContact(0).normal);
            ShowBloodEffect(pos, rot);

            hp -= 50;
            if (hp <= 0)
            {
                state = State.DIE;
            }
        }
        
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, 1.0f);

    }
    
    void OnPlayerDie()
    {
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ��� ��� ����
        StopAllCoroutines();

        // ������ �����ϰ� �ִϸ��̼� ����
        agent.isStopped = true;

        anim.SetTrigger(HashPlayerDie);


    }



}
 
