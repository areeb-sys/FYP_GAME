using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum GameMode
{
    Easy,
    Medium,
    Hard
}

public class XPManager : MonoBehaviour
{
    public int xp = 0;

    public GameMode gameMode = GameMode.Easy;

    public Image xpBarImage;
    public TMP_Text xpText;
    public TMP_Text modeText;

    private int maxXp;

    private void Start()
    {
        // Load XP from player preferences or other storage mechanism
        xp = PlayerPrefs.GetInt("XP", 0);

        // Set game mode based on XP
        if (xp < 500)
        {
            gameMode = GameMode.Easy;
        }
        else if (xp < 2000)
        {
            gameMode = GameMode.Medium;
        }
        else
        {
            gameMode = GameMode.Hard;
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
            gameMode = GameMode.Easy;
        }
        else if (xp < 2000)
        {
            gameMode = GameMode.Medium;
        }
        else if (xp < 3000)
        {
            gameMode = GameMode.Hard;
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
            case GameMode.Easy:
                return 500;
            case GameMode.Medium:
                return 1500;
            case GameMode.Hard:
                return 3000;
            default:
                return 0;
        }
    }
}