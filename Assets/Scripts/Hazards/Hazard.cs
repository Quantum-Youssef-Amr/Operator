using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hazard : MonoBehaviour
{
    [SerializeField] protected float HazardDuration;
    [SerializeField] protected Vector2 HazardScale;
    [SerializeField] protected float HazardDamage;

    protected CircleCollider2D _area;
    protected Rigidbody2D _rb;

    protected List<Collider2D> _overlappedBodies;

    void Start()
    {
        _area = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();

        transform.localScale = Vector3.one * UnityEngine.Random.Range(HazardScale.x, HazardScale.y);
        StartCoroutine(DestroyAfter(HazardDuration));
    }

    void Update()
    {
        _area.Overlap(_overlappedBodies);
        SearchAndApply(_overlappedBodies);
    }

    protected virtual void SearchAndApply(List<Collider2D> overlappedBodies)
    {
        throw new NotImplementedException();
    }

    protected virtual IEnumerator DestroyAfter(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
}
