using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyPlayer : KabadiPlayer
{
    public bool ischasing = false;
    public NavMeshAgent agent;

    public Transform target;

    public void Raid()
    {
        Debug.Log("Raiding....");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !KabaddiGameManager.instance.isChasing)
        {
            Debug.Log("Chasing...");
            target = other.transform;
            ischasing = true;
            KabaddiGameManager.instance.isChasing = true;
            KabaddiGameManager.instance.CrossLine.SetActive(true);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Chasing...");
    //        target = collision.transform;
    //        ischasing = true;
    //    }
    //}

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

    }

    void Grab()
    {
        
    }
    bool grabbing = false;
    //IEnumerator Grab()
    //{
    //    Debug.Log("Grab Him WAit ...");
    //    yield return new WaitForSeconds(0.2f);
    //    if (agent.remainingDistance <= agent.stoppingDistance)
    //    {
    //        //grabbing = true;
    //        Debug.Log("Grab Him ...");
    //        animator.SetBool("Grab", true);
    //    }
    //}
}
