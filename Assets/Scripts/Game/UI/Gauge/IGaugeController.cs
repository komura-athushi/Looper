using System;

/// <summary>
/// ゲージコントローラーの共通インターフェース
/// </summary>
public interface IGaugeController
{
    /// <summary>
    /// ゲージ値が変更されたときに発火するイベント
    /// </summary>
    event Action<float, float> OnGaugeChanged;

    /// <summary>
    /// 現在のゲージ値
    /// </summary>
    float Current { get; }

    /// <summary>
    /// ゲージの最大値
    /// </summary>
    float Max { get; }

    /// <summary>
    /// 正規化されたゲージ値 (0.0 ~ 1.0)
    /// </summary>
    float Normalized { get; }
}
