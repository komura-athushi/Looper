using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Gauge Config")]
public class GaugeConfig : ScriptableObject
{
    [Tooltip("ゲージの最大値（1 = 100%）")]
    [Min(0.01f)] public float max = 1f;          // 1 = 100%
    [Tooltip("1秒あたりの回復量（max=1基準）")]
    [Min(0f)]    public float regenPerSec = 0.25f; // 1秒あたり回復量（max=1基準）
    [Tooltip("ゲージ消費後、回復を遅らせる時間(秒)")]
    [Min(0f)]    public float regenDelayAfterUse = 0.0f; // 消費後、回復を少し遅らせたい場合
}
