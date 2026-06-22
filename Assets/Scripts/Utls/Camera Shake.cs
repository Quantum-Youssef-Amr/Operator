using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float ShakeIntensely, NumberOfShakes;
    [SerializeField] private bool allowEffectCompounding;

    private Transform _camera;
    private Vector2 _R2Loc, _camera_pos;
    private Coroutine _cameraShake;
    void Start()
    {
        _camera = Camera.main.transform;
        GameEventManager.Instance.onCameraShake += ShakeCamera;
    }

    private void ShakeCamera()
    {
        // allow effect build up
        if (allowEffectCompounding)
        {
            StartCoroutine(Camerashake());
            return;
        }

        _cameraShake ??= StartCoroutine(Camerashake());
    }


    private IEnumerator Camerashake()
    {
        for (int i = 0; i < NumberOfShakes; i++)
        {
            _camera_pos = _camera.position;
            _R2Loc = Random.insideUnitCircle;
            _R2Loc.Normalize();

            _camera.position = new Vector3(_camera_pos.x + _R2Loc.x * ShakeIntensely, _camera_pos.y + _R2Loc.y * ShakeIntensely, -10f);
            yield return new WaitForEndOfFrame();
            _camera.position = transform.position + new Vector3(0, 0, -10f);
        }
        _camera.position = transform.position + new Vector3(0, 0, -10f);
        _cameraShake = null;
    }
}
