using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Gauge Config")]
public class GaugeConfig : ScriptableObject
{
    [Min(0.01f)] public float max = 1f;          // 1 = 100%
    [Min(0f)]    public float regenPerSec = 0.25f; // 1秒あたり回復量（max=1基準）
    [Min(0f)]    public float regenDelayAfterUse = 0.1f; // 消費後、回復を少し遅らせたい場合
}
