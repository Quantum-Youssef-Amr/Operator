using UnityEngine;

public class PlaneHealth : MonoBehaviour
{
    private planeType _planeType;

    void Start()
    {
        _planeType = GetComponent<PlaneInfoHolder>().GetPlaneType();
    }


    public void TakeDamage(float damage)
    {
        _planeType.PlaneHealth -= damage;
        if (_planeType.PlaneHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        // add exploding particle system
        GameEventManager.Instance.OnPlaneDetached?.Invoke(new()
        {
            DetectedPlane = gameObject,
            PlaneState = PlaneDetectionEventArgs.PlaneEnterState.None,
            planeType = PlaneDetectionEventArgs.PlaneType.Friendly,
            ExitState = PlaneDetectionEventArgs.PlaneExitState.Crushed,
            isCrush = true,
            crushReason = PlaneDetectionEventArgs.CrushReason.Hazard
        });

        Destroy(gameObject);
    }

    public void DeathImmediate() => Death();
}
