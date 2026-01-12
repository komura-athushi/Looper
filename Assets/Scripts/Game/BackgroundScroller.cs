using UnityEngine;

/// <summary>
/// 背景のUVスクロールを制御するクラス
/// GameControllerのCurrentPlayerSpeedに基づいて背景をスクロールさせる
/// </summary>
public class BackgroundScroller : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameController gameController;
    [SerializeField] private Renderer targetRenderer;
    [Tooltip("スクロール速度の倍率（負の値で逆方向）")]
    [SerializeField] private float scrollMultiplier = 1f;
    [Tooltip("スクロールさせるマテリアルのテクスチャプロパティ名")]
    [SerializeField] private string texturePropertyName = "_MainTex";
    [Tooltip("テクスチャの繰り返し回数（X方向）")]
    [SerializeField] private float tilingX = 1f;

    private Material material;
    private Vector2 offset;
    private float backgroundWidth; // 背景の幅（Unity単位）
    private float uvScrollSpeed; // UV座標でのスクロール速度

    private void Start()
    {
        // GameControllerが未設定の場合は検索
        if (gameController == null)
        {
            gameController = FindFirstObjectByType<GameController>();
            if (gameController == null)
            {
                Debug.LogError("[BackgroundScroller] GameControllerが見つかりません！");
                enabled = false;
                return;
            }
        }

        // Rendererが未設定の場合は自分自身から取得
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
            if (targetRenderer == null)
            {
                Debug.LogError("[BackgroundScroller] Rendererが見つかりません！このコンポーネントをRendererを持つオブジェクトにアタッチするか、Inspectorで設定してください。");
                enabled = false;
                return;
            }
        }

        // マテリアルのインスタンスを作成（元のマテリアルを変更しないため）
        material = targetRenderer.material;
        
        offset = material.GetTextureOffset(texturePropertyName);
        
        // 背景の幅を取得（SpriteRendererまたはMeshRendererに対応）
        backgroundWidth = targetRenderer.bounds.size.x;
        
        // UV座標でのスクロール速度を計算
        // プレイヤーが1Unity単位移動するとき、UVは (Tiling / 背景の幅) だけ移動する必要がある
        uvScrollSpeed = tilingX / backgroundWidth;
        
        Debug.Log($"[BackgroundScroller] 初期化完了 - 背景幅: {backgroundWidth:F2}, Tiling: {tilingX}, UVスクロール速度係数: {uvScrollSpeed:F4}");
    }

    private void Update()
    {
        if (gameController == null || material == null) return;

        // CurrentPlayerSpeedに基づいてUVオフセットを更新
        // プレイヤー速度(Unity単位/秒) × UVスクロール速度係数 × 倍率
        float scrollSpeed = gameController.CurrentPlayerSpeed * uvScrollSpeed * scrollMultiplier;
        offset.x += scrollSpeed * Time.deltaTime;

        // オフセットを適用
        material.SetTextureOffset(texturePropertyName, offset);
        
        // デバッグログ
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"[BackgroundScroller] PlayerSpeed: {gameController.CurrentPlayerSpeed:F2}, UVSpeed係数: {uvScrollSpeed:F4}, ScrollSpeed: {scrollSpeed:F4}, Offset: {offset.x:F3}");
        }
    }

    private void OnDestroy()
    {
        // インスタンス化したマテリアルを破棄してメモリリークを防ぐ
        if (material != null)
        {
            Destroy(material);
        }
    }
}
