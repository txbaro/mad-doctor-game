using UnityEngine;

[System.Serializable]
public class GunData
{
    public string gunName;
    
    public string idleAnimName = "Idle";
    public string walkAnimName = "Walk";
    public string shootAnimName = "Shoot";
    public string deathAnimName = "Death";

    public Sprite gunIcon; 
    public AudioClip gunShootSound;
    public GameObject bulletPrefab;
    public bool isLaser;
}

public class PlayerShootingManager : MonoBehaviour
{
    [Header("Gun System")]
    [SerializeField] private GunData[] guns;
    
    [SerializeField] private Transform bulletSpawnPos;
    
    [Header("Laser Settings")]
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float laserRange = 8f;
    [SerializeField] private float laserDamagePerSecond = 25f;
    [SerializeField] private LayerMask enemyLayer;

    private int currentGunIndex = 0;

    private void Start()
    {
    }

    public GunData GetCurrentGun()
    {
        if (guns == null || guns.Length == 0) return null;
        return guns[currentGunIndex];
    }

    public bool IsLaserGun()
    {
        if (guns == null || guns.Length == 0) return false;
        return guns[currentGunIndex].isLaser;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentGunIndex++;
            if (currentGunIndex >= guns.Length)
                currentGunIndex = 0;
            
            StopLaser();
        }

        if (Input.GetKeyDown(KeyCode.K) && guns[currentGunIndex].isLaser)
        {
            SoundManager.instance.PlayLoopingSound(guns[currentGunIndex].gunShootSound);
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            SoundManager.instance.StopSound();
            laserObject.SetActive(false); 
        }
    }

    public void Shoot(float facingDirection)
    {
        GunData currentGun = guns[currentGunIndex];

        if (currentGun.isLaser) return;
        if (currentGun.bulletPrefab == null) return;

        GameObject newBullet = Instantiate(
            currentGun.bulletPrefab,
            bulletSpawnPos.position,
            Quaternion.identity
        );

        if (facingDirection < 0)
        {
            newBullet.GetComponent<Bullet>().SetNegativeSpeed();
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(currentGun.gunShootSound);
        }
    }

    public void FireLaser()
    {
        laserObject.SetActive(true);
        
        float playerScaleX = transform.root.localScale.x;
        float directionSign = playerScaleX > 0 ? 1f : -1f;
        
        Vector2 direction = new Vector2(directionSign, 0f);
        
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPos.position, direction, laserRange, enemyLayer);
        
        if (hit.collider != null)
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(laserDamagePerSecond * Time.deltaTime, DamageSource.Lazer);
        }

        Vector3 laserScale = laserObject.transform.localScale;
        laserScale.x = Mathf.Abs(laserScale.x); 
        laserObject.transform.localScale = laserScale;

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(guns[currentGunIndex].gunShootSound);
            }
        }
    }

    public void StopLaser()
    {
        if(laserObject.activeInHierarchy) laserObject.SetActive(false);
    }
}