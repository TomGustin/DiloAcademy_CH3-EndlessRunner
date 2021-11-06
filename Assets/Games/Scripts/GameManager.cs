using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int[] levelTresshold;
    [SerializeField] private Transform player;
    [SerializeField] private TerrainSpawner terrainSpawner;

    private int currentIndex;

    private Character2DController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<Character2DController>();
        playerController.Stop(true);
        currentIndex = 0;
        
        Score.CurrentScore = 0;
        HUD.Instance.UpdateHighScore(Score.HighScore);
    }

    // Update is called once per frame
    void Update()
    {
        Score.CurrentScore = Mathf.FloorToInt(player.position.x);
        HUD.Instance.UpdateScore(Score.CurrentScore);

        if (Score.CurrentScore > levelTresshold[currentIndex] && (currentIndex !=  levelTresshold.Length - 1))
        {
            currentIndex++;
            terrainSpawner.ChangeLevel($"level-{currentIndex + 1}");
            AudioSystem.Instance.PlaySFX("score_highlight");
        }
    }

    public void GameOver()
    {
        HUD.Instance.GameOver(true);
        playerController.Stop(true);

        if (Score.CurrentScore >= Score.HighScore) Score.HighScore = Score.CurrentScore;

        HUD.Instance.UpdateHighScore(Score.HighScore);
        HUD.Instance.UpdateScore(Score.CurrentScore);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void StartGame()
    {
        playerController.Stop(false);
    }
}
