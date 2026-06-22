using System.Runtime.Serialization;
using UnityEditor.Analytics;
using UnityEngine;

public class PlanesDetector : MonoBehaviour
{
    void Start()
    {
        GetComponent<BoxCollider2D>().size = new(
            Camera.main.orthographicSize * 4f,
            Camera.main.orthographicSize * 1.8f
        );
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        GameEventManager.Instance.OnPlaneDetached?.Invoke(new()
        {
            DetectedPlane = collision.gameObject,
            isCrush = false,
            crushReason = PlaneDetectionEventArgs.CrushReason.None,
            PlaneState = PlaneDetectionEventArgs.PlaneEnterState.Enter,
            ExitState = PlaneDetectionEventArgs.PlaneExitState.None,
            planeType = collision.gameObject.layer == LayerMask.GetMask("enemies") ? PlaneDetectionEventArgs.PlaneType.Enemy : PlaneDetectionEventArgs.PlaneType.Friendly
        });
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        GameEventManager.Instance.OnPlaneDetached?.Invoke(new()
        {
            DetectedPlane = collision.gameObject,
            isCrush = false,
            crushReason = PlaneDetectionEventArgs.CrushReason.None,
            PlaneState = PlaneDetectionEventArgs.PlaneEnterState.Exit,
            ExitState = IsPlaneExitInExitRange(collision.gameObject) ? PlaneDetectionEventArgs.PlaneExitState.CorrectExit : PlaneDetectionEventArgs.PlaneExitState.InCorrectExit,
            planeType = collision.gameObject.layer == LayerMask.GetMask("enemies") ? PlaneDetectionEventArgs.PlaneType.Enemy : PlaneDetectionEventArgs.PlaneType.Friendly
        });

        Destroy(collision.gameObject);
    }

    private bool IsPlaneExitInExitRange(GameObject plane)
    {
        float m_planeAngle = Vector2.Angle(plane.transform.position, Vector2.down) + 90f;
        PlaneMovementManager m_planeMovement = plane.GetComponent<PlaneMovementManager>();
        return m_planeAngle < m_planeMovement.ExitAngle.y && m_planeAngle > m_planeMovement.ExitAngle.x;
    }

}
