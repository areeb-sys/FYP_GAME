using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyPlayer : KabadiPlayer
{
    public bool ischasing = false;
    public bool isRaiding = false;
    public bool isTouched = false;
    public NavMeshAgent agent;
    public Vector3 spawnpoint;


    public Transform target;

    private void Start()
    {
        spawnpoint = this.transform.position;
    }

    public void Raid()
    {
        isRaiding = true;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        agent.SetDestination(target.position);
    }

    public void RaidReturn()
    {
        Debug.Log("Raid Return");
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        agent.SetDestination(KabaddiGameManager.instance.CrossLine.transform.position);
    }

    public void ChaseFailed()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        ischasing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !KabaddiGameManager.instance.isChasing)
        {
            Debug.Log("Chasing...");
            target = other.transform;
            Invoke("Chase", 2f);
            KabaddiGameManager.instance.isChasing = true;
            KabaddiGameManager.instance.CrossLine.SetActive(true);
            KabaddiGameManager.instance.chasingAI = this;
        }
    }

    public void Chase()
    {
        ischasing = true;
    }

    private void Update()
    {
        if (ischasing)
        {
            animator.SetBool("isWalking", true);
            //Debug.Log("Animation started");

            agent.SetDestination(target.position);

            if (agent.remainingDistance <= agent.stoppingDistance && !grabbing)
            {
                // Stop moving and play idle animation
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                grabbing = true;
                animator.SetTrigger("Grab");
                KabaddiGameManager.instance.ShowTabPanel();
            }
        }
        if(isRaiding)
        {
            
            Debug.LogFormat("Target : {0} , Remaining Distance : {1}", target, agent.remainingDistance);
            if (!isTouched)
            {                
                if (agent.remainingDistance <= agent.stoppingDistance && !isTouched)
                {
                    // Stop moving and play idle animation
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", false);
                    animator.SetTrigger("Touch");
                    agent.isStopped = true;
                    isTouched = true;
                    isRaiding = false;
                    RaidReturn();
                }
            }

        }
    }

    public void ResetPlayer()
    {
        isRaiding = false;
        isTouched = false;
        ischasing = false;
        transform.position = spawnpoint;
        //transform.rotation = spawnpoint;
    }
    bool grabbing = false;
    
}
