using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Gun))]
public class PlayerController : MonoBehaviour
{
    private Gun _gun;

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
