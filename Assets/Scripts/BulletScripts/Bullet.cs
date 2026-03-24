using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageAmount = 35f;

    private Vector3 moveVector = Vector3.zero;

    private Vector3 tempScale; 

    private void Update()
    {
        moveBullet();
    }

    void moveBullet()
    {
        moveVector.x = moveSpeed * Time.deltaTime;
        transform.position += moveVector;
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
        EnemyHealth health = collision.GetComponentInParent<EnemyHealth>();

        if (health != null)
        {
            health.TakeDamage(damageAmount);
             Destroy(gameObject);
        }
    }
}
