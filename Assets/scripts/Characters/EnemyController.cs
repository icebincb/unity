using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState { GUARD,PATROL,CHASE,DEAD}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour,IEndObserve
{
    private Collider col;
    private Quaternion guardRotation;
    private EnemyState enemyState;
    protected NavMeshAgent agent;
    private Animator anim;
    protected CharacterStats characterStats;
    private float attackCD;
    private float skillCD;
    [Header("Basic Settings")]
    private float speed;
    public bool isGuard;
    protected GameObject attackTarget;
    public float sightRadius;
    public float lookAtTime;
    private float remainlooAtTime;
    private float lastAttackTime;
    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 waypoint;
    private Vector3 guardpoint;
    //bool ��϶���
    private bool iswalk;
    private bool isChase;
    private bool isFollow;
    private bool isDead;
    public  bool playerDead;
    private void Awake()
    {   
       
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        speed = agent.speed;
        guardpoint = transform.position;
        remainlooAtTime = lookAtTime;
        characterStats = GetComponent<CharacterStats>();
        
        guardRotation = transform.rotation;
        isDead = false;
        col = GetComponent<Collider>();
        playerDead = false;
       
    }
    private void Start()
    {
        onEnable();
        attackCD = 0;
        skillCD = 0;

        characterStats.CurrentHealth = characterStats.MaxHealth;
        if (isGuard)
        {
            enemyState = EnemyState.GUARD;
        }
        else
        {
            enemyState = EnemyState.PATROL;
            GetNewWayPoint();
        }
        
    }
    //�л�����ʱ���� 
    void onEnable()
    {
        Gamemanager.Instance.addObserve(this);
    }

    private void OnDisable()
    {
        if (!Gamemanager.IsInit) return;
        Gamemanager.Instance.removeObserve(this);
//        Debug.Log("ִ�д���");
        if (GetComponent<LootSpawner>() && isDead)
        {
            
            GetComponent<LootSpawner>().SpawnLoot();
        }

    }

    private void Update()
    {
        if (characterStats.CurrentHealth <= 0)
            isDead = true;
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
        attackCD -= Time.deltaTime;
        attackCD = Mathf.Max(-1, attackCD);
        skillCD -= Time.deltaTime;
        skillCD = Mathf.Max(-1, skillCD);
    }

    void SwitchAnimation()
    {
        anim.SetBool("walk", iswalk);
        anim.SetBool("chase", isChase);
        anim.SetBool("follow", isFollow);
        anim.SetBool("critical", characterStats.isCritical);
        anim.SetBool("death", isDead);
    }
    public void SwitchStates()
    {
        if (isDead)
            enemyState = EnemyState.DEAD;
        //�������player �л���chase
        else 
        {   if (FoundPlayer())
            {
            enemyState = EnemyState.CHASE;
            //Debug.Log("zhaodao player");
            }
           
        }
       // Debug.Log(enemyState);
        switch (enemyState)
        {
            case EnemyState.GUARD:
                isChase = false;
                if(transform.position!= guardpoint)
                {
                    iswalk = true;
                    agent.isStopped = false;
                    agent.destination = guardpoint;
                    if (Vector3.SqrMagnitude(guardpoint - transform.position) <= agent.stoppingDistance)
                    {
                        iswalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.1f);
                    }
                }
                break;
            case EnemyState.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                //�ж��Ƿ��������
                if (Vector3.Distance(waypoint, transform.position) <= agent.stoppingDistance)
                {
                    iswalk = false;
                    if (remainlooAtTime > 0)
                        remainlooAtTime -= Time.deltaTime;
                    else 
                     GetNewWayPoint();
                }
                else
                {
                    iswalk = true;
                    agent.destination = waypoint;
                }
               
                break;
            case EnemyState.CHASE:
                //׷player

                
                //��϶���
                iswalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {//���ѻص���һ��״̬
                    isFollow = false;
                    if (remainlooAtTime > 0)
                    {
                        agent.destination = transform.position;
                        //Debug.Log(transform.position);
                        remainlooAtTime -= Time.deltaTime;
                    }
                    else if(isGuard)
                    {
                        enemyState = EnemyState.GUARD;
                    }
                    else
                    {
                        enemyState = EnemyState.PATROL;
                    }
                   

                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                //�ڹ�����Χ֮�ڹ���
                if(TargetInAttackRange()|| TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime <= 0)
                    {
                        lastAttackTime = characterStats.attackData_SO.attackCD;
                        //�����ж�
                        characterStats.isCritical = Random.value < characterStats.attackData_SO.criticalChance;
                        //ִ�й���
                        Attack();
                    }
                }
                break;
            case EnemyState.DEAD:
                //OnDisable();
                col.enabled = false;
                //agent.enabled = false;
                agent.radius = 0;
                Destroy(gameObject, 2f);
                break;
        }
    }

    private void Attack()
    {
        transform.LookAt(attackTarget.transform);
        //Debug.Log(" skillCD " + skillCD);
       //Debug.Log("attackCD" + attackCD);
        if (attackCD <= 0)
        { //�����빥������
            if (TargetInAttackRange())
            {
                attackCD = characterStats.attackData_SO.attackCD;
                anim.SetTrigger("attack");
            }
        }

        if (skillCD <= 0)
        {//���ܹ�������
            if (TargetInSkillRange())
            {
                skillCD = characterStats.attackData_SO.skillCD;
                anim.SetTrigger("skill");

            }
        }
        else 
        { 
            skillCD -= 1;
            skillCD = Mathf.Max(-1, skillCD);
        }
       
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position)<=characterStats.attackData_SO.attackRange;
         return false;
    }
    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData_SO.skillRange;
        return false;
    }
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach(var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
            return true;
            }
        }
        attackTarget = null;
        return false;
    }
    void GetNewWayPoint()
    {
        remainlooAtTime = lookAtTime;
        float randomx = Random.Range(-patrolRange, patrolRange);
        float randomz = Random.Range(-patrolRange, patrolRange);
        Vector3 randompoint =new Vector3(guardpoint.x + randomx, transform.position.y, guardpoint.z + randomz);
        //�ҵ����ƶ���Ѳ�ߵĵ�
        NavMeshHit hit;
        waypoint = NavMesh.SamplePosition(randompoint,out hit,patrolRange,1)?hit.position:transform.position;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    //Animation Event
    void Hit()
    {
        if (attackTarget != null&& transform.isFaceingTarget(attackTarget.transform))
        {
        var targetStats = attackTarget.GetComponent<CharacterStats>();

        targetStats.TakeDamage(characterStats, targetStats);
        }
        
    }

    public void EndNotify()
    {
        //��ʤ�Ķ���
        //ֹͣ�����ƶ�
        //ֹͣAgent
        playerDead = true;
        
        anim.SetBool("win", true);
        isChase = false;
        iswalk = false;
        attackTarget = null;
    }
}
