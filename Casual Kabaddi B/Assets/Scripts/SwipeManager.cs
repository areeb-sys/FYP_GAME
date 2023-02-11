using UnityEngine;


public class SwipeManager : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;
    private Vector3 movement;
    private float moveSpeed;

    private float inputX;
    private float inputZ;

    public enum Swipe { Up, Down, Left, Right, None, UpLeft, UpRight, DownLeft, DownRight };
    public float minSwipeLength = 200f;
    //public TextMesh debugInfo;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    Vector3 movementDirection;

    float tweakFactor = 0.5f;

    public static Swipe swipeDirection;
    private Vector2 PosInp;

    private void Start()
    {
        moveSpeed = 0.1f;
        anim = GetComponent<Animator>();
        GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
        controller = tempPlayer.GetComponent<CharacterController>();
    }
    void Update()
    {
        inputX = Input.GetAxis("Vertical");
        inputZ = Input.GetAxis("Horizontal");
        Vector3 movementDirection = new Vector3(inputX, 0, inputZ);
        movementDirection.Normalize();
        DetectSwipe();
    }



    public void DetectSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    //debugInfo.text = "Tapped";
                    swipeDirection = Swipe.None;
                    return;
                }

                currentSwipe.Normalize();

                //debugInfo.text = currentSwipe.x.ToString() + " " + currentSwipe.y.ToString();

                // Swipe up
                if (currentSwipe.y > 0 && currentSwipe.x > 0 - tweakFactor && currentSwipe.x < tweakFactor) {
                    swipeDirection = Swipe.Up;
                    anim.SetBool("isMoving", true);
                    Debug.Log("Up Swipe");
                    transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
                    controller.Move(movementDirection);
                    //debugInfo.text = "Up swipe";

                    // Swipe down
                } else if (currentSwipe.y < 0 && currentSwipe.x > 0 - tweakFactor && currentSwipe.x < tweakFactor) {
                    swipeDirection = Swipe.Down;
                    anim.SetBool("isMoving", true);
                    Debug.Log("down Swipe");
                    //debugInfo.text = "Down swipe";

                    // Swipe left
                } else if (currentSwipe.x < 0 && currentSwipe.y > 0 - tweakFactor && currentSwipe.y < tweakFactor) {
                    swipeDirection = Swipe.Left;
                    transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
                    anim.SetBool("isMoving", true);
                    Debug.Log("left Swipe");
                    //debugInfo.text = "Left swipe";

                    // Swipe right
                } else if (currentSwipe.x > 0 && currentSwipe.y > 0 - tweakFactor && currentSwipe.y < tweakFactor) {
                    swipeDirection = Swipe.Right;
                    anim.SetBool("isMoving", true);
                    transform.Translate(movementDirection * moveSpeed * Time.deltaTime, Space.World);
                    Debug.Log("right Swipe");
                    //debugInfo.text = "Right swipe";

                    // Swipe up left
                } else if (currentSwipe.y > 0 && currentSwipe.x < 0 ) {
                    swipeDirection = Swipe.UpLeft;
                    anim.SetBool("isMoving", true);
                    Debug.Log("UpLeft Swipe");
                    //debugInfo.text = "Up Left swipe";

                    // Swipe up right
                } else if (currentSwipe.y > 0 && currentSwipe.x > 0 ) {
                    swipeDirection = Swipe.UpRight;
                    anim.SetBool("isMoving", true);
                    Debug.Log("UpRight Swipe");
                    //debugInfo.text = "Up Right swipe";

                    // Swipe down left
                } else if (currentSwipe.y < 0 && currentSwipe.x < 0 ) {
                    swipeDirection = Swipe.DownLeft;
                    anim.SetBool("isMoving", true);
                    Debug.Log("Down left Swipe");
                    //debugInfo.text = "Down Left swipe";

                    // Swipe down right
                } else if (currentSwipe.y < 0 && currentSwipe.x > 0 ) {
                    swipeDirection = Swipe.DownRight;
                    anim.SetBool("isMoving", true);
                    Debug.Log("down right Swipe");
                    //debugInfo.text = "Down Right swipe";
                }
            }
        }
        
    }


   /* public float inputHorizontal()
    {
        if (PosInp.x != 0)
            return PosInp.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float inputVertical()
    {
        if (PosInp.y != 0)
            return PosInp.y;
        else
            return Input.GetAxis("Vertical");
    }*/
}