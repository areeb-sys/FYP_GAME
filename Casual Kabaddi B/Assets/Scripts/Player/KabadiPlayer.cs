using UnityEngine;

public class KabadiPlayer : MonoBehaviour
{
    public string playerName;
    public float speed;
    public Animator animator;

    public CharacterController _characterController;

    private bool isDodge;
    private bool isTouch;
    public Vector3 spawnpoint;
    public Quaternion spawnRotation;

    private void Start()
    {
        spawnpoint = this.transform.position;
        spawnRotation = this.transform.rotation;
    }

    public void Walk(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }

    public void Grab(bool isGrab)
    {
        animator.SetBool("isGrab", isGrab);
    }

    public void IsDodging()
    {
        isDodge = true;
        if (isDodge)
        {
            animator.SetBool("isDodging", true);
        }
        else
            animator.SetBool("isDodging", false);

    }

    public void IsTouching()
    {
        Debug.Log("Inside IsTouching");
        //animator.SetBool("isTouching", true);
        animator.SetTrigger("Touch");
    }

    public void ResetPosition()
    {
        transform.position = spawnpoint;
        transform.rotation = spawnRotation;
    }
}
