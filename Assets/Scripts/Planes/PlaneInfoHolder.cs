using UnityEngine;

public class PlaneInfoHolder : MonoBehaviour
{
    public PlaneType planeType;
    private planeType _planeType;

    void Awake()
    {
        _planeType = new(planeType)
        {
            PlaneModelID = GetRandomModelID()
        };
    }

    private string GetRandomModelID()
    {
        string m_id = "";
        for (int i = 0; i < 4;)
        {
            char m_charCode = (char)Random.Range(0, 100);
            if (char.IsLetterOrDigit(m_charCode))
            {
                m_id += m_charCode;
                i++;
            }
        }
        return m_id;
    }

    public planeType GetPlaneType()
    {
        return _planeType;
    }

    public void SetPlaneType(planeType planeType)
    {
        _planeType = planeType;
    }

}
