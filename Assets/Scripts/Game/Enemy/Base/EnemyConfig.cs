using UnityEngine;

// ScriptableObject
public abstract class EnemyConfig : ScriptableObject
{
    [Tooltip("横の移動速度(横方向)")]
    public float horizontalSpeed;  // 横の移動速度
    [Tooltip("敵のmax体力")]
    public int hp;  // 体力

    [Tooltip("敵のタイプ")]
    public BaseEnemyController.EnemyType enemyType;  // 敵のタイプ
}
