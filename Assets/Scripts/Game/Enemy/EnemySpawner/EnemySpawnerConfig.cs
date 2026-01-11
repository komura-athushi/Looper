using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LOOPER/Enemy Spawner Config")]
public class EnemySpawnerConfig : ScriptableObject
{
    [System.Serializable]
    public struct EnemyInfo
    {
        [Tooltip("敵のタイプ")]
        public BaseEnemyController.EnemyType enemyType;
        [Tooltip("敵のPrefab")]
        public GameObject enemyPrefab;
    }
    [Tooltip("スポーンする敵の情報リスト(タイプとprefab)")]
    public List<EnemyInfo> enemyInfos = new List<EnemyInfo>();

    [System.Serializable]
    public struct SpawnInfos
    {
        // 敵のタイプ
        [Tooltip("敵のタイプ")]
        public BaseEnemyController.EnemyType enemyType;
        [Tooltip("敵のスポーン確率(最低) Max: 100")]
        // スポーン確率(最低)
        public float spawnProbabilityMin;
        [Tooltip("敵のスポーン確率(最高) Max: 100")]
        // スポーン確率(最高)
        public float spawnProbabilityMax;
    }

    [System.Serializable]
    public struct EnemyWaveInfo
    {
        [Tooltip("Wave番号")]
        public int waveNumber; // Wave番号
        [Tooltip("Wave開始位置(プレイヤーの移動距離)")]
        public float spawnStartDistance; // Wave開始位置(プレイヤーの移動距離)
        [Tooltip("敵のスポーン間隔(距離)")]
        public float spawnInterval; // 敵のスポーン間隔(距離)
        [Tooltip("スポーンする敵の情報リスト")]
        public List<SpawnInfos> spawnInfos; // スポーンする敵の情報リスト
    }
    [Tooltip("敵のWave情報リスト")]
    public List<EnemyWaveInfo> enemyWaveInfos = new List<EnemyWaveInfo>();
}
