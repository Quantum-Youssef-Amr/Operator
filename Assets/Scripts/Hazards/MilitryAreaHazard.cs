using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiliaryAreaHazard : Hazard
{
    protected override void SearchAndApply(List<Collider2D> overlappedBodies)
    {
        foreach (var body in overlappedBodies)
        {
            if (body.GetComponent<PlaneInfoHolder>().GetPlaneType().PlaneType == PlaneTypeEnum.military)
                continue;

            if (body.GetComponent<PlaneHealth>())
                body.GetComponent<PlaneHealth>().DeathImmediate();
        }
    }

    protected override IEnumerator DestroyAfter(float Duration)
    {
        return base.DestroyAfter(Duration);
    }
}
