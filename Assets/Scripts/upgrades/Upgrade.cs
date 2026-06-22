using UnityEngine;

[CreateAssetMenu(fileName = "new upgrade", menuName = "upgrades")]
public class Upgrade : ScriptableObject
{
    public string UpgradeName;
    [TextArea] public string upgradeDescription;
    public Upgrades upgrade;
    public float upgradeValue;
    public bool IsUpgradeRepeated;
    public int upgradeLevel;
    public float upgradeCostAdder;
}