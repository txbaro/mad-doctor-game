using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.5f;
    
    [SerializeField] private float minBound_X = -71f, maxBound_X = 71f, minBound_Y = -3.3f, maxBound_Y = 0f;
    
    private Vector3 tempPos;
    private float xAixs, yAixs;
    private PlayerAnimation playerAnimation;

    [SerializeField] 
    private float moveWaitTime = 0.3f, shootWaitTime = 0.5f;

    private float waitBeforeMoving, waitBeforeShooting;
    private bool canMove = true;

    private PlayerShootingManager playerShootingManager;

    private bool playerDied;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerShootingManager = GetComponent<PlayerShootingManager>();
    }
    
    void Update()
    {
        if(playerDied)
        {
            return;
        }

        HandleMovement();
        HandleAnimation();
        HandleDirection();

        HandleShooting();
        CheckIfCanMove();
    }
    
    void HandleMovement()
    {
        xAixs = Input.GetAxisRaw(TagManager.HORIZONTAL_AXIS);
        yAixs = Input.GetAxisRaw(TagManager.VERTICAL_AXIS); 

        tempPos = transform.position;
        tempPos.x += xAixs * moveSpeed * Time.deltaTime;
        tempPos.y += yAixs * moveSpeed * Time.deltaTime;

        // Giới hạn biên
        if (tempPos.x < minBound_X) tempPos.x = minBound_X;
        if (tempPos.x > maxBound_X) tempPos.x = maxBound_X;
        if (tempPos.y < minBound_Y) tempPos.y = minBound_Y;
        if (tempPos.y > maxBound_Y) tempPos.y = maxBound_Y;
        
        transform.position = tempPos;
    }

    void HandleAnimation()
    {
        if (!canMove) return;

        GunData currentGun = playerShootingManager.GetCurrentGun();
        
        string walkAnim = currentGun != null ? currentGun.walkAnimName : TagManager.WALK_ANIMATION_NAME;
        string idleAnim = currentGun != null ? currentGun.idleAnimName : TagManager.IDLE_ANIMATION_NAME;

        if(Mathf.Abs(xAixs) > 0 || Mathf.Abs(yAixs) > 0)
        {
            playerAnimation.PlayAnimation(walkAnim);
        }
        else
        {
            playerAnimation.PlayAnimation(idleAnim);
        }
    }

    void HandleDirection()
    {
        if (xAixs > 0)
        {
            playerAnimation.SetFacingDirection(true);
        }
        else if (xAixs < 0)
        {
            playerAnimation.SetFacingDirection(false);
        }
    }
    
    void StopMovement()
    {
        canMove = false;
        waitBeforeMoving = Time.time + moveWaitTime;
    }

    void Shoot()
    {
        waitBeforeShooting = Time.time + shootWaitTime;
        StopMovement();

        GunData currentGun = playerShootingManager.GetCurrentGun();
        string shootAnim = currentGun != null ? currentGun.shootAnimName : TagManager.SHOOT_ANIMATION_NAME;

        playerAnimation.PlayAnimation(shootAnim);

        playerShootingManager.Shoot(transform.localScale.x);
    }

    void CheckIfCanMove()
    {
        if(Time.time > waitBeforeMoving)
        {
            canMove = true;
        }
    }

    void HandleShooting()
    {
        if (playerShootingManager.IsLaserGun())
        {
            if (Input.GetKey(KeyCode.K))
            {
                playerShootingManager.FireLaser();
            }
            else if (Input.GetKeyUp(KeyCode.K)) 
            {
                playerShootingManager.StopLaser();
            }
            else 
            {
                playerShootingManager.StopLaser();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (Time.time > waitBeforeShooting)
                {
                    Shoot(); 
                }
            }
        }
    }

    public void PlayerDied()
    {
        playerDied = true;
        playerAnimation.PlayAnimation(TagManager.DEATH_ANIMATION_NAME);
        Invoke("DestroyPlayerAfterDelay", 1.5f);
    }

    void DestroyPlayerAfterDelay()
    {
        Destroy(gameObject);
    }
}