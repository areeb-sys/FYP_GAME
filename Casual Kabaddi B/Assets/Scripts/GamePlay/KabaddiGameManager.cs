using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KabaddiGameManager : MonoBehaviour
{
    public GameObject playerTeam;
    public GameObject opponentTeam;

    public GameObject HeadTailPanel;
    public TMP_Text tossResultText;

    public GameObject headsButton, tailsButton;

    private bool playerTeamGetsFirstRaid;

    public XPManager xPManager;

    public GameObject MenuPanel;

    public void StartGame()
    {
        // Ask the player to choose heads or tails
        tossResultText.text = "Choose heads or tails.";

        AskPlayerForHeadTail();
    }


    private void AskPlayerForHeadTail()
    {
        HeadTailPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }

    public void ChooseHead(bool playerChoseHeads)
    {
        headsButton.SetActive(false);
        tailsButton.SetActive(false);
        // Randomly determine the result of the toss
        bool tossResultIsHeads = Random.value > 0.5f;

        // Determine if the player won the toss
        if (playerChoseHeads == tossResultIsHeads)
        {
            playerTeamGetsFirstRaid = true;
            tossResultText.text = "You won the toss! Choose to raid first or defend.";
        }
        else
        {
            playerTeamGetsFirstRaid = false;
            tossResultText.text = "Opponent won the toss and chose to raid first. Defend!";
        }

        // Assign player and opponent team members
        AssignTeamMembers(playerTeam, 7);
        AssignTeamMembers(opponentTeam, 7);
    }

    void AssignTeamMembers(GameObject team, int numMembers)
    {
        // Iterate through the team's child objects and activate the first 'numMembers' members
        for (int i = 0; i < team.transform.childCount; i++)
        {
            if (i < numMembers)
            {
                team.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                team.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public bool DoesPlayerTeamGetFirstRaid()
    {
        return playerTeamGetsFirstRaid;
    }

    public void Exit()
    {
        Application.Quit();
    }
}