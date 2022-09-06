using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform targetObject;
    public float fireRate = 1f;
    private float nextFire = 0.0f;
    public string tagObject;
    public GameObject enemyGun,enemyBullet,muzzle;
    Animator animator;
    NavMeshAgent navAgent;

    void Start()
    {
        muzzle.SetActive(false);
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        if (targetObject == null)
            targetObject = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        navAgent.destination = targetObject.position;
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            animator.SetBool("Attack", true);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                muzzle.SetActive(true);
                StartCoroutine(MuzzleOff(0.2f));
                Instantiate(enemyBullet, enemyGun.transform.position, enemyGun.transform.rotation);
            }
            
        }else{
            animator.SetBool("Attack", false);
        }
    }

    IEnumerator MuzzleOff(float secondUntildestroy) {
        yield return new WaitForSeconds(secondUntildestroy);
        muzzle.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == tagObject)
        {
            animator.SetTrigger("Death");
            Destroy(collision.gameObject);
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.isStopped = true;
            StartCoroutine(clearObject());
        }
    }

    IEnumerator clearObject()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

}
