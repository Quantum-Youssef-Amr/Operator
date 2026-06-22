using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _t;

    void Start()
    {
        Cursor.visible = false;
        _mainCamera = Camera.main;
        _t = transform;
    }

    void Update()
    {
        _t.position = (Vector2)_mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
    }
}
