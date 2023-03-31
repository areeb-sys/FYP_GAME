using UnityEngine.UI;
using UnityEngine;

public class managCharCon : MonoBehaviour
{
    //character controller componenet
    private CharacterController _characterController;
    public Animator animator;
    private joystickManager joystick;

    //rotation
    private Transform meshPlayer;

    //movement
    private Vector3 v_movement;
    public float maxSpeed = .1f; // max speed the player can move
    private float gravity;
    private bool isDodge;
    private bool isTouch;
    public Button dodge;

    // Start is called before the first frame update
    void Start()
    {
        gravity = 0.5f;
        GameObject tempPlayr = GameObject.FindGameObjectWithTag("Player");
        meshPlayer = tempPlayr.transform.GetChild(0);
        _characterController = tempPlayr.GetComponent<CharacterController>();
        animator = meshPlayer.GetComponent<Animator>();
        joystick = GameObject.Find("imgJoystickBg").GetComponent<joystickManager>();

    }

    // Update is called once per frame
    void Update()
    {
        float inputX = joystick.inputHorizontal();
        float inputZ = joystick.inputVertical();

        if (inputX == 0f && inputZ == 0f)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    private void FixedUpdate()
    {
        if (_characterController.isGrounded)
        {
            v_movement.y = 0f;
        }
        else
        {
            v_movement.y -= gravity * Time.deltaTime;
        }

        float inputX = joystick.inputHorizontal();
        float inputZ = joystick.inputVertical();

        v_movement = new Vector3(inputX, v_movement.y, inputZ).normalized;

        if (v_movement != Vector3.zero)
        {
            // Calculate the speed based on the joystick position

            _characterController.Move(v_movement * Time.deltaTime);

            // Rotate the meshPlayer based on the movement direction
            Vector3 lookDir = new Vector3(v_movement.x, 0, v_movement.z);
            meshPlayer.rotation = Quaternion.LookRotation(lookDir);
        }
    }

    public void IsDodging()
    {
        isDodge = true;
        if (isDodge)
        {
            Debug.Log("Inside IsDodgeing");
            animator.SetBool("isDodging", true);
        }
        else
            animator.SetBool("isDodging", false);

    }
    public void IsTouching()
    {
        isTouch = true;
        if (isTouch)
        {
            Debug.Log("Inside IsTouching");
            animator.SetBool("isTouching", true);
            isTouch = false;
        }
    }
}