using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    None,
    Cone,       // ��ä��
    Circle,     // ��
    Trapezoid,  // ��ٸ���
    Rectangle,  // �簢��
    HybridCone, // ��ä�� + ��ٸ���
}

public class RangePayload
{
    public RangeType Type { get; set; }
    public Material DetectionMaterial { get; set; }
    // ��ä��, ��
    public float Radius { get; set; }
    public float Angle { get; set; }
    // �簢��(Width - ���� ����, Height - ��)
    public float Height { get; set; }
    public float Width { get; set; }
    // ��ä��(UpperBase - ����, LowerBase - ��)
    public float UpperBase { get; set; }
    public float LowerBase { get; set; }
}

public class RangeManager : Singleton<RangeManager>
{
    [SerializeField] private List<GameObject> ranges;

    public GameObject CreateRange(Transform parent, RangePayload payload, bool isSetParent = true)
    {
        if (payload == null)
            return null;

        GameObject obj = new GameObject();
        BaseRange range = null;

        switch (payload.Type)
        {
            case RangeType.Cone:
                range = obj.AddComponent<ConeRange>();
                break;
            case RangeType.Circle:
                range = obj.AddComponent<CircleRange>();
                break;
            case RangeType.Trapezoid:
                range = obj.AddComponent<TrapezoidRange>();
                break;
            case RangeType.Rectangle:
                range = obj.AddComponent<RectangleRange>();
                break;
            case RangeType.HybridCone:
                range = obj.AddComponent<HybridConeRange>();
                break;
        }

        if (range != null)
        {
            range.Init(obj, parent, isSetParent);
            range.CreateRange(payload);

            return obj;
        }

        return null;
    }
}