using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlaneMovementManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PlaneType planeType;
    [SerializeField] private float UpdateLineTimer = 0.2f;
    [SerializeField] private float PointDeleteDistance = 0.2f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Transform _t;
    private List<Vector3> _movementWaypoints = new();
    private bool _isInEditPathMode;

    #region inputs
    private InputSystem _inputs;
    void Awake()
    {
        _inputs = new();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _t = transform;
    }

    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
        _spriteRenderer.sprite = planeType.PlaneSprite;
        StartCoroutine(UpdatePathLineRendererTimer());

        _inputs.Player.LeftMouseClick.performed += _ =>
        {
            if (!_isInEditPathMode)
            {
                HidePathLineRenderer();
                return;
            }
            AddPointToMovementWaypoints();
        };

        _inputs.Player.RightMouseClick.performed += _ =>
        {
            _isInEditPathMode = false;
            HidePathLineRenderer();
        };
    }

    void Update()
    {
        if (_movementWaypoints.Count == 0)
        {
            MoveInLine();
        }
        else
            MoveOnPath();
    }

    private void MoveOnPath()
    {
        MoveToPoint(_movementWaypoints[0]);
        if (Vector2.Distance(_movementWaypoints[0], _t.position) <= planeType.PlaneTurningDistance)
        {
            _movementWaypoints.RemoveAt(0);
            UpdatePathLineRenderer();
        }
    }

    private void MoveToPoint(Vector2 point)
    {
        _rb.AddForce(planeType.PlaneMovementSpeed * Time.deltaTime * _t.up, ForceMode2D.Force);
        RotateToPointHeading(point);
    }

    private void RotateToPointHeading(Vector2 point)
    {
        _t.rotation = Quaternion.RotateTowards(_t.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, (point - (Vector2)_t.position).normalized)), planeType.PlaneRotatingSpeed * Time.deltaTime);
    }

    private void MoveInLine()
    {
        _rb.AddForce(planeType.PlaneMovementSpeed * Time.deltaTime * _t.up, ForceMode2D.Force);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isInEditPathMode = !_isInEditPathMode;

        if (_isInEditPathMode)
            ShowPathLineRenderer();
        else
            HidePathLineRenderer();

        UpdatePathLineRenderer();
    }

    private void AddPointToMovementWaypoints()
    {
        Vector2 m_clickWorldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        if (RemovePointIfPossible(m_clickWorldPoint))
            return;

        _movementWaypoints.Add(m_clickWorldPoint);
    }

    private bool RemovePointIfPossible(Vector2 m_clickWorldPoint)
    {

        for (int pointIdx = 0; pointIdx < _movementWaypoints.Count; pointIdx++)
        {
            if (Vector2.Distance(_movementWaypoints[pointIdx], m_clickWorldPoint) <= PointDeleteDistance)
            {
                _movementWaypoints.Remove(_movementWaypoints[pointIdx]);
                return true;
            }
        }
        return false;
    }

    private void UpdatePathLineRenderer()
    {
        Vector3[] m_waypointsCopy = new Vector3[_movementWaypoints.Count + 2];
        m_waypointsCopy[0] = _t.position;

        for (int i = 0; i < _movementWaypoints.Count; i++)
            m_waypointsCopy[i + 1] = _movementWaypoints[i];

        if (m_waypointsCopy.Length == 2)
            m_waypointsCopy[^1] = _t.up * 1000;
        else
            m_waypointsCopy[^1] = (m_waypointsCopy[^2] - m_waypointsCopy[^3]).normalized * 1000;


        GameEventManager.Instance.OnPlaneChangeWayPoints?.Invoke(new()
        {
            Waypoints = m_waypointsCopy
        });
    }

    private void HidePathLineRenderer()
    {
        GameEventManager.Instance.OnExitWayPointEditMode?.Invoke();
    }

    private void ShowPathLineRenderer()
    {
        GameEventManager.Instance.OnEnterWayPointEditMode?.Invoke();
    }

    private IEnumerator UpdatePathLineRendererTimer()
    {
        yield return new WaitForSeconds(UpdateLineTimer);

        if (_movementWaypoints.Count > 0)
            UpdatePathLineRenderer();

        StartCoroutine(UpdatePathLineRendererTimer());
    }
}
