using UnityEngine;

// 弾を生成するGunスクリプト
public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject burstBulletPrefab;

    private BulletGuageController gauge;
    [SerializeField] private BulletCostConfig bulletCostConfig;

    public void Fire()
    {
        if (bulletPrefab == null || gauge == null) return;
        
        // バースト発射可能なら
        if (gauge.Normalized >= bulletCostConfig.burstBulletCost)
        {
            // バースト発射
            BurstFire();
            return;
        }
        
        // 通常発射
        NormalFire();
    }

    // 通常壇発射
    private void NormalFire()
    {
        // ゲージ消費出来たら
        bool isConsumed = gauge.TryConsume(bulletCostConfig.normalBulletCost);
        if (!isConsumed) return;

        // 弾発射
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // 通常弾発射音再生
        AudioManager.Instance.PlaySound("fire");
    }

    private void BurstFire()
    {
        // ゲージ消費
        gauge.TryConsume(bulletCostConfig.burstBulletCost);
        // バースト弾発射
        Instantiate(burstBulletPrefab, transform.position, Quaternion.identity);
    }

    private void Awake()
    {
        if (gauge == null)
            gauge = GetComponent<BulletGuageController>();
    }
}
