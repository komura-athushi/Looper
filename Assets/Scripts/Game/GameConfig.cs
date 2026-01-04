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

    [Tooltip("エネミー進行度の増加量（1秒あたり、100が最大値）")]
    public float enemyProgressIncreasePerSecond = 0.4f;
}
