using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private Image gravityIcon;

    public static HUD Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHighScore(int highscore)
    {
        highscoreText.text = highscore.ToString();
    }

    public void UpdateGravityDelay(float step)
    {
        gravityIcon.fillAmount = step;
    }

    public void GameOver(bool flag)
    {
        gameoverPanel.SetActive(flag);
    }
}
