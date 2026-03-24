using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private PlayerHealth playerHealth;

    private float maxHealth;

    private void Start()
    {
        maxHealth = playerHealth.GetHealth();
    }

    private void Update()
    {
        healthFill.fillAmount = playerHealth.GetHealth() / maxHealth;
    }
}
