using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageAmount = 35f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 tempScale; 

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void Update()
    {
        moveBullet();
    }

    void moveBullet()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    public void SetNegativeSpeed()
    {
        moveSpeed *= -1f;
        tempScale = transform.localScale;
        tempScale.x = -tempScale.x;
        transform.localScale = tempScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                enemyHealth = collision.GetComponentInParent<EnemyHealth>();
            }
            
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount, DamageSource.Bullets);
            }

            Destroy(gameObject);
        }
    }
}