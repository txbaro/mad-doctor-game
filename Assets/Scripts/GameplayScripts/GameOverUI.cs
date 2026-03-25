using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = "SCORE: " + score;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        highScoreText.text = "HIGHSCORE: " + highScore;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}