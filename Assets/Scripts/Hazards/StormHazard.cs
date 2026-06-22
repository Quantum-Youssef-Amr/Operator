using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormHazard : Hazard
{
    [SerializeField] private float TeleportChance, TeleportDuration;
    protected override void SearchAndApply(List<Collider2D> overlappedBodies)
    {
        foreach (var body in overlappedBodies)
        {
            planeType m_planeType = body.GetComponent<PlaneInfoHolder>().GetPlaneType();
            switch (m_planeType.PlaneType)
            {
                default:
                case PlaneTypeEnum.privatePlanes:
                    body.GetComponent<PlaneHealth>().DeathImmediate();
                    break;

                case PlaneTypeEnum.cargo:
                case PlaneTypeEnum.civilian:
                    body.GetComponent<PlaneHealth>().TakeDamage(HazardDamage);
                    if (UnityEngine.Random.value < TeleportChance)
                        StartCoroutine(ReturnAirCraft(body.gameObject, m_planeType, GetRandomDirection(), TeleportDuration));
                    break;

                case PlaneTypeEnum.military:
                    break;
            }
        }
    }

    private IEnumerator ReturnAirCraft(GameObject AirCraft, planeType planeType, Vector2 SpawnDirection, float Duration)
    {
        Destroy(AirCraft);
        yield return new WaitForSeconds(Duration);
        Instantiate(AirCraft, (Vector2)transform.position + SpawnDirection, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, SpawnDirection)));

        AirCraft.GetComponent<PlaneInfoHolder>().SetPlaneType(planeType);
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 m_d = UnityEngine.Random.insideUnitCircle;
        m_d.Normalize();
        return 0.9f * HazardScale * m_d;
    }


    protected override IEnumerator DestroyAfter(float Duration)
    {
        return base.DestroyAfter(Duration);
    }
}
