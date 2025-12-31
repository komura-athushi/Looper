using UnityEngine;

// プレイヤーが発射する弾のスクリプト
public class Bullet : MonoBehaviour
{

    [SerializeField] private BulletConfig bulletConfig;

    private float speed => bulletConfig.speed;
    private float lifeTime => bulletConfig.lifeTime;
    private BulletConfig.BulletType bulletType => bulletConfig.bulletType;
    private bool isPassThrough => bulletConfig.isPassThrough;
    private int damageAmount => bulletConfig.damageAmount;

    private float _t;

    private void Update()
    {
        // 可変フレームを考慮
        transform.Translate(Vector3.right * (speed * Time.deltaTime));

        _t += Time.deltaTime;
        if (_t >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemyに衝突したかチェック
        BaseEnemyController enemy = collision.GetComponent<BaseEnemyController>();
        // Enemyでなければ何もしない
        if(enemy == null) return;

        // Enemyにダメージを与える
        enemy.TakeDamage(damageAmount);

        // 貫通弾でなければ弾を消滅させる
        if (isPassThrough) return;
        Destroy(gameObject); // 弾を消滅
    }
}
