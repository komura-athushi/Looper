using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// BGMとSEのボリューム調整UIを管理するクラス
/// </summary>
public class AudioSettingsUI : MonoBehaviour
{
    [Header("BGM設定")]
    [SerializeField] private Slider bgmSlider;
    
    [Header("SE設定")]
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        InitializeSliders();
    }

    private void InitializeSliders()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager.Instance が見つかりません");
            return;
        }

        // BGMスライダーの初期化
        if (bgmSlider != null)
        {
            bgmSlider.minValue = 0f;
            bgmSlider.maxValue = 1f;
            bgmSlider.value = AudioManager.Instance.GetBGMVolume();
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }

        // SEスライダーの初期化
        if (seSlider != null)
        {
            seSlider.minValue = 0f;
            seSlider.maxValue = 1f;
            seSlider.value = AudioManager.Instance.GetSEVolume();
            seSlider.onValueChanged.AddListener(OnSEVolumeChanged);
        }
    }

    private void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
        
    }

    private void OnSEVolumeChanged(float value)
    {
        AudioManager.Instance.SetSEVolume(value);
    }

    private void OnDestroy()
    {
        // リスナーの削除
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
        }
        if (seSlider != null)
        {
            seSlider.onValueChanged.RemoveListener(OnSEVolumeChanged);
        }
    }
}
