using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Rock : MonoBehaviour
{
    public enum RockStates
    {
        HitPlayer,
        HitEnemy,
        HitNothing
    }
    public RockStates rockStates;
    public Rigidbody rb;
    public GameObject breakEffect;
    [Header("Basic Setting")]
    public float damage;
    public float force;
    public GameObject target;
    private Vector3 direction;
    private void Start()
    {
        rockStates = RockStates.HitPlayer;
        
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        FlyToTarget();
       
    }
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude<1)
        {
            rockStates = RockStates.HitNothing;

        }
    }
    void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<characterController>().gameObject;
        direction = (target.transform.position - transform.position+Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (rockStates ==RockStates.HitPlayer)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                collision.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                collision.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, collision.gameObject.GetComponent<CharacterStats>());
                rockStates = RockStates.HitNothing;
                  
            }
        }
        else if (rockStates ==RockStates.HitEnemy)
        {
            if (collision.gameObject.GetComponent<Golem>())
            {
                var otherStates = collision.gameObject.GetComponent<CharacterStats>();
                otherStates.TakeDamage(damage, otherStates);
                Instantiate(breakEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
