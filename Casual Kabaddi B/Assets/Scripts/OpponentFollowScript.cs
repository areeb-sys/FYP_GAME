using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OpponentFollowScript : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        agent.destination = target.transform.position;
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
            target = GameObject.FindGameObjectWithTag("Player");
    }
}
