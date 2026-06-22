using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public float Score { get; private set; }

    [SerializeField] private float CorrectExitMultiplier = 1, InCorrectExitMultiplier = 0.3f, CrushMultiplier = -0.5f;

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    void Start()
    {
        GameEventManager.Instance.OnPlaneDetached += CalculateScore;
    }

    private void CalculateScore(PlaneDetectionEventArgs args)
    {
        if (args.PlaneState == PlaneDetectionEventArgs.PlaneEnterState.Enter || args.planeType == PlaneDetectionEventArgs.PlaneType.Enemy) return;

        planeType m_DetectedPlaneType = new(args.DetectedPlane.GetComponent<PlaneInfoHolder>().planeType);

        switch (args.ExitState)
        {
            default:
            case PlaneDetectionEventArgs.PlaneExitState.None:
                break;
            case PlaneDetectionEventArgs.PlaneExitState.CorrectExit:
                AddScore((float)m_DetectedPlaneType.PlaneType
                         * ((float)m_DetectedPlaneType.planeSize / 10)
                         * CorrectExitMultiplier);
                break;
            case PlaneDetectionEventArgs.PlaneExitState.InCorrectExit:
                AddScore((float)m_DetectedPlaneType.PlaneType
                         * ((float)m_DetectedPlaneType.planeSize / 10)
                         * InCorrectExitMultiplier);
                break;
            case PlaneDetectionEventArgs.PlaneExitState.Crushed:
                AddScore((float)m_DetectedPlaneType.PlaneType
                         * ((float)m_DetectedPlaneType.planeSize / 10)
                         * CrushMultiplier);
                break;
        }
        GameEventManager.Instance.OnScoreChanged?.Invoke(Score);
    }

    public void AddScore(float score)
    {
        Score += score;
    }
}
