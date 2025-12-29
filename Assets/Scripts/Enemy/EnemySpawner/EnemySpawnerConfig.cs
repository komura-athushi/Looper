using UnityEngine;

[CreateAssetMenu(menuName = "LOOPER/Enemy Spawner Config")]
public class EnemySpawnerConfig : ScriptableObject
{
    public GameObject enemyPrefab;      // スポーン対象のプリファブ
    public float spawnInterval = 2f;    // スポーン間隔
    public BaseEnemyController.EnemyType enemyType; // スポーンする敵のタイプ
}
