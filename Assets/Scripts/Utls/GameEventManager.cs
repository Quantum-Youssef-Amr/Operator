using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    public Action OnEnterWayPointEditMode;
    public Action<PlaneWayPointsEditEventArgs> OnPlaneChangeWayPoints;
    public Action OnExitWayPointEditMode;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }


}

public struct PlaneWayPointsEditEventArgs
{
    public Vector3[] Waypoints;
}