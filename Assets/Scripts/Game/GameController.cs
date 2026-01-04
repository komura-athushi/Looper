using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        GameClear,
        Failed
    }

    [Header("設定")]
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private ResultView resultView;
    [SerializeField] private PlayerController playerController;

    private Transform playerTransform;

    private float elapsedTime = 0f;
    private float traveledDistance = 0f;
    private float goalDistance; // プレイヤーとゴールの初期距離
    
    // 現在のプレイヤー速度（操作によって変化）
    private float currentPlayerSpeed;
    
    // ゲームステート管理
    private GameState currentState = GameState.Playing;

    // 距離表示用にpublicプロパティを提供
    public float TraveledDistance => traveledDistance;
    public float RemainingDistance => Mathf.Max(0, goalDistance - traveledDistance);
    public float CurrentPlayerSpeed => currentPlayerSpeed;
    // プレイヤーのTransform参照を公開
    public Transform PlayerTransform => playerTransform;
    
    // ゲーム開始からの経過時間
    public float ElapsedTime => elapsedTime;
    
    // 現在のゲームステート
    public GameState CurrentState => currentState;

    // エネミー進行度関連
    private EnemyProgressGaugeController enemyProgressGaugeController;

    private void Start()
    {
        // プレイヤーをコンポーネントで取得
        PlayerController playerCtrl = UnityEngine.Object.FindFirstObjectByType<PlayerController>();
        if (playerCtrl != null)
        {
            playerTransform = playerCtrl.transform;
        }
        else
        {
            Debug.LogError("PlayerController コンポーネントが見つかりません！Playerオブジェクトに PlayerController がアタッチされているか確認してください。");
        }

        currentPlayerSpeed = gameConfig.playerSpeed;

        // GameConfigから直接ゴールまでの距離を取得
        goalDistance = gameConfig.distanceToGoal;


        // ゲーム開始時にゴールを生成
        SpawnGoal();


        enemyProgressGaugeController = GetComponent<EnemyProgressGaugeController>();
        if (enemyProgressGaugeController == null)
        {
            Debug.LogWarning("[GameController] enemyProgressGaugeController が設定されておらず、同じ GameObject 上にも見つかりませんでした。Inspectorで割り当てるか、同じオブジェクトにコンポーネントを追加してください。");
        }
        
    }

    private void Update()
    {
        // ゲーム中のみ時間・距離を更新
        if (currentState == GameState.Playing)
        {
            UpdatePlayerSpeed();
            MeasureTimeAndDistance();
            UpdateEnemyProgress();
        }
        
        // ゲームクリア or Failed時のリトライ入力チェック
        if ((currentState == GameState.GameClear || currentState == GameState.Failed) &&
            Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            RetryGame();
        }
    }

    private void SpawnGoal()
    {
        if (gameConfig.goalPrefab == null)
        {
            Debug.LogError("ゴールのプレハブが設定されていません！");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("プレイヤーが設定されていません！");
            return;
        }

        // プレイヤーから右に goalDistance 離れた位置に生成
        Vector3 spawnPosition = playerTransform.position + Vector3.right * goalDistance;
        spawnPosition.z = 0f;

        // ゴールを生成
        GameObject goalObject = Instantiate(gameConfig.goalPrefab, spawnPosition, Quaternion.identity);
        
        // ゴールにGameControllerを設定
        GoalController goal = goalObject.GetComponent<GoalController>();
        if (goal != null)
        {
            goal.Initialize(this);
        }

        float estimatedTime = goalDistance / gameConfig.playerSpeed;
        Debug.Log($"ゴールを生成しました！プレイヤー位置: {playerTransform.position.x:F1}, ゴール位置: {spawnPosition.x:F1}, 距離: {goalDistance:F1}m (標準速度での到達予想時間: {estimatedTime:F1}秒)");
    }

    private void UpdatePlayerSpeed()
    {
        if (playerController == null) return;

        // PlayerControllerのGhost状態をチェック
        if (playerController.IsGhost)
        {
            // Ghost状態なら速度を0に
            currentPlayerSpeed = 0f;
        }
        else
        {
            // Ghost状態でないなら加速
            currentPlayerSpeed = Mathf.Min(currentPlayerSpeed + gameConfig.playerSpeedAcceleration * Time.deltaTime, gameConfig.playerSpeed);
        }
    }

    private void MeasureTimeAndDistance()
    {
        // 経過時間を追跡
        elapsedTime += Time.deltaTime;
        
        // プレイヤーは右に向かって進んでいる設定
        // 時間経過によって距離を計算（仮想的な移動）
        traveledDistance += currentPlayerSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ゲームクリア処理
    /// </summary>
    public void SetGameClear()
    {
        if (currentState != GameState.Playing) return;
    
        currentState = GameState.GameClear;
        Time.timeScale = 0f; // ゲーム全体を停止
        
        Debug.Log($"ゲームクリア！経過時間: {elapsedTime:F2}秒");
        
        if(resultView == null)
        {
            Debug.LogError("ResultView が設定されていません！");
            return;
        }
        resultView.ShowGameClear();
        playerController.ForceExitGhostMode();
    }

    public void SetGameFailed()
    {
        if (currentState != GameState.Playing) return;
    
        currentState = GameState.Failed;
        Time.timeScale = 0f; // ゲーム全体を停止
        
        Debug.Log("Failed!");
        
        if(resultView == null)
        {
            Debug.LogError("ResultView が設定されていません！");
            return;
        }
        resultView.ShowGameFailed();
        playerController.ForceExitGhostMode();
    }

    /// <summary>
    /// ゲームをリトライ
    /// </summary>
    private void RetryGame()
    {
        Time.timeScale = 1f; // タイムスケールをリセット
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateEnemyProgress()
    {
        // enemyProgressが100%を超えたら、Failedにする
        if (enemyProgressGaugeController.Normalized >= 1f)
        {
            SetGameFailed();
        }
    }
}
