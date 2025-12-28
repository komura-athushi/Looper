using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    [SerializeField] protected EnemyConfig config;
    protected Rigidbody2D rb;
    protected int hp;
    private float lifeTime;
    
    protected virtual void Start()
    {
        hp = config.hp;
        lifeTime = config.lifeTime;
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
}