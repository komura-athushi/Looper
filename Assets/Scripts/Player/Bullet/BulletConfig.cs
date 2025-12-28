using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Bullet Config")]
public class BulletConfig : ScriptableObject
{

    public enum BulletType
    {
        Normal,
        Burst,
    }
    // 弾のスピード
    [SerializeField] public float speed = 12f;
    // 弾が生成されて消えるまでの時間
    [SerializeField] public float lifeTime = 4f;

    // 弾の種類、enum形式で選択できるように
    [SerializeField] public BulletType bulletType;

    [SerializeField] public int damageAmount = 1; // 弾のダメージ量

    [SerializeField] public bool isPassThrough = false; // 弾が敵を貫通するかどうか
}
