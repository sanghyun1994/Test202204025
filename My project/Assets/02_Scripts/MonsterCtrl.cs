using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;       // 네비 기능을 사용하기위해 추가할 네임 스페이스

public class MonsterCtrl : MonoBehaviour


{
    public enum State
    { 
        IDLE,
        TRACE,
        ATTACK,
        DIE

    }

    // 몬스터의 현재 상태
    public State state = State.IDLE;
    // 추적 사정 거리
    public float traceDist = 10.0f;
    // 공격 사정 거리
    public float attackDist = 2.0f;
    // 몬스터의 사망 여부
    public bool isDie = false;

    // 컴포넌트 캐시 변수
    [SerializeField] private Transform monsterTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    // animator 파라미터의 해시값 추출
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

        // 몬스터의 상태를 체크하는 코루틴 호출
        StartCoroutine(CheckMonsterState());
        // 상태에 따라 몬스터의 행동을 수행하는 코루틴 호출
        StartCoroutine(MonsterAction());

    }

    void OnDisable()
    {
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
        
    }

    void Awake()
    {
        monsterTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();      // 추적 대상 tr할당
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");               // 블러드 스프레이 이펙트 프리팹 로드 

    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {

            yield return new WaitForSeconds(0.3f);      // 0.3초 동안 중지하는 동안 제어권을 메시지루프로 양보한다

            if (state == State.DIE) yield break;        // 몬스터가 die상태일 경우 코루틴을 중지한다

            float distance = Vector3.Distance(playerTr.position, monsterTr.position);

            // 공격 사정범위로 들어왔는지 체크
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            // 추적 사정범위로 들어왔는지 체크
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

    // 몬스터의 상태에 따라 몬스터의 행동 변화
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE 상태
                case State.IDLE:
                    agent.isStopped = true;     // 추적 중지
                    anim.SetBool(hashTrace, false);

                    break;

                // 추적 상태
                case State.TRACE:
                    agent.SetDestination(playerTr.position);    // 추적 대상의 좌표로 이동
                    agent.isStopped = false;

                    anim.SetBool(hashTrace, true);

                    anim.SetBool(hashAttack, false);

                    break;

                // 공격 상태
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;

                // 사망
                case State.DIE:
                    isDie = true;

                    agent.isStopped = true;                             // 추적 중지

                    anim.SetTrigger(HashDie);

                    GetComponent<CapsuleCollider>().enabled = false;    // 몬스터의 collider 컴포 비활성화

                    // 일정 시간 대기 후 오브젝트 풀링으로 환원
                    yield return new WaitForSeconds(3.0f);

                    // 사망 후 다시 사용할 때를 위해 hp값 초기화
                    hp = 100;
                    isDie = false;

                    // 몬스터의 collider 컴포 재활성
                    GetComponent<CapsuleCollider>().enabled = true;
                    // 몬스터 비활성
                    this.gameObject.SetActive(false);

                    break;
            }
            yield return new WaitForSeconds(0.3f);

        }


    }


    void OnDrawGizmos()
        {
            // 추적 사정거리 표시
            if (state == State.TRACE)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, traceDist);
            }

            // 공격 사정거리 표시
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
            // 충돌한 총알 삭제
            Destroy(coll.gameObject);
            // 피격 애니메이션 실행
            anim.SetTrigger(hashHit);

            // 총알의 충돌 지점
            Vector3 pos = coll.GetContact(0).point;
            // 충돌 지점의 법선 벡터
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
        // 몬스터의 상태를 체크하는 코루틴 함수를 모두 정지
        StopAllCoroutines();

        // 추적을 중지하고 애니메이션 수행
        agent.isStopped = true;

        anim.SetTrigger(HashPlayerDie);


    }



}
 
