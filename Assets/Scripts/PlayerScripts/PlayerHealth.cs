using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField] private float health = 100f;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(float damageAmount)
    {
        if (health <= 0f) return;

        health -= damageAmount;
        health = Mathf.Max(0, health); 

        if (health <= 0f)
        {
            playerMovement.PlayerDied();

            if (GameOverUI.instance != null)
            {
                int finalScore = GameplayController.instance.GetScore();
                GameOverUI.instance.ShowGameOver(finalScore);
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void HealPlayer(int amount)
    {
        health += amount;
        
        health = Mathf.Min(health, maxHealth); 
    }
}