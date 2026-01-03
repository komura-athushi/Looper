using System;
using UnityEngine;

public class PlayerGaugeController : MonoBehaviour
{
    [SerializeField] private GaugeConfig config;

    public event Action<float, float> OnGaugeChanged;

    public float Current => _model.Current;
    public float Max => _model.Max;
    public float Normalized => _model.Normalized;

    private GaugeModel _model;  // こいつの値を弄るとゲージが変化する
    private float _regenBlockTimer;

    private void Awake()
    {
        _model = new GaugeModel(config.max, 0.0f);
        Notify();
    }

    private void Update()
    {
        if (_regenBlockTimer > 0f)
        {
            _regenBlockTimer -= Time.deltaTime;
            return;
        }

        _model.Regen(config.regenPerSec * Time.deltaTime);
        Notify();
    }

    // ゲージを消費する
    public bool TryConsume(float amount)
    {
        // 消費可能かチェック
        if(!_model.CanConsume(amount))
        {
            return false;
        }
        _model.TryConsume(amount);

        _regenBlockTimer = config.regenDelayAfterUse;
        Notify();
        return true;
    }

    // ゲージを0にする
    public void ConsumeAll()
    {
        _model.ConsumeAll();
        _regenBlockTimer = config.regenDelayAfterUse;
        Notify();
    }

    private void Notify()
    {
        // ゲージ消費をviewに通知
        OnGaugeChanged?.Invoke(_model.Current, _model.Max);
    }
}
