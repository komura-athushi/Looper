using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "LOOPER/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("プレイヤー設定")]
    [Tooltip("プレイヤーの標準速度（1秒ごとに進む距離）")]
    public float playerSpeed = 5f;
    [Tooltip("Ghost解除後のプレイヤー移動速度の加速度(1秒あたり)")]
    public float playerSpeedAcceleration = 0.5f;


    [Header("ゴール設定")]
    [Tooltip("プレイヤーとゴールの初期距離")]
    public float distanceToGoal = 100f;
    
    [Tooltip("ゴールのプレハブ")]
    public GameObject goalPrefab;

    [Tooltip("Enemyを4体以上同時に倒したときのボーナス進行度(最大値1)")]
    public float enemyDefeatBonusProgress = 0.01f;
    [Tooltip("Strong Enemyを倒したときのボーナス進行度(最大値1)")]
    public float strongEnemyDefeatBonusProgress = 0.03f;
}
