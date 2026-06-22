using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlaneInfoHolder))]
public class PlaneMovementManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private float UpdateLineTimer = 0.2f;
    [SerializeField] private float PointDeleteDistance = 0.2f;
    public Vector2 ExitAngle = new Vector2(0, 360);

    private planeType planeType;
    private Rigidbody2D _rb;
    private Transform _t, _appearanceChild;
    private List<Vector3> _movementWaypoints = new();
    private Vector2 _StartPoint = -Vector2.one;
    private bool _isInEditPathMode;

    #region inputs
    private InputSystem _inputs;
    void Awake()
    {
        _inputs = new();
        _rb = GetComponent<Rigidbody2D>();
        planeType = GetComponent<PlaneInfoHolder>().GetPlaneType();
        _t = transform;
        _appearanceChild = _t.GetChild(0).transform;
    }

    void OnEnable() => _inputs.Enable();
    void OnDisable() => _inputs.Disable();
    #endregion

    void Start()
    {
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
            SetSelectionState();
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
        if (planeType.RotatesWhenMoving)
            RotateToPointHeading(point, _appearanceChild);
        else
            _appearanceChild.rotation = Quaternion.identity;

        _rb.AddForce(planeType.PlaneMovementSpeed * Time.deltaTime * _t.up, ForceMode2D.Force);
        RotateToPointHeading(point, _t);
    }

    private void RotateToPointHeading(Vector2 point, Transform obj_transform)
    {
        obj_transform.rotation = Quaternion.RotateTowards(_t.rotation, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, (point - (Vector2)_t.position).normalized)), planeType.PlaneRotatingSpeed * Time.deltaTime);
    }

    private void MoveInLine()
    {
        if (_movementWaypoints.Count == 0 && _StartPoint != -Vector2.one)
        {
            _rb.AddForce(planeType.PlaneMovementSpeed * Time.deltaTime * (_StartPoint - (Vector2)_t.position).normalized, ForceMode2D.Force);
            return;
        }
        _rb.AddForce(planeType.PlaneMovementSpeed * Time.deltaTime * _t.up, ForceMode2D.Force);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isInEditPathMode = !_isInEditPathMode;
        SetSelectionState();
        UpdatePathLineRenderer();
    }

    public void SelectPlane()
    {
        _isInEditPathMode = true;
        SetSelectionState();
        UpdatePathLineRenderer();
    }

    private void SetSelectionState()
    {
        if (_isInEditPathMode)
        {
            ShowPathLineRenderer();
            ShowExitAngle();
            GameEventManager.Instance.OnPlaneSelect?.Invoke(gameObject);
            GameEventManager.Instance.OnShowAirCraftDetails?.Invoke(planeType);
        }
        else
        {
            HidePathLineRenderer();
            HideExitAngle();
            GameEventManager.Instance.OnPlaneDeselecting?.Invoke();
            GameEventManager.Instance.OnHideAirCraftDetails?.Invoke();
        }
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
            m_waypointsCopy[^1] = _t.up * 100;
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

    private void HideExitAngle()
    {
        GameEventManager.Instance.OnHideExitAngle?.Invoke();
    }

    private void ShowExitAngle()
    {
        GameEventManager.Instance.OnShowExitAngle?.Invoke(new() { ExitRange = ExitAngle });
    }

    public void AddStartPoint(Vector2 point)
    {
        _StartPoint = point;
    }
}
