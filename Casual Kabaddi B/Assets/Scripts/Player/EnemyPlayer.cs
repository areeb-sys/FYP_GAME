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
    


    public Transform target;

    

    public void Raid()
    {
        isRaiding = true;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        agent.SetDestination(target.position);
        agent.stoppingDistance = 1f;
    }

    public void RaidReturn()
    {
        Debug.Log("Raid Return");
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false);
        //agent.SetDestination(null);
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(KabaddiGameManager.instance.CrossLine.transform.position);

        KabaddiGameManager.instance.ControlPanel.SetActive(true);
        KabaddiGameManager.instance.currentPlayer.canMove = true;
    }

    
    public void RaidFailed()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        agent.isStopped = true;
        isRaiding = false;
    }

    public void ChaseFailed()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        agent.isStopped = true;
        ischasing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Chase the Player
        if (other.gameObject.tag == "Player" && KabaddiGameManager.instance.isPlayerRaiding)
        {
            Debug.Log("Chasie the player...");
            target = other.transform;
            Invoke("Chase", 1f);
            KabaddiGameManager.instance.CrossLine.SetActive(true);
            KabaddiGameManager.instance.chasingAI = this;
        }

        if(other.gameObject.tag == "Middle Point" && isRaiding)
        {
            Debug.Log("AI Raid Complete!");
            KabaddiGameManager.instance.RaidComplete();
        }

        if(other.gameObject.tag == "Player" && isTouched && isRaiding)
        {
            KabaddiGameManager.instance.grabButton.SetActive(true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && isTouched && isRaiding)
        {
            KabaddiGameManager.instance.grabButton.SetActive(false);
        }
    }

    public void Chase()
    {
        ischasing = true;
        animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        if (ischasing)
        {
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
            if (!isTouched)
            {                
                if (agent.remainingDistance <= agent.stoppingDistance && !isTouched)
                {
                    // Stop moving and play idle animation
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", false);
                    animator.SetTrigger("Touch");
                    //agent.isStopped = true;
                    isTouched = true;
                    //isRaiding = false;
                    RaidReturn();
                }
            }
            else if(agent.remainingDistance <= agent.stoppingDistance)
            {
                KabaddiGameManager.instance.RaidComplete();
            }
        }
    }

    public void ResetPlayer()
    {
        isRaiding = false;
        isTouched = false;
        ischasing = false;
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
        //transform.rotation = spawnpoint;
        ResetPosition();
    }
    bool grabbing = false;

    
    
}
