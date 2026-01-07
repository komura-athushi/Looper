using UnityEngine;

// HopperEnemy用Config
// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Hopper Enemy Config")]
public class HopperEnemyConfig : EnemyConfig
{
    [Tooltip("縦移動の速度(1秒あたり)")]
    public float verticalSpeed;  // 縦移動の速度(1秒あたり)
    [Tooltip("上移動⇔下移動が切り替わる時間間隔(秒)")]
    public float switchInterval;  // 上移動⇔下移動が切り替わる時間間隔(秒)
}
