using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Bullet Cost Config")]
public class BulletCostConfig : ScriptableObject
{
    [Tooltip("通常弾のゲージ消費量")]
    [SerializeField] public float normalBulletCost = 0.125f; // 1/8
    [Tooltip("バースト弾のゲージ消費量")]
    [SerializeField] public float burstBulletCost = 1f;      // フルゲージ
}