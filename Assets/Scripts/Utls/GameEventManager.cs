using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    #region effects events
    public Action onCameraShake;
    #endregion

    #region plane events
    public Action<GameObject> OnPlaneDisplayFullInfo;
    public Action<GameObject> OnPlaneSelect;
    public Action OnPlaneDeselecting;
    #endregion

    #region Path events
    public Action OnEnterWayPointEditMode;
    public Action<PlaneWayPointsEditEventArgs> OnPlaneChangeWayPoints;
    public Action OnExitWayPointEditMode;

    #endregion

    #region Spawner events
    public Action<PlaneDetectionEventArgs> OnPlaneDetached;
    #endregion

    #region Shader Events
    public Action<ShaderEventArgs> OnShowExitAngle;
    public Action OnHideExitAngle;
    #endregion

    #region UI events
    public Action<float> OnScoreChanged;
    public Action<planeType> OnShowAirCraftDetails;
    public Action OnHideAirCraftDetails;
    public Action OnMouseIndicatorSelectPlane;
    #endregion

    #region HealthEvents
    public Action OnHealthEqZero;
    #endregion

    #region spawner events
    public Action OnDayEnd;
    public Action OnDayStart;
    #endregion

    #region Daily rewards events
    public Action OnShowDailyRewards;
    public Action OnSelectDailyReward;
    public Action OnHideDailyRewards;
    #endregion

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

public struct PlaneDetectionEventArgs
{
    public enum PlaneEnterState { None, Enter, Exit }
    public enum PlaneExitState { None, CorrectExit, InCorrectExit, Crushed }
    public enum CrushReason { None, Hazard, enemy }
    public enum PlaneType { None, Friendly, Enemy }
    public GameObject DetectedPlane;
    public PlaneEnterState PlaneState;
    public PlaneType planeType;
    public PlaneExitState ExitState;
    public bool isCrush;
    public CrushReason crushReason;
}

public struct ShaderEventArgs
{
    /// <summary>
    /// exit range in degrees
    ///:: x -> min ::
    ///:: y -> max ::
    /// </summary>
    public Vector2 ExitRange;
}