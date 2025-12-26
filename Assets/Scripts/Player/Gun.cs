using UnityEngine;

// 弾を生成するGunスクリプト
public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletPrefab;

    public void Fire()
    {
        if (muzzle == null || bulletPrefab == null) return;
        Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
    }
}
