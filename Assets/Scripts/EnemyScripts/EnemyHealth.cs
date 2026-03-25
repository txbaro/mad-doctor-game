using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;

    private Enemy enemyScript;
    
    [Header("Loot Drop")]
    public GameObject[] potionPrefabs; 
    [Range(0f, 1f)]
    public float dropChance = 0.5f; 

    private void Awake()
    {
        enemyScript = GetComponent<Enemy>();
    }

    // Bạn phải thêm "DamageSource source" vào trong ngoặc đơn này
public void TakeDamage(float damageAmount, DamageSource source) 
{
    if(health <= 0)
    {
        return;
    }
    health -= damageAmount;

    if (health > 0f)
    {
        // Bây giờ biến 'source' đã tồn tại, lỗi CS0103 sẽ biến mất
        if (source == DamageSource.Lazer)
        {
            enemyScript.PlayLazerHitAnim();
        }
        else
        {
            enemyScript.GetHit();
        }
    }   

    if(health <= 0f)
    {
        health = 0;
        DropLoot(); // Hàm rớt đồ của bạn
        enemyScript.EnemyDied();
        EnemySpawner.instance.EnemyDied(gameObject);
        GameplayController.instance.EnemyKilled();
    }
}

    private void DropLoot()
    {
        // Kiểm tra xem đã gán Prefab chưa VÀ tỉ lệ random có trúng không (<= dropChance)
        if (potionPrefabs.Length > 0 && Random.value <= dropChance)
        {
            // Bốc thăm ngẫu nhiên 1 vị trí trong mảng bình thuốc (đỏ hoặc xanh)
            int randomIndex = Random.Range(0, potionPrefabs.Length);
            
            // Sinh ra bình thuốc rơi ngay tại vị trí hiện tại của quái vật
            Instantiate(potionPrefabs[randomIndex], transform.position, Quaternion.identity);
        }
    }
}