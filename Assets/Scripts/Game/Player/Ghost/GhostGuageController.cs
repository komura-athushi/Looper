using System;
using UnityEngine;

public class GhostGuageController : MonoBehaviour, IGaugeController
{
    [SerializeField] private GaugeConfig config;
    [SerializeField] private GameController gameController;

    public event Action<float, float> OnGaugeChanged;

    public float Current => _model.Current;
    public float Max => _model.Max;
    public float Normalized => _model.Normalized;

    private GaugeModel _model;  // こいつの値を弄るとゲージが変化する
    private float _regenBlockTimer;
    private bool _isRegenStopped = false; // 回復停止フラグ

    private void Awake()
    {
        _model = new GaugeModel(config.max, 0.0f);
        Notify();
    }

    private void Update()
    {
        // GameControllerがないか Playing ステートでない場合は回復しない
        if (gameController == null)
        {
            gameController = FindFirstObjectByType<GameController>();
            if (gameController == null) return;
        }
        
        if (gameController.CurrentState != GameController.GameState.Playing)
        {
            return;
        }

        if (_regenBlockTimer > 0f)
        {
            _regenBlockTimer -= Time.deltaTime;
            return;
        }

        // 回復停止中は回復しない
        if (_isRegenStopped)
        {
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

    // ゲージを0にする
    public void ConsumeAll()
    {
        _model.ConsumeAll();
        _regenBlockTimer = config.regenDelayAfterUse;
        Notify();
    }

    // 回復を停止する
    public void StopRegen()
    {
        _isRegenStopped = true;
    }

    // 回復を再開する
    public void ResumeRegen()
    {
        _isRegenStopped = false;
    }

    private void Notify()
    {
        // ゲージ消費をviewに通知
        OnGaugeChanged?.Invoke(_model.Current, _model.Max);
    }
}
