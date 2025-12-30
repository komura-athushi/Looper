using UnityEngine;

// 弾を生成するGunスクリプト
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject burstBulletPrefab;

    [SerializeField] private PlayerGaugeController gauge;
    [SerializeField] private BulletCostConfig bulletCostConfig;

    public void Fire()
    {
        if (muzzle == null || bulletPrefab == null) return;
        
        // もしゲージがマックスなら
        if (gauge.Normalized >= 1f)
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
        Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        // 通常弾発射音再生
        AudioManager.Instance.PlaySound("fire");
    }

    private void BurstFire()
    {
        // ゲージ消費
        gauge.TryConsume(bulletCostConfig.burstBulletCost);
        // バースト弾発射
        Instantiate(burstBulletPrefab, muzzle.position, Quaternion.identity);
    }
}
