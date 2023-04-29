using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && KabaddiGameManager.instance.isPlayerRaiding)
        {
            Debug.Log("Play Raid Sound");
        }
    }
}
