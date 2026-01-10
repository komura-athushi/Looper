using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 汎用的なゲージ表示クラス
/// IGaugeControllerインターフェースを実装したコントローラーで使用可能
/// </summary>
public class GaugeView : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject targetObject;

    private IGaugeController gaugeController;

    private void Start()
    {
        // GameObjectからIGaugeControllerを実装したコンポーネントを取得
        if (targetObject != null)
        {
            gaugeController = targetObject.GetComponent<IGaugeController>();
            
            if (gaugeController == null)
            {
                Debug.LogError($"[GaugeView] {targetObject.name} には IGaugeController を実装したコンポーネントがありません");
                return;
            }
        }
        
        gaugeController.OnGaugeChanged += OnChanged;
        OnChanged(gaugeController.Current, gaugeController.Max);
    }

    // 自動で呼ばれる
    private void OnDisable()
    {
        if (gaugeController == null) return;
        gaugeController.OnGaugeChanged -= OnChanged;
    }

    // 自動で呼ばれる
    private void OnChanged(float current, float max)
    {
        fillImage.fillAmount = max <= 0 ? 0 : current / max;
    }
}
