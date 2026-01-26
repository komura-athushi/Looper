using UnityEngine;
using System;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GhostCostConfig ghostCostConfig;
    [SerializeField] private GhostGuageController gauge;

    private bool _isGhostMode = false;

    // Ghost状態の変化を通知するイベント
    public event Action<bool> OnGhostModeChanged;

    // Ghost状態かどうかを外部から取得
    public bool IsGhostMode => _isGhostMode;
    // Ghost状態に入るまでの長押し時間
    public float ActivationTime => ghostCostConfig != null ? ghostCostConfig.activationTime : 0.3f;

    private void Update()
    {
        if(!_isGhostMode) return;

        // Ghost状態中はゲージを消費
        gauge.TryConsume(ghostCostConfig.ghostCost * Time.deltaTime);
        // ゲージが尽きたらGhost状態を終了
        if (gauge.Current <= 0f)
        {
            StopGhostMode();
        }
    }

    // Ghost状態を開始
    public void StartGhostMode()
    {
        if (_isGhostMode) return; // 既にGhost状態なら何もしない

        _isGhostMode = true;
        gauge.StopRegen();
        OnGhostModeChanged?.Invoke(true);
        AudioManager.Instance.PlaySE("PlayerGhost");
    }

    // Ghost状態を停止
    public void StopGhostMode()
    {
        if (!_isGhostMode) return; // Ghost状態でなければ何もしない
        _isGhostMode = false;
        gauge.ResumeRegen();
        OnGhostModeChanged?.Invoke(false);
    }
}
