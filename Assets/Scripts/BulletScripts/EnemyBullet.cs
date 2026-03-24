using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
