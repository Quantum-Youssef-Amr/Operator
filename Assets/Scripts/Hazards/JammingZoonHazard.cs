
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JammingZoonHazard : Hazard
{
    [SerializeField] private float TeleportChance = 0.3f;
    [SerializeField] private float HideDuration = 2f;

    protected override void SearchAndApply(List<Collider2D> overlappedBodies)
    {
        foreach (var body in overlappedBodies)
        {
            if (Random.value > TeleportChance)
                continue;

            GameObject m_planeHolder = body.gameObject;
            planeType m_planeTypeHolder = body.GetComponent<PlaneInfoHolder>().GetPlaneType();
            Vector2 m_randomPlaneTeleportDirection = GetRandomDirection();

            Destroy(body.gameObject);
            StartCoroutine(ReturnAirCraft(m_planeHolder, m_planeTypeHolder, m_randomPlaneTeleportDirection, HideDuration));
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 m_d = Random.insideUnitCircle;
        m_d.Normalize();
        return 0.9f * HazardScale * m_d;
    }

    private IEnumerator ReturnAirCraft(GameObject AirCraft, planeType planeType, Vector2 SpawnDirection, float Duration)
    {
        yield return new WaitForSeconds(Duration);
        Instantiate(AirCraft, (Vector2)transform.position + SpawnDirection, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, SpawnDirection)));

        AirCraft.GetComponent<PlaneInfoHolder>().SetPlaneType(planeType);
        AirCraft.GetComponent<PlaneHealth>().TakeDamage(HazardDamage);
    }

    protected override IEnumerator DestroyAfter(float Duration)
    {
        return base.DestroyAfter(Duration);
    }
}
