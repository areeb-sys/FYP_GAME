using UnityEngine;


public class CamFollow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed  =0.125f;

    public Vector3 offset;

    public bool isFollow = false;

    private void FixedUpdate()
    {
        if(isFollow)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }


    }

}