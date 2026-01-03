using UnityEngine;

public class EnemySpawner: MonoBehaviour
{
    [SerializeField] private EnemySpawnerConfig spawnerConfig;
    private PlayerController player;
    private GameObject enemyPrefab;
    private float spawnInterval;
    private float _spawnTimer = 0f;
    private float screenRightEdge;

    private void Start()
    {
        // 初期化処理があればここに記述
        enemyPrefab = spawnerConfig.enemyPrefab;
        spawnInterval = spawnerConfig.spawnInterval;
        
        // 画面右端の位置を計算
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            screenRightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        }
        
        // Playerが設定されていなければ自動で探す
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
    }
    
    private void Update()
    {
        // スポーンのタイミングや条件をここで管理
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        // 画面右端のx座標とPlayerのy座標を使用
        Vector3 spawnPosition = new Vector3(screenRightEdge, player.transform.position.y, 0);
        
        // Enemyを生成
        GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
