using UnityEngine;

public class HealPotion : MonoBehaviour
{
    public int healAmount = 20; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.PLAYER_TAG))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.HealPlayer(healAmount);
                Destroy(gameObject); 
            }
        }
    }
}
