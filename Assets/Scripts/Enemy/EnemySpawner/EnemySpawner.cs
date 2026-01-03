using UnityEngine;

public class EnemySpawner: MonoBehaviour
{
    [SerializeField] private EnemySpawnerConfig spawnerConfig;
    private GameObject enemyPrefab;
    private float spawnInterval;
    private float _spawnTimer = 0f;

    private void Start()
    {
        // 初期化処理があればここに記述
        enemyPrefab = spawnerConfig.enemyPrefab;
        spawnInterval = spawnerConfig.spawnInterval;
    }
    
    private void Update()
    {
        // スポーンのタイミングや条件をここで管理
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnInterval)
        {
            SpawnEnemy(transform.position);
            _spawnTimer = 0f;
        }
    }

    public void SpawnEnemy(Vector3 position)
    {
        // Enemyを生成
        GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
