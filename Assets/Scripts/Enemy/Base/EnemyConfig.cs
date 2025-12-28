using UnityEngine;

// ScriptableObject
public abstract class EnemyConfig : ScriptableObject
{
    public float speed;  // 横の移動速度
    public int hp;  // 体力
    public float lifeTime;  // 出現から消滅までの時間
}
