using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

// プレイヤークラス
public class PlayerController : MonoBehaviour
{
    [SerializeField] Gun _gun; // 弾の発射するGunコンポーネント
    [SerializeField] private PlayerConfig config;
    [SerializeField] private Image ghostSpriteRenderer; // Ghost状態時のイメージ
    private int hp;

    // 無敵状態関連
    private bool _isInvincible = false;  // 無敵状態フラグ
    private float _invincibilityTimer = 0f;  // 無敵時間カウンター
    private SpriteRenderer _spriteRenderer;  // 点滅用

    // Ghost状態関連
    private Ghost _ghost;
    private bool _isGhost = false;  // Ghost状態フラグ
    private float _spaceKeyHoldTime = 0f;
    private bool _waitingForKeyRelease = false;  // Ghost解除後のキーリリース待ち

    // Ghost状態を外部から参照するプロパティ
    public bool IsGhost => _isGhost;

    // HP変更時のイベント
    public event Action<int> OnHPChanged;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _ghost = GetComponent<Ghost>();
        hp = config.maxHP;
        OnHPChanged?.Invoke(hp);

        // Ghostスプライトを初期状態で非表示に
        ghostSpriteRenderer.enabled = false;
        _ghost.OnGhostModeChanged += OnGhostModeChanged;
        
    }

    private void Update()
    {
        HandleInput();
        HandleInvincibility();
    }

    private void HandleInput()
    {

        // Spaceキーが押されている場合
        if (Keyboard.current.spaceKey.isPressed)
        {
            _spaceKeyHoldTime += Time.deltaTime;

            float activation = _ghost.ActivationTime;
            // 長押しでGhost状態に移行（キーリリース待ち中でなければ）
            if (_spaceKeyHoldTime >= activation && !_isGhost && !_waitingForKeyRelease)
            {
                _ghost.StartGhostMode();
            }
        }
        else
        {
            // Spaceキーが離された
            float activation = _ghost.ActivationTime;
            if (_spaceKeyHoldTime > 0f && _spaceKeyHoldTime < activation)
            {
                // 短押しの場合は弾を発射
                _gun.Fire();
            }

            // Ghost状態を解除
            if (_isGhost)
            {
                _ghost.StopGhostMode();
            }

            // キーを離したのでフラグをリセット
            _spaceKeyHoldTime = 0f;
            _waitingForKeyRelease = false;
        }
    }

    // Ghost状態変化時のコールバック
    private void OnGhostModeChanged(bool isGhostMode)
    {
        // Ghost状態になった時
        if (isGhostMode)
        {
            // Ghost状態開始
            _isGhost = true;
            ghostSpriteRenderer.enabled = true;
            _spriteRenderer.enabled = true;  // 通常スプライトも表示
        }
        // Ghost状態が解除された時
        else
        {
            // Ghost状態解除
            _isGhost = false;
            ghostSpriteRenderer.enabled = false;
            
            // 解除時にSpaceキーがまだ押されていれば、キーリリース待ちフラグを立てる
            if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
            {
                _waitingForKeyRelease = true;
            }
        }
    }

    // 外部からGhost状態を解除する
    public void ForceExitGhostMode()
    {
        if (_isGhost)
        {
            _ghost.StopGhostMode();
        }
    }

    private void HandleInvincibility()
    {
        // ダメージ無敵時間の更新
        if (_invincibilityTimer > 0f)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
            {
                _isInvincible = false;  // 無敵解除
                _spriteRenderer.enabled = true;  // 点滅表示終了
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
        // 無敵状態またはGhost状態ではダメージを受けない
        if (_isInvincible || _isGhost)
        {
            return false;
        }

        hp -= damage;
        AudioManager.Instance.PlaySE("PlayerDamaged");

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
