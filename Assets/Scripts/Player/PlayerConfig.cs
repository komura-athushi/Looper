using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Player Config")]
public class PlayerConfig : ScriptableObject
{
    public int maxHP;  // プレイヤーの最大体力
    public float invincibilityDuration;  // 無敵時間（秒）
    public float blinkInterval;  // 点滅間隔（秒）
}
