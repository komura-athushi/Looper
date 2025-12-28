using UnityEngine;
using UnityEngine.InputSystem;


// プレイヤークラス
[RequireComponent(typeof(Gun))]
public class PlayerController : MonoBehaviour
{
    private Gun _gun; // 弾の発射するGunコンポーネント

    private void Awake()
    {
        _gun = GetComponent<Gun>();
    }

    private void Update()
    {
        // 短押しショット
        // New Input System
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _gun.Fire();
        }
    }
}
