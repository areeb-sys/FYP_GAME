using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum GameMode
{
    E,
    M,
    H
}

public class XPManager : MonoBehaviour
{
    public int xp = 0;
    public string userName;
    public GameMode gameMode = GameMode.E;

    public Image xpBarImage;
    public TMP_Text xpText;
    public TMP_Text modeText;

    public GameObject NamePanel;

    public TMP_Text userName_Text;
    public TMP_InputField userName_InputField;

    private int maxXp;

    private void Start()
    {

        if(PlayerPrefs.GetInt("XP", 0) == 0)
        {
            NamePanel.SetActive(true);
            xp = 10;
            PlayerPrefs.SetInt("XP", xp);
        }
        else
        {
            userName = PlayerPrefs.GetString("UserName");
            userName_Text.text = userName;
        }

        // Set game mode based on XP
        if (xp < 500)
        {
            gameMode = GameMode.E;
        }
        else if (xp < 2000)
        {
            gameMode = GameMode.M;
        }
        else
        {
            gameMode = GameMode.H;
        }
        UpdateXPBar();
    }

    public void AddXP(int amount)
    {
        xp += amount;
        if (xp < 0) return;

        // Update game mode based on new XP
        if (xp < 500)
        {
            gameMode = GameMode.E;
        }
        else if (xp < 2000)
        {
            gameMode = GameMode.M;
        }
        else if (xp < 3000)
        {
            gameMode = GameMode.H;
        }
        if (xp > 3000) xp = 3000;
        // Save XP to player preferences or other storage mechanism
        PlayerPrefs.SetInt("XP", xp);
        UpdateXPBar();
    }

    private void UpdateXPBar()
    {
        maxXp = GetMaxXP(gameMode);
        // Update XP bar fill based on current XP and max XP for the current game mode
        float fillAmount = (float)xp / maxXp;
        xpBarImage.fillAmount = fillAmount;
        xpText.text = string.Format("{0}/{1}", xp, (int)maxXp);
        modeText.text = gameMode.ToString();
    }

    public int GetMaxXP(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.E:
                return 500;
            case GameMode.M:
                return 2000;
            case GameMode.H:
                return 3000;
            default:
                return 0;
        }
    }

    public void SetUserName()
    {
        if(userName_InputField.text != null)
        {
            userName = userName_InputField.text;
            userName_Text.text = userName;

            PlayerPrefs.SetString("UserName", userName);

            NamePanel.SetActive(false);
        }
    }
}