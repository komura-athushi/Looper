using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Ghost Cost Config")]
public class GhostCostConfig : ScriptableObject
{
    [Tooltip("Ghost状態のゲージ消費量(1秒あたり、1がMAX)")]
    [SerializeField] public float ghostCost = 0.3f; // 1/8
    [Tooltip("Ghost状態に入るための長押し時間（秒）")]
    [SerializeField] public float activationTime = 0.3f;
}