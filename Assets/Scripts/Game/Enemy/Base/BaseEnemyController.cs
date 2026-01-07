using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Normal,
        // 他の敵タイプを追加可能
        Hopper,
        Strong
    }


    [SerializeField] protected EnemyConfig config;
    protected Rigidbody2D rb;
    protected int hp;
    private float screenLeftEdge;
    public EnemyType enemyType;
    public GameController gameController;
    public EnemySpawner enemySpawner;
    
    protected virtual void Start()
    {
        hp = config.hp;
        enemyType = config.enemyType;
        rb = GetComponent<Rigidbody2D>();
        
        // 画面左端の位置を計算
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenLeftEdge = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        }
    }

    protected virtual void OnDestroy()
    {
        // 敵が破壊されたときにスポーナーに通知
        enemySpawner.DecreaseSpawnedEnemyCount();
    }

    protected virtual void Update() {
        // 画面左端を超えたら削除
        if (transform.position.x < screenLeftEdge)
        {
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
        MovePattern();
    }
    
    protected abstract void MovePattern(); // 各子クラスで実装
    public virtual void TakeDamage(int damage, BulletConfig.BulletType bulletType) { /* 共通 */ }

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