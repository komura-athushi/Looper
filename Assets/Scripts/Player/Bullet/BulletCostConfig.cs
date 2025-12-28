using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Bullet Cost Config")]
public class BulletCostConfig : ScriptableObject
{
    [SerializeField] public float normalBulletCost = 0.125f; // 1/8
    [SerializeField] public float burstBulletCost = 1f;      // フルゲージ
}