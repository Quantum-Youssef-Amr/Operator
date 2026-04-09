using UnityEngine;

[CreateAssetMenu(fileName = "PlaneType", menuName = "Planes/PlaneType")]
public class PlaneType : ScriptableObject
{
    public string PlaneName;
    public Sprite PlaneSprite;
    public PlaneTypeEnum planeType;
    public float PlaneMass, PlaneMovementSpeed, PlaneTurningDistance = 0.01f, PlaneRotatingSpeed, PlaneHealth;
    public bool PlaneCanGiveDamage;
    public float PlaneDamage;
}


public enum PlaneTypeEnum
{
    military, civilian, cargo
}