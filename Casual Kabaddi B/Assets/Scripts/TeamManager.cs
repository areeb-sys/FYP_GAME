using UnityEngine;
using System.Collections.Generic;

public class TeamManager : MonoBehaviour
{
   // public GameObject playerPrefab;
    public List<managCharCon> team = new List<managCharCon>();
    private int activePlayerIndex = 0;

    /*void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            managCharCon playerController = newPlayer.GetComponent<managCharCon>();
           // playerController.playerNumber = i;
            team.Add(playerController);
        }

        ActivatePlayer(activePlayerIndex);
    }

    void Update()
    {
        if (!team[activePlayerIndex].isAlive)
        {
            ShiftPlayer();
        }
    }

    private void ShiftPlayer()
    {
        // Deactivate the current player
        team[activePlayerIndex].isActive = false;
        team[activePlayerIndex].gameObject.SetActive(false);

        // Find the next alive player in the team
        int nextIndex = activePlayerIndex;
        do
        {
            nextIndex = (nextIndex + 1) % team.Count;
        } while (!team[nextIndex].isAlive);

        // Activate the next player and set them as the new active player
        activePlayerIndex = nextIndex;
        ActivatePlayer(activePlayerIndex);
    }

    private void ActivatePlayer(int index)
    {
        team[index].isActive = true;
        team[index].gameObject.SetActive(true);
    }
}
*/
}