using System;
using Unity.VisualScripting;
using UnityEngine;

public class PathLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer PathRenderer;
    void Start()
    {
        GameEventManager.Instance.OnPlaneChangeWayPoints += DrawPath;
        GameEventManager.Instance.OnExitWayPointEditMode += HidePath;
        GameEventManager.Instance.OnEnterWayPointEditMode += ShowPath;
    }

    private void ShowPath() => PathRenderer.enabled = true;

    private void HidePath()
    {
        PathRenderer.positionCount = 0;
        PathRenderer.enabled = false;
    }

    private void DrawPath(PlaneWayPointsEditEventArgs args)
    {
        PathRenderer.positionCount = args.Waypoints.Length;
        PathRenderer.SetPositions(args.Waypoints);
    }
}
