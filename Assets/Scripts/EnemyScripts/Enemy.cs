using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTagert;

    [SerializeField] private float moveSpeed = 2f;

    private Vector3 tempScale;

    [SerializeField] private float meleeDistance = 1.5f;
    [SerializeField] private float rangedDistance = 5f;

    private PlayerAnimation enemyAnimation;

    [SerializeField] private float attackWaitTime = 2.5f;
    private float attackTimer;

    [SerializeField] private float attackFinishedWaitTime = 0.5f;
    private float attackFinishedTimer;

    [SerializeField] private EnemyDamageArea enemyDamageArea;

    [SerializeField] private EnemyAttackType attackType;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootDelay = 0.2f;
    private bool canShoot = true;

    private bool enemyDied;
    private bool isTakingHit;

    private void Awake()
    {
        playerTagert = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG).transform;
        enemyAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        if (enemyDied || isTakingHit)
            return;

        SearchForPlayer();
    }

    void SearchForPlayer()
    {
        if (!playerTagert)
            return;

        HandleFacingDirection();

        float targetDistance = (attackType == EnemyAttackType.Melee)
                                ? meleeDistance
                                : rangedDistance;

        if (Vector3.Distance(transform.position, playerTagert.position) > targetDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTagert.position,
                moveSpeed * Time.deltaTime);

            enemyAnimation.PlayAnimation(TagManager.WALK_ANIMATION_NAME);
        }
        else
        {
            if (attackType == EnemyAttackType.Gun)
            {
                enemyAnimation.PlayAnimation(TagManager.IDLE_ANIMATION_NAME);
            }

            Attack();
        }

    }

    void Attack()
    {
        if (Time.time > attackTimer)
        {
            attackTimer = Time.time + attackWaitTime;

            switch (attackType)
            {
                case EnemyAttackType.Melee:
                    enemyAnimation.PlayAnimation(TagManager.ATTACK_ANIMATION_NAME);
                    EnemyAttack(); 
                    break;

                case EnemyAttackType.Gun:
                    if (canShoot)
                    {
                        canShoot = false;
                        Invoke(nameof(Shoot), shootDelay);
                    }
                    break;
            }
        }
    }


    void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null)
        return;

    GameObject bullet = Instantiate(
        bulletPrefab,
        shootPoint.position,
        Quaternion.identity
    );

    Vector2 direction = (playerTagert.position - shootPoint.position).normalized;

    bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 15f;

        canShoot = true;
    }



    void EnemyAttack()
    {
        if (enemyDamageArea == null)
            return;

        enemyDamageArea.gameObject.SetActive(true);
        enemyDamageArea.ResetDeactivateTimer();
    }

    public void EnemyDied()
    {
        enemyDied = true;
        enemyAnimation.PlayAnimation(TagManager.DEATH_ANIMATION_NAME);
        Invoke(nameof(DestroyEnemyAfterDelay), 1.5f);
    }

    void DestroyEnemyAfterDelay()
    {
        Destroy(gameObject);
    }

    public void GetHit()
    {
        if (enemyDied)
            return;

        isTakingHit = true;

        enemyAnimation.PlayAnimation(TagManager.GETHIT_ANIMATION_NAME);

        Invoke(nameof(ResetHitState), 0.3f);
    }

    void ResetHitState()
    {
        isTakingHit = false;
    }

    void HandleFacingDirection()
    {
        tempScale = transform.localScale;

        if (playerTagert.position.x < transform.position.x)
        {
            tempScale.x = Mathf.Abs(tempScale.x); 
        }
        else
        {
            tempScale.x = -Mathf.Abs(tempScale.x);
        }

        transform.localScale = tempScale;
    }

}
