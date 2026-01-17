using System;
using UnityEngine;

public class EnemyProgressGaugeController : MonoBehaviour, IGaugeController
{
    [SerializeField] private GaugeConfig config;
    [SerializeField] private GameController gameController;

    public event Action<float, float> OnGaugeChanged;

    public float Current => _model.Current;
    public float Max => _model.Max;
    public float Normalized => _model.Normalized;

    private GaugeModel _model;  // こいつの値を弄るとゲージが変化する
    private float _regenBlockTimer;

    private void Awake()
    {
        // GameControllerが設定されていない場合は自動取得
        if (gameController == null)
        {
            gameController = FindFirstObjectByType<GameController>();
        }
        
        _model = new GaugeModel(config.max, 0.0f);
        Notify();
    }

    private void Update()
    {
        // GameStateがPlaying以外の場合はゲージ操作を行わない
        if (gameController != null && gameController.CurrentState != GameController.GameState.Playing)
        {
            return;
        }
        
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
        _model.TryConsume(amount);

        _regenBlockTimer = config.regenDelayAfterUse;
        Notify();
        return true;
    }

    public void Regen(float amount)
    {
        _model.Regen(amount);
        Notify();
    }

    private void Notify()
    {
        // ゲージ消費をviewに通知
        OnGaugeChanged?.Invoke(_model.Current, _model.Max);
    }
}
