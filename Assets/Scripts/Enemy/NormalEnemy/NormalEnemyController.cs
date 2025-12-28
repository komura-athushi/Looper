using UnityEngine;

public class NormalEnemyController : BaseEnemyController
{
    protected override void MovePattern()
    {
        // 左に移動するだけ
        Vector2 newPosition = rb.position + Vector2.left * (config.speed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
