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
    public Player playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    public List<Player> PlayersList;
    public List<GameObject> EnemiesList;

    public GameObject ControlPanel;

    public joystickManager joystick;
    [SerializeField]
    private Player currentPlayer;
    private Transform currentCameraTarget;

    public CamFollow camFollow;
    public Vector3 EnemyCameraOffset;
    public Vector3 PlayerCameraOffset;

    public bool isChasing;
    public GameObject CrossLine;

    public GameObject TabsPanel;
    public TMP_Text tabsText;

    public static KabaddiGameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        float inputX = joystick.inputHorizontal();
        float inputZ = joystick.inputVertical();

        

        if (currentPlayer != null)
        {
            if (inputX == 0f && inputZ == 0f)
            {
                currentPlayer.Walk(false);
            }
            else
            {
                currentPlayer.Walk(true);
            }

            Vector3 movement = new Vector3(inputX, 0, inputZ).normalized;

            if (movement != Vector3.zero)
            {
                // Calculate the speed based on the joystick position

                currentPlayer._characterController.Move(movement * Time.deltaTime);

                // Rotate the meshPlayer based on the movement direction
                Vector3 lookDir = new Vector3(movement.x, 0, movement.z);
                currentPlayer.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }
    }

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
        StartCoroutine(AssignTeamMembers());
    }
    IEnumerator AssignTeamMembers()
    {
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

        //if(playerTeamGetsFirstRaid)
        //{
        //    Raid();
        //}
        //else
        //{
        //    Defend();
        //}

        Raid();
    }

    private void Raid()
    {
        if(PlayersList.Count > 0)
        {
            currentPlayer = PlayersList[0];
            PlayersList.RemoveAt(0);
        }
        currentPlayer.ActiveObject.SetActive(true);
        ControlPanel.SetActive(true);

        currentCameraTarget = currentPlayer.transform;
        camFollow.target = currentCameraTarget;
        camFollow.offset = PlayerCameraOffset;
        camFollow.transform.localEulerAngles = new Vector3(10, 0, 0);
    }

    private void Defend()
    {
        GameObject enemy;
        enemy = EnemiesList[0];
        ControlPanel.SetActive(false);
        currentCameraTarget = enemy.transform;
        camFollow.target = currentCameraTarget;
        camFollow.offset = EnemyCameraOffset;
        camFollow.transform.localEulerAngles = new Vector3(45, 0, 0);
    }


    public bool DoesPlayerTeamGetFirstRaid()
    {
        return playerTeamGetsFirstRaid;
    }
    #region Spawning
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
            Player player =  Instantiate(playerPrefab, position, playerSpawnPoint.rotation) as Player;
            Debug.Log("Spawnning Player ");
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
            Debug.Log("Spawnning Enemy ");

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
            Player player = Instantiate(playerPrefab, position, playerSpawnPoint.rotation) as Player;
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
            EnemiesList.Add(enemy);
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
            Player player = Instantiate(playerPrefab, position, playerSpawnPoint.rotation) as Player;
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
            EnemiesList.Add(enemy);
            currentAngle += angleIncrement;
        }
    }
    #endregion

    public void Dodge()
    {
        currentPlayer.IsDodging();
    }

    public void Touch()
    {
        currentPlayer.IsTouching();
    }

    public void ShowTabPanel()
    {
        var random = Random.Range(0, 6);
        tabsText.text = "Tab " + 6 + "times";
            
    }
    public void Exit()
    {
        Application.Quit();
    }
}