using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private Text enemyKillCountText;

    private int enemyKillCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void EnemyKilled()
    {
        enemyKillCount++;
        enemyKillCountText.text = "Kills: " + enemyKillCount.ToString();
    }

    public void RestartGame()
    {
        enemyKillCount = 0;
        enemyKillCountText.text = "Kills: 0";
        Invoke("Restart", 2f);
    }

    void Restart()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
