using UnityEngine;

public class NormalEnemyController : BaseEnemyController
{
    // FixedUpdateで呼ばれる
    protected override void MovePattern()
    {
        // 左に移動するだけ
        Vector2 newPosition = rb.position + Vector2.left * ((config.horizontalSpeed + gameController.CurrentPlayerSpeed) * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    public override bool TakeDamage(int damage, BulletConfig.BulletType bulletType)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
