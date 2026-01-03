using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        // 他の敵タイプを追加可能
    }


    [SerializeField] protected EnemyConfig config;
    protected Rigidbody2D rb;
    protected int hp;
    private float lifeTime;
    public BaseEnemyController.EnemyType enemyType;
    
    protected virtual void Start()
    {
        hp = config.hp;
        lifeTime = config.lifeTime;
        enemyType = config.enemyType;
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update() {
        MovePattern();

        lifeTime -= Time.deltaTime;
        // 生存時間を超えたら消滅
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    protected abstract void MovePattern(); // 各子クラスで実装
    public virtual void TakeDamage(int damage) { /* 共通 */ }

    // プレイヤーに接触したときの処理
    // ダメージを与えて自分は消滅
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if(playerController == null) return;
        
        bool isPlayerHit = playerController.TakeDamage(1);
        if (!isPlayerHit) return;
        
        Destroy(gameObject);
    }
}