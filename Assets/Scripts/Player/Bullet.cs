using UnityEngine;

// プレイヤーが発射する弾のスクリプト
public class Bullet : MonoBehaviour
{
    // 弾のスピード
    [SerializeField] private float speed = 12f;
    // 弾が生成されて消えるまでの時間
    [SerializeField] private float lifeTime = 4f;

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
}
