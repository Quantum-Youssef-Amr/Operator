using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlaneSelectIndicator : MonoBehaviour
{
    [SerializeField] private float PlaneDetectionRadius = 1f;
    private GameObject _nearByPlane;
    private GameObject _selectedPlane;
    private Transform _t;
    private Camera _mainCamera;
    private Image _indicatorImage;
    private InputSystem _inputs;

    void Awake()
    {
        _inputs = new();
    }

    void OnEnable()
    {
        _inputs.Enable();
    }

    void OnDisable()
    {
        _inputs.Disable();
    }

    void Start()
    {
        _t = transform;
        _mainCamera = Camera.main;
        _indicatorImage = GetComponent<Image>();
        GameEventManager.Instance.OnPlaneSelect += (plane) => _selectedPlane = plane;
        GameEventManager.Instance.OnPlaneDeselecting += () => _selectedPlane = null;
    }

    void Update()
    {
        Vector2 m_mouseWorldPos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.value);
        _nearByPlane = Physics2D.CircleCast(
                m_mouseWorldPos,
                PlaneDetectionRadius,
                Vector2.zero,
                0,
                LayerMask.GetMask("Planes")
            ).collider?.gameObject;

        if (_nearByPlane != null)
        {
            _t.position = _nearByPlane.transform.position;
            _indicatorImage.enabled = true;
            if (_inputs.Player.LeftMouseClick.WasPressedThisFrame())
                _nearByPlane.GetComponent<PlaneMovementManager>().SelectPlane();
            return;
        }

        if (_selectedPlane == null)
        {
            _indicatorImage.enabled = false;
            return;
        }

        _t.position = _selectedPlane.transform.position;
    }
}
