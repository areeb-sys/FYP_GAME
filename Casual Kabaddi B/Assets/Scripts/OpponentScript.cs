using System.Collections;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    public float followSpeed = 10f;
    private Transform playerTransform;
    private Vector3 direction;
    private bool isFollowingPlayer = false;
    public Animator animator;

    private void Start()
    {
       animator = gameObject.GetComponent<Animator>();
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (isFollowingPlayer)
        {
           animator.SetBool("isMoving", true);
            direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * followSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isFollowingPlayer = true;
            
        }
    }
}
