using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;
    private  PlayerMovement playerMovement;
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void TakeDamage(float damageAmount)
    {
        if(health <= 0f)
        {
            return;
        }

        health -= damageAmount;

        if(health <= 0f)
        {
            playerMovement.PlayerDied();
            GameplayController.instance.RestartGame();
        }
    }

    public float GetHealth()
    {
        return health;
    }

}
