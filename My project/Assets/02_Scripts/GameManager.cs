using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    // 몬스터의 출현 위치를 저장하는 리스트 타입 변수
    public List<Transform> points = new List<Transform>();

    //몬스터를 미리 생성해 저장할 리스트 자료형
    public List<GameObject> monsterPool = new List<GameObject>();

    // 오브젝트 풀링에 생성할 몬스터의 최대 갯수
    public int maxMonsters = 10; 

    public GameObject monster;

    // 몬스터의 생성 간격
    public float createTime = 3.0f;

    // 게임 종료 여부를 저장할 멤버 변수
    private bool isGameOver;

    // 게임 종료 여부를 저장할 프로퍼티
    public bool IsGameOver
    {
        get { return isGameOver; }
        set {
            isGameOver = value;
            if(isGameOver)
            {
                CancelInvoke("CreateMonster");
            }
        }
    }

    // 싱글턴 인스턴스 선언
    public static GameManager instance = null;

    // 스크립트실행시 가장 먼저 호출될 이벤트 함수
    void Awake()
    {
        // instance가 할당되지 않을 경우
        if (instance == null)
        {
            instance = this;
        }
        // instance에 할당된 클래스의 인스턴스가 다를 경우 새로운 class를 의미함
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        // 다른 신으로 넘어가더라도 삭제하지 않고 유지함
        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        // 몬스터 오브젝트 풀 생성
        CreateMonsterPool();

        Transform spawnPointGroup = GameObject.Find("SpawnPointGroup")?.transform;

        //SPG의 모든 차일드 오브젝트로부터 TR추출
        //spawnPointGroup?.GetComponentsInChildren<Transform>(points);
        
        foreach(Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        // 일정 간격으로 함수 호출
        InvokeRepeating("CreateMonster", 2.0f, createTime);
    }

    void CreateMonster()
    {
        // 몬스터의 생성위치를 불규칙하게 산출
        int idx = Random.Range(0, points.Count);
        // 몬스터 프리팹 생성
        //Instantiate(monster, points[idx].position, points[idx].rotation);
        
        // 오브젝트 풀로부터 몬스터 추출
        GameObject _monster = GetMonsterInPool();
        // 추출 몬스터의 위치 회전 설정
        _monster?.transform.SetPositionAndRotation(points[idx].position, points[idx].rotation);

        // 추출 몬스터 활성화 
        _monster?.SetActive(true);

    }
    
    void CreateMonsterPool()
    {
        for( int i=0; i < maxMonsters; i++)
        {
            // 몬스터 생성
            var _monster = Instantiate<GameObject>(monster);
            // 몬스터 이름 지정
            _monster.name = $"Monster_{i:00}";
            // 몬스터 비활성화
            _monster.SetActive(false);

            // 생성한 몬스터를 오브젝트 풀에 추가한다.
            monsterPool.Add(_monster);

        }
    }
    public GameObject GetMonsterInPool()
    {
        // 오브젝트 풀을 처음부터 끝까지 순회
        foreach (var _monster in monsterPool)
        {
            if ( _monster.activeSelf == false)
            {
                // 몬스터 반환
                return _monster;
            }

        }
        return null;

    }


    
}
