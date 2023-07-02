using UnityEngine;
public class Player : KabadiPlayer
{
    public GameObject ActiveObject;
    public bool canMove;



    public void ResetPlayer()
    {
        canMove = false;
        ActiveObject.SetActive(false);
        ResetPosition();
    }
    public void Position()
    {
        transform.position = spawnpoint;
    }

}
