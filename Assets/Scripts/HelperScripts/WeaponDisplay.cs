using UnityEngine;
using UnityEngine.UI; 

public class WeaponDisplay : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image gunIconImage; 

    private PlayerShootingManager shootingManager;

    private void Awake()
    {
        shootingManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShootingManager>();
    }

    private void Update()
    {
        UpdateGunIcon();
    }

    void UpdateGunIcon()
    {
        GunData currentGun = shootingManager.GetCurrentGun();

        if (currentGun != null && gunIconImage != null)
        {
            gunIconImage.sprite = currentGun.gunIcon;
        }
    }
}