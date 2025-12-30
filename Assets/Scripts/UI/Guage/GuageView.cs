using UnityEngine;
using UnityEngine.UI;

// ゲージ表示用
// 他クラスからの参照は想定していない
public class GaugeView : MonoBehaviour
{
    [SerializeField] private PlayerGaugeController gauge;
    [SerializeField] private Image fillImage;

    private void Start()
    {
        if (gauge == null) return;
        gauge.OnGaugeChanged += OnChanged;
        OnChanged(gauge.Current, gauge.Max);
    }

    // 自動で呼ばれる
    private void OnDisable()
    {
        if (gauge == null) return;
        gauge.OnGaugeChanged -= OnChanged;
    }

    // 自動で呼ばれる
    private void OnChanged(float current, float max)
    {
        fillImage.fillAmount = max <= 0 ? 0 : current / max;
    }
}
