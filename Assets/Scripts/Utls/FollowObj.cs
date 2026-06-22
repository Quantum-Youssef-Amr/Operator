using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private bool LerpPositions;
    [SerializeField] private float LerpSpeed;


    private Transform _t;
    void Start()
    {
        _t = transform;
    }

    void Update()
    {
        _t.position = Vector2.Lerp(_t.position, Target.position, LerpPositions ? LerpSpeed * Time.deltaTime : 1f);
    }
}
