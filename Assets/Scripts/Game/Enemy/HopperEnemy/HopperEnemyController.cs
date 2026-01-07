using UnityEngine;

public class HopperEnemyController : BaseEnemyController
{
    private HopperEnemyConfig hopperConfig;
    private float switchTimer = 0f;
    private float verticalDirection = 1f; // 1: 上移動, -1: 下移動

    protected override void Start()
    {
        base.Start();   // 基底クラスのStartを呼び出す
        hopperConfig = (HopperEnemyConfig)config;
    }

    // FixedUpdateで呼ばれる
    protected override void MovePattern()
    {
        // 縦移動の切り替え処理
        switchTimer += Time.fixedDeltaTime;

        if (switchTimer >= hopperConfig.switchInterval)
        {
            verticalDirection *= -1f; // 移動方向を反転
            switchTimer = 0f;
        }

        // 左に移動しつつ、縦にも移動
        Vector2 newPosition = rb.position + Vector2.left * ((config.horizontalSpeed + gameController.CurrentPlayerSpeed) * Time.fixedDeltaTime)
                                         + Vector2.up * (verticalDirection * hopperConfig.verticalSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    public override void TakeDamage(int damage, BulletConfig.BulletType bulletType)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
