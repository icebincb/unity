using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class characterController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;
    private float lastAttacktime=0.3f;
    private CharacterStats characterStats;
    private bool isDead;
    private float stopDistance;
    private void Awake()
    {
       // Gamemanager.Instance.RegisterPlayer(characterStats);
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        stopDistance = agent.stoppingDistance;
        isDead = false;
        //Gamemanager.Instance.RegisterPlayer(characterStats);
        // 
        
    }

    private void OnEnable()
    {
        Mousemanage.Instance.OnMouseClicked += MoveToTarget;
        Mousemanage.Instance.OnEnemyClicked += EventAttack;

        Gamemanager.Instance.RegisterPlayer(characterStats);
    }

    private void Start()
    {
       
        //characterStats.charactorData.currentHP = characterStats.charactorData.maxHP;
       // Gamemanager.Instance.RegisterPlayer(characterStats);
        /*Mousemanage.Instance.OnMouseClicked += MoveToTarget;
        Mousemanage.Instance.OnEnemyClicked += EventAttack;

        Gamemanager.Instance.RegisterPlayer(characterStats);*/
        Savemanager.Instance.LoadPlayerData();
        if (GameObject.FindGameObjectWithTag("Player")!= this.gameObject)
            Destroy(GameObject.FindGameObjectWithTag("Player"));

    }

    void OnDisable()
    {
        Mousemanage.Instance.OnMouseClicked -= MoveToTarget;
        Mousemanage.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth <= 0;
        if (isDead)
        {
           
            Gamemanager.Instance.NotifyObservers();
        }
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        lastAttacktime -= Time.deltaTime;
        SelectAnimation();
    }
    private void SelectAnimation()
    {
        anim.SetFloat("Speed",agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }
    public void MoveToTarget(Vector3 target)
    {
        if (isDead) return;
        StopAllCoroutines();
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }
    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if(target!=null)
        {
            attackTarget = target;
            characterStats.isCritical = UnityEngine.Random.value<characterStats.attackData_SO.criticalChance;
            StartCoroutine("MoveToAttackTarget");

        }
    }
    IEnumerator MoveToAttackTarget()
    {
        
        agent.isStopped = false;
        agent.stoppingDistance = characterStats.attackData_SO.attackRange;
        transform.LookAt(attackTarget.transform);

        while (Vector3.Distance(attackTarget.transform.position, transform.position)>characterStats.attackData_SO.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;//下一帧再次执行上面的命令
        }
        agent.isStopped = true;
        //Attack
        if (lastAttacktime < 0)
        {
            anim.SetBool("Critical", characterStats.isCritical);
            anim.SetTrigger("Attack");
            //冷却时间重置
            lastAttacktime = characterStats.attackData_SO.attackCD;
        }
    }
    //Animation Event
     void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
           // Debug.Log(attackTarget);
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20,ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
       
    }
}
