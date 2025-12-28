using UnityEngine;

// ゲージ消費処理
public sealed class GaugeModel
{
    public float Max { get; }
    public float Current { get; private set; }
     public float Normalized => Max <= 0 ? 0 : Mathf.Clamp01(Current / Max);

    public GaugeModel(float max, float initial)
    {
        Max = Mathf.Max(0.0001f, max);
        Current = Mathf.Clamp(initial, 0, Max);
    }

    // ゲージ消費
    public void TryConsume(float amount)
    {
        amount = Mathf.Max(0, amount);
        Current -= amount;
        // 0より小さくなったら0に戻す
        if(Current < 0) Current = 0;
    }

    // ゲージを0にする
    public void ConsumeAll()
    {
        Current = 0f;
    }

    // ゲージを回復する
    public void Regen(float amount)
    {
        if (amount <= 0) return;
        Current = Mathf.Min(Max, Current + amount);
    }
}
