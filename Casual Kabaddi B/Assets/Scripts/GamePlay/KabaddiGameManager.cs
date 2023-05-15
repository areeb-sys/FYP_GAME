using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public EnemyPlayer enemyPrefab;
    public Transform enemySpawnPoint;

    public List<Player> PlayersList;
    public List<EnemyPlayer> EnemiesList;

    public GameObject ControlPanel;

    public joystickManager joystick;
    [SerializeField]
    public Player currentPlayer;// Current Player for Raid and Chasing
    private Transform currentCameraTarget;

    public CamFollow camFollow;
    public Vector3 EnemyCameraOffset;
    public Vector3 PlayerCameraOffset;

    public GameObject CrossLine;

    public GameObject TabsPanel;
    public TMP_Text tabsText;
    public EnemyPlayer chasingAI;// Opponent 

    public int playerPoints = 0;
    public TMP_Text playerPointText;
    public int opponentPoints = 0;
    public TMP_Text opponentPointsText;

    public GameObject RaidCompletePanel;
    public TMP_Text raidCompleteText;

    public GameObject kabadiStadium;
    public GameObject soccerStadium;

    public GameObject gamePanel;
    public GameObject grabButton;

    [Header("Teams Details")]
    public TMP_Text[] playerNames;
    public TMP_Text[] opponentNames;
    public GameObject TeamsPanel;

    [Header("Game End ")]
    public TMP_Text totalPlayerSuccessfullRaids_Text;
    public TMP_Text totalOpponentSuccessfullRaids_Text;
    public TMP_Text resultText;
    public TMP_Text xpRewardText;
    public GameObject GameEndPanel;

    public List<string> RandomNamesList = new List<string>();

    public AudioSource musicAudioSource;
    public Slider musicSlider; 
    public AudioSource kabadiCountAudioSource;
    public TMP_Text kabadiCountText;
    public static KabaddiGameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        SetMusicSlider(PlayerPrefs.GetFloat("MusicSlider", 0.5f));
    }
    private void Update()
    {
        float inputX = joystick.inputHorizontal();
        float inputZ = joystick.inputVertical();

        

        if (currentPlayer != null && currentPlayer.canMove)
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
            Debug.Log("You Won the Toss");
        }
        else
        {
            playerTeamGetsFirstRaid = false;
            tossResultText.text = "You loose the toss and choose to raid first. Defend!";
            Debug.Log("You Lose the Toss");
        }
        StartCoroutine(AssignTeamMembers());
    }
    IEnumerator AssignTeamMembers()
    {
        yield return new WaitForSeconds(2f);
        
        tossResultText.text = "Making Teams Formation. Please Wait...";
        //xPManager.gameMode = GameMode.Hard;
        switch (xPManager.gameMode)
        {
            case GameMode.E:
                SpawnEasyMode();
                soccerStadium.SetActive(false);
                kabadiStadium.SetActive(true);
                break;
            case GameMode.M:
                SpawnMediumMode();
                soccerStadium.SetActive(false);
                kabadiStadium.SetActive(true);
                break;
            case GameMode.H:
                SpawnHardMode();
                soccerStadium.SetActive(true);
                kabadiStadium.SetActive(false);
                break;
        }

        yield return new WaitForSeconds(1.5f);
        HeadTailPanel.SetActive(false);
        kabadiCountText.gameObject.SetActive(false);
        TeamsPanel.SetActive(true);
        grabButton.SetActive(false);

        for (int i = 0; i < 7; i++)
        {
            //Captain Condition
            if(i == 0)
            {
                PlayersList[i].playerName = xPManager.userName;
            }
            else
            {
                PlayersList[i].playerName = RandomNamesList[Random.Range(0, RandomNamesList.Count)];
            }

            EnemiesList[i].playerName = RandomNamesList[Random.Range(0, RandomNamesList.Count)];

            playerNames[i].text = PlayersList[i].playerName;
            opponentNames[i].text = EnemiesList[i].playerName;

            yield return new WaitForSeconds(0.5f);
        }
        kabadiCountText.gameObject.SetActive(true);
        kabadiCountAudioSource.Play();
        for (int i = 10; i >= 0; i--)
        {
            kabadiCountText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        musicAudioSource.Stop();

        TeamsPanel.SetActive(false);
        gamePanel.SetActive(true);

        yield return new WaitForSeconds(7f);

        if (playerTeamGetsFirstRaid)
        {
            Raid();
            Debug.Log("Raiding");
        }
        else
        {
            Defend();
            Debug.Log("Defending..");
        }

        //Defend();
    }
    public bool isPlayerRaiding = false;
    public int playerCounter = 0;
    private void Raid()
    {
        playerCounter++;
        isPlayerRaiding = true;
        if (PlayersList.Count > 0)
        {
            currentPlayer = PlayersList[playerCounter - 1];
            currentPlayer.canMove = true;
            //PlayersList.RemoveAt(0);
        }
        currentPlayer.ActiveObject.SetActive(true);
        ControlPanel.SetActive(true);
        camFollow.isFollow = true;
        currentCameraTarget = currentPlayer.transform;
        camFollow.target = currentCameraTarget;
        camFollow.offset = PlayerCameraOffset;
        camFollow.transform.localEulerAngles = new Vector3(10, 0, 0);
        //playerCounter++;
    }
    public int enemyCounter = 0;
    private void Defend()
    {
        camFollow.isFollow = true;
        enemyCounter++;
        isPlayerRaiding = false;
        //EnemyPlayer enemy;
        chasingAI = EnemiesList[enemyCounter - 1];
        //EnemiesList.RemoveAt(0);
        ControlPanel.SetActive(false);
        var random = Random.Range(0, PlayersList.Count - 1);
        currentPlayer = PlayersList[random];
        chasingAI.target = PlayersList[random].transform;
        chasingAI.Raid();
        currentCameraTarget = chasingAI.transform;
        camFollow.target = currentCameraTarget;
        camFollow.offset = EnemyCameraOffset;
        camFollow.transform.localEulerAngles = new Vector3(45, 0, 0);

        camFollow.isFollow = true;

        //enemyCounter++;
    }
    public int points = 0;
    public void RaidComplete()
    {
        
        RaidCompletePanel.SetActive(true);
        if(isPlayerRaiding)
        {
            chasingAI.ChaseFailed();            
            PlayersPoint();
            currentPlayer.canMove = false;
        }
        else
        {
            chasingAI.RaidFailed();
            OpponentsPoint();
        }
        Debug.Log("Raid Complete");
        raidCompleteText.text = "Raid Success!";
        Invoke("SwitchRaid", 3f);
    }
    public int raidCounter = 0;
    public void RaidFailed()
    {
        RaidCompletePanel.SetActive(true);
        raidCompleteText.text = "Raid Failed!";
        Invoke("SwitchRaid", 3f);
    }

    public void ResetEverything()
    {
        currentPlayer.Walk(false);
        
        CrossLine.SetActive(false);
        TabsPanel.SetActive(false);
        RaidCompletePanel.SetActive(false);
        grabButton.SetActive(false);
    }

    private void SwitchRaid()
    {
        raidCounter++;
        if(raidCounter == 14)
        {
            GameEnd();
            return;
        }
        raidCompleteText.text = "Swtiching Turn";
        Invoke("SwitchTurn", 1f);
    }

    private void SwitchTurn()
    {
        
        ResetEverything();
        foreach (var player in PlayersList)
        {
            player.ResetPlayer();
        }
        foreach(var opponent in EnemiesList)
        {
            opponent.ResetPlayer();
        }
        if (isPlayerRaiding)
        {
            Defend();
        }
        else
        {
            Raid();
        }
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
            EnemyPlayer enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
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
            EnemyPlayer enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
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
            EnemyPlayer enemy = Instantiate(enemyPrefab, position, enemySpawnPoint.rotation);
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
    public int tabsTimes = 0;
    public void ShowTabPanel()
    {
        var random = Random.Range(3, 8);
        tabsTimes = random;
        tabsText.text = "Tab " + tabsTimes + " times to ungrab";
        TabsPanel.SetActive(true);
        StartCoroutine(TabsCountDown());
    }

    IEnumerator TabsCountDown()
    {
        yield return new WaitForSeconds(3f);
        if(tabsTimes > 0)
        {
            //OpponentsPoint();
            RaidFailed();
        }
    }
    public void Tab()
    {
        tabsTimes--;
        if(tabsTimes <= 0)
        {
            //PlayersPoint();
            TabsPanel.SetActive(false);

            RaidComplete();
        }
        tabsText.text = "Tab " + tabsTimes + " times to ungrab";
    }

    public void PlayersPoint()
    {
        playerPoints++;
        playerPointText.text = playerPoints + "";
    }
    public void OpponentsPoint()
    {
        opponentPoints++;
        opponentPointsText.text = opponentPoints + "";
    }

    public void Grab()
    {
        chasingAI.RaidFailed();
        RaidFailed();
    }

    public void GameEnd()
    {
        //Show Stats here
        camFollow.isFollow = false;
        musicAudioSource.Play();
        var gainedXP = 0;
        if(playerPoints > opponentPoints)
        {
            resultText.text = "You Win";
            gainedXP = 25;
        }
        else if(playerPoints == opponentPoints)
        {
            resultText.text = "Draw";
            gainedXP = 5;
        }
        else
        {
            resultText.text = "You Lose";
            gainedXP = 5;
        }
        xpRewardText.text = string.Format("XP : {0} + {1}", xPManager.xp, gainedXP);
        xPManager.AddXP(gainedXP);
        GameEndPanel.SetActive(true);
        Debug.Log(opponentPoints);
        totalOpponentSuccessfullRaids_Text.text = opponentPoints.ToString();
        totalPlayerSuccessfullRaids_Text.text = playerPoints.ToString();


    }
    public void SetMusicSlider(float value)
    {
        musicAudioSource.volume = value;
        musicSlider.value = value;
        PlayerPrefs.SetFloat("MusicSlider", value);
    }
    public void Exit()
    {
        Application.Quit();
    }
}