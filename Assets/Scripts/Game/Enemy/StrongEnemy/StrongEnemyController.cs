using UnityEngine;

public class StrongEnemyController : BaseEnemyController
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
        AudioManager.Instance.PlaySE("EnemyDamaged");
        if(bulletType == BulletConfig.BulletType.Burst)
        {
            // 貫通弾の場合、HPを0にする
            hp = 0;
        }

        if (hp <= 0)
        {
            Destroy(gameObject);
            AudioManager.Instance.PlaySE("BossDeath");
            return true;
        }
        return false;
    }
}
