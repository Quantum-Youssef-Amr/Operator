using UnityEngine;

[CreateAssetMenu(fileName = "PlaneType", menuName = "Planes/PlaneType")]
public class PlaneType : ScriptableObject
{
    public string PlaneName;
    public string PlaneModelID;
    public Sprite PlaneSprite;
    public PlaneTypeEnum planeType;
    public PlaneSize planeSize;
    public float PlaneMass, PlaneMovementSpeed, PlaneTurningDistance = 0.01f, PlaneRotatingSpeed, PlaneHealth;
    public bool RotatesWhenMoving, PlaneCanGiveDamage;
    public float PlaneDamage;
    public LayerMask PlaneDamageLayerMask;
}

/// <summary>
/// encode plane type value
/// </summary>
public enum PlaneTypeEnum
{
    military = 80,
    civilian = 150,
    privatePlanes = 180,
    cargo = 120
}

/// <summary>
/// encoding the size multiplier, divide by 10 to get the x
/// </summary>
public enum PlaneSize
{
    Small = 10, Medium = 15, large = 20
}

// model for data exchange
public class planeType
{
    public string PlaneName;
    public string PlaneModelID;
    public Sprite PlaneSprite;
    public PlaneTypeEnum PlaneType;
    public PlaneSize planeSize;
    public float PlaneMass, PlaneMovementSpeed, PlaneTurningDistance = 0.01f, PlaneRotatingSpeed, PlaneHealth;
    public bool RotatesWhenMoving, PlaneCanGiveDamage;
    public float PlaneDamage;
    public LayerMask PlaneDamageLayerMask;

    public planeType(PlaneType planeType)
    {
        PlaneName = planeType.PlaneName;
        PlaneModelID = planeType.PlaneModelID;
        PlaneSprite = planeType.PlaneSprite;
        PlaneType = planeType.planeType;
        PlaneMass = planeType.PlaneMass;
        PlaneMovementSpeed = planeType.PlaneMovementSpeed;
        PlaneTurningDistance = planeType.PlaneTurningDistance;
        PlaneRotatingSpeed = planeType.PlaneRotatingSpeed;
        PlaneHealth = planeType.PlaneHealth;
        RotatesWhenMoving = planeType.RotatesWhenMoving;
        PlaneCanGiveDamage = planeType.PlaneCanGiveDamage;
        PlaneDamage = planeType.PlaneDamage;
        PlaneDamageLayerMask = planeType.PlaneDamageLayerMask;
    }
}