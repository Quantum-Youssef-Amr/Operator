using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class upgradesManager : MonoBehaviour
{
    public static upgradesManager Instance { get; private set; }

    public HashSet<Upgrade> boughtUpgrades = new();
    [SerializeField] private Upgrade[] AllUpgrades;
    void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    public bool buyUpgrade(Upgrade upgrade)
    {
        if (ScoreManager.Instance.Score >= (upgrade.upgradeValue + (upgrade.upgradeLevel * upgrade.upgradeCostAdder)))
        {
            boughtUpgrades.Add(upgrade);
            return true;
        }
        return false;
    }

    public void SellUpgrade(Upgrade upgrade)
    {
        if (boughtUpgrades.Contains(upgrade))
        {
            boughtUpgrades.Remove(upgrade);
            ScoreManager.Instance.AddScore(upgrade.upgradeValue + (upgrade.upgradeLevel * upgrade.upgradeCostAdder));
        }
    }

    public (bool is_bought, int level) IsUpgradeBought(Upgrades upgrade)
    {
        var m_upgradeFounder = boughtUpgrades.Where(UG => UG.upgrade == upgrade);
        return (m_upgradeFounder.Any(), m_upgradeFounder.Any() ? m_upgradeFounder.Count() : 0);
    }
}

/// <summary>
/// encode upgrade value
/// </summary>
public enum Upgrades
{
    RenderPlanesShapes,
    RenderPlaneInfo,
    RenderPlaneDirectionAndSpeed,
    RadarScanSpeed,
    RadarRangeIncrease,
}
