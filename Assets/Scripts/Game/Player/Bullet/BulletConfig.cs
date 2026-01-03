using UnityEngine;

// ScriptableObject
[CreateAssetMenu(menuName = "LOOPER/Bullet Config")]
public class BulletConfig : ScriptableObject
{
    // 弾の種類
    public enum BulletType
    {
        Normal,
        Burst,
    }
    // 弾のスピード
    [Tooltip("弾のスピード")]
    [SerializeField] public float speed = 12f;

    // 弾の種類、enum形式で選択できるように
    [Tooltip("弾の種類")]
    [SerializeField] public BulletType bulletType;
    [Tooltip("弾のダメージ量")]

    [SerializeField] public int damageAmount = 1; // 弾のダメージ量

    [Tooltip("trueなら貫通弾")]
    [SerializeField] public bool isPassThrough = false; // 弾が敵を貫通するかどうか
}
