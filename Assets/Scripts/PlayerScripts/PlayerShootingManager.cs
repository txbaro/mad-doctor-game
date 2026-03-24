using UnityEngine;

[System.Serializable]
public class GunData
{
    public string gunName;
    
    // --- SỬA: Thêm các biến lưu tên Animation cho từng súng ---
    public string idleAnimName = "Idle";
    public string walkAnimName = "Walk";
    public string shootAnimName = "Shoot";

    // Sprite chỉ dùng để hiển thị UI hoặc icon (nếu cần), không gán trực tiếp vào Player nữa
    public Sprite gunIcon; 
    
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

        SoundManager.instance.PlayShootSound();
    }

    public void FireLaser()
    {
        laserObject.SetActive(true);
        float directionSign = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 direction = Vector2.right * directionSign;
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPos.position, direction, laserRange, enemyLayer);
        float length = laserRange;
        if (hit.collider != null)
        {
            length = hit.distance;
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(laserDamagePerSecond * Time.deltaTime);
        }
        laserObject.transform.localScale = new Vector3(length * directionSign, 1f, 1f);
    }

    public void StopLaser()
    {
        if(laserObject.activeInHierarchy) laserObject.SetActive(false);
    }
}