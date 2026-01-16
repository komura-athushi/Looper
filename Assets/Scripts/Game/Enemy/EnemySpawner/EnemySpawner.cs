using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner: MonoBehaviour
{
    [SerializeField] private EnemySpawnerConfig spawnerConfig;
    private GameController gameController;
    private PlayerController player;
    private float spawnDistanceTimer = 0f;
    private float screenRightEdge;
    private int currentWaveNumber = 1;
    private int currentSpawnedEnemyCount = 0;
    private EnemySpawnerConfig.EnemyWaveInfo? currentWaveInfo;
    private EnemySpawnerConfig.EnemyWaveInfo? nextWaveInfo;
    private float previousPlayerDistance = 0f;

    // Enemyのprefabとスポーン確率を管理するための辞書
    class EnemyInfo
    {
        public GameObject enemyPrefab;
        public float spawnProbability;
    }
    // EnemyTypeごとのEnemyInfoを格納する辞書
    Dictionary<BaseEnemyController.EnemyType, EnemyInfo> enemyInfoDict = new Dictionary<BaseEnemyController.EnemyType, EnemyInfo>();


    private void Start()
    {
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
        gameController = FindFirstObjectByType<GameController>();

        // 最初と次のWave情報を取得
        currentWaveInfo = GetWaveInfo(currentWaveNumber);
        nextWaveInfo = GetWaveInfo(currentWaveNumber + 1);
        previousPlayerDistance = gameController.TraveledDistance;

        // EnemyInfo辞書の初期化
        foreach (var enemyInfo in spawnerConfig.enemyInfos)
        {
            enemyInfoDict[enemyInfo.enemyType] = new EnemyInfo
            {
                enemyPrefab = enemyInfo.enemyPrefab,
                spawnProbability = 0f // 初期値
            };
        }
    }
    
    private void Update()
    {
        // GameControllerのStateがPlayingでなければ処理しない
        if (gameController.CurrentState != GameController.GameState.Playing) return;


        // 現Waveの決定
        DecideWave();
        // 敵のスポーン決定
        DecideSpawnEnemy();

        previousPlayerDistance = gameController.TraveledDistance;
    }

    private void DecideWave()
    {
        // 次Waveが無ければ戻る
        if (nextWaveInfo == null) return;
        // プレイヤーの進んだ距離が次Waveの開始距離に達していなければ戻る
        if (gameController.TraveledDistance < nextWaveInfo?.spawnStartDistance) return;
        
        currentWaveNumber++;
        currentWaveInfo = nextWaveInfo;
        nextWaveInfo = GetWaveInfo(currentWaveNumber + 1);
    }

    private void DecideSpawnEnemy()
    {
        // プレイヤーの進んだ距離を加算
        spawnDistanceTimer += gameController.TraveledDistance - previousPlayerDistance;
        // まだスポーン間隔に達していなければ戻る
        if(spawnDistanceTimer < (currentWaveInfo?.spawnInterval)) return;
        // タイマーをリセット
        spawnDistanceTimer = 0f;

        // 敵のスポーン確率を計算&加算
        float totalProbability = 0f;
        foreach (var spawnInfo in currentWaveInfo?.spawnInfos)
        {
            // 最小/最大確率の間でランダムに決定して合計確率に加算
            float probability = Random.Range(spawnInfo.spawnProbabilityMin, spawnInfo.spawnProbabilityMax);
            enemyInfoDict[spawnInfo.enemyType].spawnProbability += probability;
            totalProbability += probability;
        }

        // 一番spawnProbabilityが高いEnemyInfoを探索
        EnemyInfo selectedEnemyInfo = null;
        float highestProbability = -1f;
        foreach (var enemyInfo in enemyInfoDict.Values)
        {
            if (enemyInfo.spawnProbability > highestProbability)
            {
                highestProbability = enemyInfo.spawnProbability;
                selectedEnemyInfo = enemyInfo;
            }
        }
        if(highestProbability < totalProbability) return; // 合計確率に達していなければスポーンしない

        // 敵をスポーン
        SpawnEnemy(selectedEnemyInfo.enemyPrefab);
        // スポーンしたEnemyのspawnProbabilityから合計確率を減算
        selectedEnemyInfo.spawnProbability -= totalProbability;
    }

    // 指定したWaveNumberに対応するWaveInfoを取得するヘルパー関数
    private EnemySpawnerConfig.EnemyWaveInfo? GetWaveInfo(int waveNumber)
    {
        // 指定したWaveNumberに対応するWaveInfoを取得
        foreach (var wave in spawnerConfig.enemyWaveInfos)
        {
            if (wave.waveNumber == waveNumber)
            {
                return wave;
            }
        }

        // 見つからなければnullを返す
        return null;
    }

    // Enemyをスポーンさせる
    public void SpawnEnemy(GameObject enemyPrefab)
    {
        // 画面右端のx座標とPlayerのy座標を使用
        Vector3 spawnPosition = new Vector3(screenRightEdge, player.transform.position.y, 0);
        
        // Enemyを生成
        var enemy = GameObject.Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        var baseEnemyController = enemy.GetComponent<BaseEnemyController>();
        baseEnemyController.enemySpawner = this;
        baseEnemyController.gameController = gameController;
        currentSpawnedEnemyCount++;
    }

    public void DecreaseSpawnedEnemyCount()
    {
        currentSpawnedEnemyCount = Mathf.Max(0, currentSpawnedEnemyCount - 1);
    }
}
