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
    public GameObject raidButton, defendButton;

    private bool playerTeamGetsFirstRaid;

    public XPManager xPManager;

    public GameObject MenuPanel;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    public List<GameObject> PlayersList;
    public List<GameObject> EnemiesList;

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
            tossResultText.text = "You won the toss! You are raiding first.";
        }
        else
        {
            playerTeamGetsFirstRaid = false;
            tossResultText.text = "Opponent won the toss and choose to raid first. Defend!";
        }

        // Assign player and opponent team members
        //AssignTeamMembers(playerTeam, 7);
        //AssignTeamMembers(opponentTeam, 7);

        StartCoroutine(AssignTeamMembers());
    }

    IEnumerator AssignTeamMembers()
    {
        //// Iterate through the team's child objects and activate the first 'numMembers' members
        //for (int i = 0; i < team.transform.childCount; i++)
        //{
        //    if (i < numMembers)
        //    {
        //        team.transform.GetChild(i).gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        team.transform.GetChild(i).gameObject.SetActive(false);
        //    }
        //}
        yield return new WaitForSeconds(2f);
        tossResultText.text = "Making Teams Formation. Please Wait...";
        xPManager.gameMode = GameMode.Hard;
        switch (xPManager.gameMode)
        {
            case GameMode.Easy:
                SpawnEasyMode();
                break;
            case GameMode.Medium:
                SpawnMediumMode();
                break;
            case GameMode.Hard:
                SpawnHardMode();
                break;
        }
        yield return new WaitForSeconds(1.5f);
        HeadTailPanel.SetActive(false);
    }

    public bool DoesPlayerTeamGetFirstRaid()
    {
        return playerTeamGetsFirstRaid;
    }

    void SpawnEasyMode()
    {
        //Ddestroy Previous Players and Clear the List
        foreach (var player in PlayersList)
        {
            Destroy(player);
        }
        PlayersList.Clear();

        //Formating player in line starts from given spawnpoint
        for (int i = 0; i < 7; i++)
        {
            Vector3 position = playerSpawnPoint.position + Vector3.right * i * 2;
            GameObject player =  Instantiate(playerPrefab, position, playerSpawnPoint.rotation);
            PlayersList.Add(player);
        }

        //Ddestroy Previous Enemies and Clear the List
        foreach (var enemy in EnemiesList)
        {
            Destroy(enemy);
        }
        EnemiesList.Clear();

        //Formating enemies in line starts from given spawnpoint
        for (int i = 0; i < 7; i++)
        {
            Vector3 position = enemySpawnPoint.position + Vector3.left * i * 2;
            GameObject enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
            EnemiesList.Add(enemy);
        }

    }

    void SpawnMediumMode()
    {
        //Ddestroy Previous Players and Clear the List
        foreach (var player in PlayersList)
        {
            Destroy(player);
        }
        PlayersList.Clear();

        //Formating player in line starts from given spawnpoint

        // Calculate the angle between each object in the line
        float angleIncrement = 180f / 8;

        // Instantiate players in a line with a curve
        float currentAngle = 0f;
        float curveRadius = 1f;
        Vector3 position = playerSpawnPoint.position;
        
        for (int i = 0; i < 7; i++)
        {
            if (i > 0) // Don't offset the starting point
            {
                position += playerSpawnPoint.right * 1;
            }
            position += Quaternion.Euler(0f, currentAngle, 0f) * (Vector3.forward * curveRadius);
            GameObject player = Instantiate(playerPrefab, position, playerSpawnPoint.rotation);
            PlayersList.Add(player);
            currentAngle += angleIncrement;
        }

        //Ddestroy Previous Enemies and Clear the List
        foreach (var enemy in EnemiesList)
        {
            Destroy(enemy);
        }
        EnemiesList.Clear();

        //Formating enemies in line starts from given spawnpoint
        position = enemySpawnPoint.position;
        for (int i = 0; i < 7; i++)
        {
            if (i > 0) // Don't offset the starting point
            {
                position += (enemySpawnPoint.right * -1f) * 1;
            }
            position += Quaternion.Euler(0f, currentAngle, 0f) * (Vector3.forward * curveRadius);
            GameObject enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
            PlayersList.Add(enemy);
            currentAngle += angleIncrement;
        }
    }

    void SpawnHardMode()
    {
        //Ddestroy Previous Players and Clear the List
        foreach (var player in PlayersList)
        {
            Destroy(player);
        }
        PlayersList.Clear();

        //Formating player in line starts from given spawnpoint

        // Calculate the angle between each object in the line
        float angleIncrement = -180f / 8;

        // Instantiate players in a line with a curve
        float currentAngle = 0f;
        float curveRadius = 1f;
        Vector3 position = playerSpawnPoint.position;

        for (int i = 0; i < 7; i++)
        {
            if (i > 0) // Don't offset the starting point
            {
                position += playerSpawnPoint.right * 1;
            }
            position += Quaternion.Euler(0f, currentAngle, 0f) * (Vector3.back * curveRadius);
            GameObject player = Instantiate(playerPrefab, position, playerSpawnPoint.rotation);
            PlayersList.Add(player);
            currentAngle += angleIncrement;
        }

        //Ddestroy Previous Enemies and Clear the List
        foreach (var enemy in EnemiesList)
        {
            Destroy(enemy);
        }
        EnemiesList.Clear();

        //Formating enemies in line starts from given spawnpoint
        position = enemySpawnPoint.position;
        for (int i = 0; i < 7; i++)
        {
            if (i > 0) // Don't offset the starting point
            {
                position += (enemySpawnPoint.right * 1f) * 1;
            }
            position += Quaternion.Euler(0f, currentAngle, 0f) * (Vector3.back * curveRadius);
            GameObject enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
            PlayersList.Add(enemy);
            currentAngle += angleIncrement;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}