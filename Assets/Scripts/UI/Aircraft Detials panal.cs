using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AircraftDetailsPanel : MonoBehaviour
{
    [SerializeField] private GameObject DetailsPanel;
    [SerializeField] private Image PlaneImagePreview;
    [SerializeField] private Sprite DefaultPlaneSprite;
    [SerializeField] private TMPro.TextMeshProUGUI PlaneName, PlaneModel;
    [SerializeField] private TMPro.TextMeshProUGUI PlaneMass, PlaneType, PlaneSize, PlaneSpeed, PlaneHealth, PlaneValue;

    void Start()
    {
        GameEventManager.Instance.OnShowAirCraftDetails += SetUpPanel;
        GameEventManager.Instance.OnHideAirCraftDetails += HidePanel;
    }

    private void HidePanel()
    {
        DetailsPanel.SetActive(false);
    }

    private void SetUpPanel(planeType type)
    {
        PlaneImagePreview.sprite = upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlanesShapes).is_bought ? type.PlaneSprite : DefaultPlaneSprite;

        PlaneName.text = type.PlaneName;
        PlaneModel.text = type.PlaneModelID;

        PlaneMass.text = $"{type.PlaneMass} TN";
        PlaneType.text = upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlaneInfo).is_bought ? $"{type.PlaneType}" : $"---";
        PlaneSize.text = upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlaneInfo).is_bought ? $"{type.planeSize}" : $"---";
        PlaneSpeed.text = upgradesManager.Instance.IsUpgradeBought(Upgrades.RenderPlaneDirectionAndSpeed).is_bought ? $"{type.PlaneMovementSpeed} Km/h" : $"--- Km/h";
        PlaneHealth.text = $"{type.PlaneHealth} Hp";
        PlaneValue.text = $"{Mathf.RoundToInt((int)type.PlaneType * ((int)type.planeSize / 10f))}";

        DetailsPanel.SetActive(true);
    }
}
