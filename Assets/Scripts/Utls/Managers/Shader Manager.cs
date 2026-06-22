using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    [SerializeField] private Material RadarMaterial;

    void Start()
    {
        GameEventManager.Instance.OnShowExitAngle += showRangeInMaterial;
        GameEventManager.Instance.OnHideExitAngle += hideRangeInMaterial;
    }

    private void hideRangeInMaterial()
    {
        RadarMaterial.SetInt("_Render", 0);
    }

    private void showRangeInMaterial(ShaderEventArgs args)
    {
        RadarMaterial.SetInt("_Render", 1);
        RadarMaterial.SetVector("_Angle", args.ExitRange);
    }
}
