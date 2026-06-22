using System;
using UnityEngine;

public class PlayHealth : MonoBehaviour
{
    [SerializeField] private int Health = 5;
    [SerializeField] private GameObject HealthEntity;

    private int _currentHealth;
    private Transform _t;

    void Start()
    {
        _currentHealth = Health;
        _t = transform;
        SetUpHealthMeter(Health);
        GameEventManager.Instance.OnPlaneDetached += IsPlaneCrushed;
    }

    private void IsPlaneCrushed(PlaneDetectionEventArgs args)
    {
        if (args.isCrush && args.ExitState == PlaneDetectionEventArgs.PlaneExitState.Crushed)
        {
            _currentHealth--;

            if (_currentHealth <= 0)
            {
                GameEventManager.Instance.OnHealthEqZero?.Invoke();
            }
            UpdateHealthDisplay(_currentHealth);
        }
    }

    private void UpdateHealthDisplay(int currentHealth)
    {
        for (int m_healthPoint = Health; m_healthPoint > 0; m_healthPoint--)
        {
            if (currentHealth == m_healthPoint)
                return;
            Destroy(_t.GetChild(m_healthPoint).gameObject);
        }
    }

    private void SetUpHealthMeter(int health)
    {
        for (int m_HealthPoint = 0; m_HealthPoint < health; m_HealthPoint++)
        {
            Instantiate(HealthEntity, Vector2.zero, Quaternion.identity, _t);
        }
    }
}
