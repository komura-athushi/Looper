using UnityEngine;

public class CastleController : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private bool autoFindGameController = true;
    [SerializeField] private GameController gameController;
    
    private Camera mainCamera;
    private bool isActive = true;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        // GameControllerの参照を取得
        if (autoFindGameController || gameController == null)
        {
            gameController = FindFirstObjectByType<GameController>();
        }
        
        // メインカメラの参照を取得
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("メインカメラが見つかりません！");
            enabled = false;
            return;
        }
        
        // SpriteRendererを取得
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRendererが見つかりません！このスクリプトにはSpriteRendererが必要です。");
            enabled = false;
            return;
        }
    }
    
    void Update()
    {
        if (!isActive || gameController == null) return;
        
        // 右から左に移動
        MoveLeftward();
        
        // 画面外に出たかチェック
        if (IsOutOfScreen())
        {
            StopMovement();
        }
    }
    
    /// <summary>
    /// 右から左へ移動する
    /// </summary>
    private void MoveLeftward()
    {
        float moveSpeed = gameController.CurrentPlayerSpeed * Time.deltaTime;
        transform.position += Vector3.left * moveSpeed;
    }
    
    /// <summary>
    /// オブジェクトの画像全体が画面外（左端）に出たかチェック
    /// </summary>
    private bool IsOutOfScreen()
    {
        // オブジェクトの右端の座標を取得（画像全体が消えるまで待つため）
        float rightEdge = GetObjectRightEdge();
        
        // 右端の座標をViewport座標に変換
        Vector3 rightEdgeWorldPos = new Vector3(rightEdge, transform.position.y, transform.position.z);
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(rightEdgeWorldPos);
        
        // Viewport座標で右端（x < 0）に出たら画像全体が画面外
        return viewportPos.x < 0;
    }
    
    /// <summary>
    /// オブジェクトの右端のワールド座標を取得
    /// </summary>
    private float GetObjectRightEdge()
    {
        float objectWidth = GetObjectWidth();
        return transform.position.x + objectWidth * 0.5f;
    }
    
    /// <summary>
    /// SpriteRendererから幅を取得
    /// </summary>
    private float GetObjectWidth()
    {
        return spriteRenderer.bounds.size.x;
    }
    
    /// <summary>
    /// 移動を停止する
    /// </summary>
    private void StopMovement()
    {
        isActive = false;
        Debug.Log($"{gameObject.name} が画面外に出たため移動を停止しました");
        
        // GameControllerに移動完了を通知
        if (gameController != null)
        {
            Debug.Log("CastleController: 移動完了をGameControllerに通知");
        }
    }
    
    /// <summary>
    /// 移動を再開する（外部から呼び出し可能）
    /// </summary>
    public void ResumeMovement()
    {
        isActive = true;
    }
    
    /// <summary>
    /// 移動状態を取得
    /// </summary>
    public bool IsMoving => isActive;
}
