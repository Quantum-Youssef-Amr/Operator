using UnityEngine;

public class PlaneAppearanceSetter : MonoBehaviour
{
    [SerializeField] private Sprite DefaultSprite;
    private PlaneType _planeType;
    private SpriteRenderer _sr;
    void Awake()
    {
        _planeType = transform.parent.GetComponent<PlaneInfoHolder>().planeType;
        _sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlaneInfo).is_bought)
            GameEventManager.Instance.OnPlaneDisplayFullInfo?.Invoke(gameObject);

        if (upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlanesShapes).is_bought)
        {
            _sr.sprite = _planeType.PlaneSprite;
        }
        else
            _sr.sprite = DefaultSprite;


        if (upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlaneDirectionAndSpeed).is_bought)
        {
            _planeType.RotatesWhenMoving = true;
        }
    }
}
