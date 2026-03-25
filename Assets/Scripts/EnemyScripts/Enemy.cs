using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerTagert;

    [SerializeField] private float moveSpeed = 2f;

    private Vector3 tempScale;

    [SerializeField] private float meleeDistance = 1.5f;
    [SerializeField] private float rangedDistance = 5f;

    private PlayerAnimation enemyAnimation;
    [Header("Animation Settings")]
    [SerializeField] private string lazerHitAnimName = "GetElectric";
    [SerializeField] private string getHitAnimName = "GetHit";

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
        GameObject playerObj = GameObject.FindGameObjectWithTag(TagManager.PLAYER_TAG);

        if (playerObj != null)
        {
            playerTagert = playerObj.transform;
        }

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
            enemyAnimation.PlayAnimation(TagManager.IDLE_ANIMATION_NAME);
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

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
        
        Collider2D col = GetComponent<Collider2D>();
        if(col != null) col.enabled = false;

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
    
    // Sửa dòng này: Thay vì dùng TagManager, hãy dùng biến getHitAnimName
    enemyAnimation.PlayAnimation(getHitAnimName); 
    
    // Đảm bảo thời gian ResetHitState khớp với độ dài animation bị trúng đạn của bạn
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

    public void PlayLazerHitAnim()
{
    if (enemyDied) return;

    // Chặn Update di chuyển khi đang bị laser chích
    isTakingHit = true; 
    
    if (!enemyAnimation.IsAnimationPlaying(lazerHitAnimName))
    {
        enemyAnimation.PlayAnimation(lazerHitAnimName);
    }

    // Hủy lệnh Reset cũ nếu có và tạo lệnh mới để quái sớm quay lại trạng thái bình thường
    CancelInvoke(nameof(ResetHitState));
    Invoke(nameof(ResetHitState), 0.1f); 
}
}