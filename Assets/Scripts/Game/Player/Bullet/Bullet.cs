using UnityEngine;

// プレイヤーが発射する弾のスクリプト
public class Bullet : MonoBehaviour
{

    [SerializeField] private BulletConfig bulletConfig;

    private float speed => bulletConfig.speed;
    private BulletConfig.BulletType bulletType => bulletConfig.bulletType;
    private bool isPassThrough => bulletConfig.isPassThrough;
    private int damageAmount => bulletConfig.damageAmount;
    private float screenRightEdge;
    private GameController gameController;
    private int enemyDefeatCount = 0;

    private void Start()
    {
        // 画面右端の位置を計算
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        }
        gameController = FindFirstObjectByType<GameController>();
    }

    private void Update()
    {
        // 可変フレームを考慮
        transform.Translate(Vector3.right * ((speed - gameController.CurrentPlayerSpeed) * Time.deltaTime));

        // 画面右端を超えたら削除
        if (transform.position.x > screenRightEdge)
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
        bool isDeath = enemy.TakeDamage(damageAmount, bulletType);
        if(isDeath)
        {
            enemyDefeatCount++;
            // 一定数以上の敵を倒したら進行度をボーナス加算
            if(enemyDefeatCount == 4)
            {
                gameController.DecreaseEnemyProgress(gameController.EnemyDefeatBonusProgress);
            }
            // Strong Enemyを倒した場合はさらに進行度をボーナス加算
            if(enemy.enemyType == BaseEnemyController.EnemyType.Strong)
            {
                gameController.DecreaseEnemyProgress(gameController.StrongEnemyDefeatBonusProgress);
            }
        }

        // 貫通弾でなければ弾を消滅させる
        if (isPassThrough) return;
        Destroy(gameObject); // 弾を消滅
    }
}
