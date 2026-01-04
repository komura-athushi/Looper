using UnityEngine;
using UnityEngine.InputSystem;
using System;

// プレイヤークラス
[RequireComponent(typeof(Gun))]
public class PlayerController : MonoBehaviour
{
    private Gun _gun; // 弾の発射するGunコンポーネント
    [SerializeField] private PlayerConfig config;
    private int hp;

    // 無敵状態関連
    private bool _isInvincible = false;  // 無敵状態フラグ
    private float _invincibilityTimer = 0f;  // 無敵時間カウンター
    private SpriteRenderer _spriteRenderer;  // 点滅用

    // HP変更時のイベント
    public event Action<int> OnHPChanged;

    private void Awake()
    {
        _gun = GetComponent<Gun>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        hp = config.maxHP;
        OnHPChanged?.Invoke(hp);
    }

    private void Update()
    {
        // 短押しショット
        // New Input System
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _gun.Fire();
        }

        // 無敵時間の更新
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
            {
                _isInvincible = false;
                _spriteRenderer.enabled = true;  // 点滅終了して表示
            }
            else
            {
                // 点滅処理
                _spriteRenderer.enabled = ((int)(_invincibilityTimer / config.blinkInterval)) % 2 == 0;
            }
        }
    }

    // ダメージを受ける
    public bool TakeDamage(int damage)
    {
        // 無敵状態ではダメージを受けない
        if (_isInvincible)
        {
            return false;
        }

        hp -= damage;

        if(hp <= 0)
        {
            // GameControllerに通知してゲームオーバー処理を行う
            GameController gameController = FindFirstObjectByType<GameController>();
            if (gameController != null)
            {
                gameController.SetGameFailed();
            }
        }

        OnHPChanged?.Invoke(hp);

        // ダメージを受けたら無敵状態に
        _isInvincible = true;
        _invincibilityTimer = config.invincibilityDuration;

        return true;
    }

    // 外部からHP情報を取得するメソッド
    public int GetCurrentHP() => hp;
    public int GetMaxHP() => config.maxHP;
}
