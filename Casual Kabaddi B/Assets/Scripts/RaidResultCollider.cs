using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidResultCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && KabaddiGameManager.instance.isPlayerRaiding)
        {
            KabaddiGameManager.instance.RaidComplete();
        }
    }
}
