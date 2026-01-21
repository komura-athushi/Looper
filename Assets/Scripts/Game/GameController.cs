using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        FadeIn,
        MovePlayer,
        CountStartTimer,
        Playing,
        GameClear,
        Failed,
        FadeOut,
    }

    [Header("設定")]
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private ResultView resultView;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CastleController castleController;
    [SerializeField] private CountNumberController countNumberController;

    private Transform playerTransform;

    private float elapsedTime = 0f;
    private float traveledDistance = 0f;
    private float goalDistance; // プレイヤーとゴールの初期距離
    
    // 現在のプレイヤー速度（操作によって変化）
    private float currentPlayerSpeed;
    
    // ゲームステート管理
    private GameState currentState = GameState.FadeIn;
    
    // フェード関連
    private bool isWaitingForFadeIn = true;
    
    // カウントダウンタイマー関連
    private float countdownTimer = 3f;
    private int lastDisplayedNumber = 3;

    // 距離表示用にpublicプロパティを提供
    public float TraveledDistance => traveledDistance;
    public float RemainingDistance => Mathf.Max(0, goalDistance - traveledDistance);
    // 現在のプレイヤー速度(1秒あたり)
    public float CurrentPlayerSpeed => currentPlayerSpeed;
    // プレイヤーのTransform参照を公開
    public Transform PlayerTransform => playerTransform;
    
    // ゲーム開始からの経過時間
    public float ElapsedTime => elapsedTime;
    
    // 現在のゲームステート
    public GameState CurrentState => currentState;
    // エネミー進行度関連
    private EnemyProgressGaugeController enemyProgressGaugeController;
    public float EnemyDefeatBonusProgress => gameConfig.enemyDefeatBonusProgress;
    public float StrongEnemyDefeatBonusProgress => gameConfig.strongEnemyDefeatBonusProgress;


    private void Start()
    {
        // フェードイン中はゲームを停止
        Time.timeScale = 0f;
        
        // シーンロード完了後にフェードイン開始
        StartCoroutine(StartFadeInAfterLoad());
        
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
    
    private IEnumerator StartFadeInAfterLoad()
    {
        // 1フレーム待機してシーンロード完了を待つ
        yield return null;
        
        // フェードイン開始
        FadeController.Instance.StartFadeIn();
    }

    private void Update()
    {
        // フェードイン待機中
        if (isWaitingForFadeIn)
        {
            if (FadeController.Instance.IsFadeIn())
            {
                isWaitingForFadeIn = false;
                Time.timeScale = 1f; // ゲーム開始
                currentState = GameState.MovePlayer; // ステートをMovePlayerに変更
                Debug.Log("フェード完了 - ステートをMovePlayerに変更");
            }
            return;
        }
        
        // MovePlayerステート（Castle移動中）
        if (currentState == GameState.MovePlayer)
        {
            // CastleControllerをチェック
            if (castleController == null)
            {
                castleController = FindFirstObjectByType<CastleController>();
            }
            
            // Castleの移動が完了したらPlayingステートに変更
            if (castleController != null && !castleController.IsMoving)
            {
                currentState = GameState.CountStartTimer;
                Time.timeScale = 0f; // 一時停止
                countdownTimer = 3f; // カウントダウンタイマーを初期化
                lastDisplayedNumber = 3;
                
                // カウントダウン開始時に数字を表示
                Debug.Log($"[GameController] countNumberControllerが設定されています: {countNumberController.name}");
                countNumberController.ShowNumber(3);
                
                Debug.Log("Castle移動完了 - ステートをCountStartTimerに変更");
            }
            return;
        }
        
        // CountStartTimerステート（カウントダウン中）
        if (currentState == GameState.CountStartTimer)
        {
            // unscaledDeltaTimeを使用してTime.timeScale=0でも時間を計測
            countdownTimer -= Time.unscaledDeltaTime;
            
            // 現在表示すべき数字を計算（3→2→1）
            int currentNumber = Mathf.CeilToInt(countdownTimer);
            
            // 数字が変わった時に表示を更新
            if (currentNumber != lastDisplayedNumber && currentNumber > 0)
            {
                lastDisplayedNumber = currentNumber;
                Debug.Log($"[GameController] ShowNumberを呼び出し: {currentNumber}");
                countNumberController.ShowNumber(currentNumber);
                Debug.Log($"カウントダウン: {currentNumber}");
            }
            
            // カウントダウン完了
            if (countdownTimer <= 0f)
            {
                // 数字を非表示
                countNumberController.HideNumber();
                
                // ゲーム開始
                currentState = GameState.Playing;
                Time.timeScale = 1f; // ゲーム再開
                AudioManager.Instance.PlaySound("game"); // BGM再生
                Debug.Log("カウントダウン完了 - ステートをPlayingに変更");
            }
            return;
        }
        
        // ゲーム中のみ時間・距離を更新
        if (currentState == GameState.Playing)
        {
            UpdatePlayerSpeed();
            MeasureTimeAndDistance();
            UpdateEnemyProgress();
            return;
        }
        
        // ゲームクリア or Failed時のリトライ入力チェック
        if ((currentState == GameState.GameClear || currentState == GameState.Failed) &&
            Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            RetryGame();
            return;
        }
        
        // FadeOut完了後のシーンリロード処理
        if (currentState == GameState.FadeOut && FadeController.Instance.IsFadeOut())
        {
            Time.timeScale = 1f; // タイムスケールをリセット
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
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
        spawnPosition.z = 1f;

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
        

        AudioManager.Instance.StopBGM();
        resultView.ShowGameClear();
        playerController.ForceExitGhostMode();
    }

    public void SetGameFailed()
    {
        if (currentState != GameState.Playing) return;
    
        currentState = GameState.Failed;
        Time.timeScale = 0f; // ゲーム全体を停止
        
        Debug.Log("Failed!");
        
        AudioManager.Instance.StopBGM();
        resultView.ShowGameFailed();
        playerController.ForceExitGhostMode();
    }

    /// <summary>
    /// ゲームをリトライ
    /// </summary>
    private void RetryGame()
    {
        currentState = GameState.FadeOut;
        FadeController.Instance.StartFadeOut();
    }

    private void UpdateEnemyProgress()
    {
        // enemyProgressが100%を超えたら、Failedにする
        if (enemyProgressGaugeController.Normalized >= 1f)
        {
            SetGameFailed();
        }
    }

    public void DecreaseEnemyProgress(float amount)
    {
        enemyProgressGaugeController.TryConsume(amount);
    }
}
