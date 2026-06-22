using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject PlanePrefab;
    [SerializeField] private PlaneType[] planes;
    [SerializeField] private GameObject[] Hazards;
    [SerializeField] private GameObject EnemyPrefab;

    [Header("Spawner Settings")]
    [SerializeField] private float SpawnRate;
    [SerializeField] private Vector2 RandomAngleSpawn = new(-45f, 45f);
    [SerializeField] private float ChangeRate;
    [SerializeField] private AnimationCurve PlanesSpawnCurve, hazardsSpawnCurve, EnemiesSpawnCurve;

    private int _dayCounter, _spawnedPlanes_exit, _spawnedHazards_exit, _spawnedEnemies_exit;
    private Camera _mainCamera;
    private Transform _t;

    void Awake()
    {
        _mainCamera = Camera.main;
        _t = transform;
    }
    void Start()
    {
        GameEventManager.Instance.OnHideDailyRewards += StartDay;
        GameEventManager.Instance.OnPlaneDetached += MarkPlaneExited;

        StartDay();
    }

    private void MarkPlaneExited(PlaneDetectionEventArgs args)
    {
        switch (args.planeType)
        {
            default:
            case PlaneDetectionEventArgs.PlaneType.None:
                break;

            case PlaneDetectionEventArgs.PlaneType.Friendly:
                if (args.ExitState != PlaneDetectionEventArgs.PlaneExitState.None)
                    _spawnedPlanes_exit++;
                break;
            case PlaneDetectionEventArgs.PlaneType.Enemy:
                if (args.ExitState != PlaneDetectionEventArgs.PlaneExitState.None)
                    _spawnedEnemies_exit++;
                break;
        }
    }

    private void StartDay()
    {
        GameEventManager.Instance.OnDayStart?.Invoke();
        _dayCounter++;
        SpawnRate -= ChangeRate;

        StartCoroutine(SpawnDay());
    }

    private IEnumerator SpawnDay()
    {
        int m_planesToSpawn = Mathf.RoundToInt(PlanesSpawnCurve.Evaluate(_dayCounter)),
            m_hazardsToSpawn = Mathf.RoundToInt(hazardsSpawnCurve.Evaluate(_dayCounter)),
            m_enemiesToSpawn = Mathf.RoundToInt(EnemiesSpawnCurve.Evaluate(_dayCounter));

        int m_spawnedPlanes = 0, m_spawnedHazards = 0, m_spawnedEnemies = 0;

        for (int i = 0; i < m_planesToSpawn + m_hazardsToSpawn + m_enemiesToSpawn;)
        {
            switch (Random.Range(0, 2))
            {
                default:
                case 0:         // spawn planes
                    if (m_spawnedPlanes >= m_planesToSpawn)
                        continue;

                    SpawnPlane();
                    m_spawnedPlanes++;

                    break;
                case 1:         // spawn hazards
                    if (m_spawnedHazards >= m_hazardsToSpawn)
                        continue;

                    SpawnHazard();
                    m_spawnedHazards++;

                    break;
                case 2:         // spawn enemies
                    if (m_spawnedEnemies >= m_enemiesToSpawn)
                        continue;

                    SpawnEnemy();
                    m_spawnedEnemies++;

                    break;
            }
            i++;
            yield return new WaitForSeconds(1f / SpawnRate);
        }

        yield return new WaitUntil(
            () =>
            {
                return _spawnedPlanes_exit == m_planesToSpawn
                && _spawnedHazards_exit == m_hazardsToSpawn
                && _spawnedEnemies_exit == m_enemiesToSpawn;
            }
        );

        EndDay();
    }

    private void SpawnPlane()
    {
        Vector2 m_spawnLocation = GetRandomRadialPosition(mag: _mainCamera.orthographicSize * 2 + 1);
        GameObject m_spawnedPlane = Instantiate(PlanePrefab, m_spawnLocation, Quaternion.identity, _t);
        m_spawnedPlane.GetComponent<PlaneInfoHolder>().planeType = planes[Random.Range(0, planes.Length)];
        m_spawnedPlane.GetComponent<PlaneMovementManager>().AddStartPoint(Quaternion.Euler(0, 0, Random.Range(RandomAngleSpawn.x, RandomAngleSpawn.y)) * m_spawnLocation);
    }

    private Vector2 GetRandomRadialPosition(float mag)
    {
        Vector2 m_v = Random.insideUnitCircle;
        m_v.Normalize();
        return m_v * mag;
    }

    private void SpawnHazard()
    {
        if (Hazards.Length == 0) return;
        Vector2 m_spawnLocation = GetRandomPositionOnScreen();
        GameObject m_spawnedHazard = Instantiate(Hazards[Random.Range(0, Hazards.Length)], m_spawnLocation, Quaternion.identity, _t);
    }

    private Vector2 GetRandomPositionOnScreen()
    {
        return _mainCamera.ViewportToWorldPoint(new(Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }

    private void SpawnEnemy()
    {
        if (EnemyPrefab == null) return;
        Vector2 m_spawnLocation = GetRandomRadialPosition(mag: _mainCamera.orthographicSize * 2 + 1);
        GameObject m_spawnedPlane = Instantiate(EnemyPrefab, m_spawnLocation, Quaternion.identity, _t);
    }

    private void EndDay()
    {
        GameEventManager.Instance.OnDayEnd?.Invoke();
        GameEventManager.Instance.OnShowDailyRewards?.Invoke();
    }
}
