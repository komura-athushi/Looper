using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Tooltip("プレイヤーの最大体力")]
    public int maxHP;  // プレイヤーの最大体力
    [Tooltip("ダメージを食らった後無敵時間（秒）")]
    public float invincibilityDuration;  // 無敵時間（秒）
    [Tooltip("ダメージを食らった後の無敵時間の点滅間隔（秒）")]
    public float blinkInterval;  // 点滅間隔（秒）
}
