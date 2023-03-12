using System.Collections;
using System.Collections.Generic;
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

    private float inputX;
    private float inputZ;
    private Vector3 v_movement;
    public float speed;
    private float gravity;
    public Button dodge;


    // Start is called before the first frame update
    void Start()
    {
        speed = 0.1f;
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
        //inputX = Input.GetAxis("Horizontal");
        //inputZ = Input.GetAxis("Vertical");

        inputX = joystick.inputHorizontal();
        inputZ = joystick.inputVertical();



        //Debug.Log(inputX + inputZ); 
        if(inputX ==0 && inputZ == 0 )
        {
            animator.SetBool("isMoving", false);
        }
        else 
        {
            animator.SetBool("isMoving", true);
        }
       

        

    }

    private void FixedUpdate()
    {
        if(_characterController.isGrounded)
        {
            v_movement.y = 0f;

        }
        else
        {
            v_movement.y -= gravity * Time.deltaTime;
        }
        //movement
        v_movement = new Vector3(inputX * speed, v_movement.y, inputZ * speed);
        _characterController.Move(v_movement);
        //rotation
        if(inputX !=0 || inputZ !=0)
        {
            Vector3 lookDir = new Vector3(v_movement.x, 0, v_movement.z);
            meshPlayer.rotation = Quaternion.LookRotation(lookDir);
        }
        
    }

    public void IsDodging()
    {
        Debug.Log("Inside IsDodgeing");
        animator.SetBool("isDodging", true);
        
    }
}
