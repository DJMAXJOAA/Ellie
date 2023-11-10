using Assets.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
    [SerializeField] private RangeType type = RangeType.None;
    public string checkTag = "Monster";

    public Transform target;
    public Material material;
    public float radius = 10.0f;
    public float angle = 60.0f;
    public float height = 10.0f;
    public float width = 5.0f;
    public float upperBase = 5.0f;
    public float lowerBase = 5.0f;

    private List<Transform> transforms = new List<Transform>();
    private GameObject my;

    private void Update()
    {
        GetKey();
    }

    private List<Transform> RangeCheck()
    {
        switch (type)
        {
            case RangeType.Cone:
                ConeRange cone = my.GetComponent<ConeRange>();
                return cone.CheckRange(checkTag);
            case RangeType.Circle:
                CircleRange circle = my.GetComponent<CircleRange>();
                return circle.CheckRange(checkTag);
            case RangeType.Trapezoid:
                TrapezoidRange trapezoid = my.GetComponent<TrapezoidRange>();
                return trapezoid.CheckRange(checkTag);
            case RangeType.Rectangle:
                RectangleRange rectangle = my.GetComponent<RectangleRange>();
                return rectangle.CheckRange(checkTag);
            case RangeType.HybridCone:
                HybridConeRange hybridCone = my.GetComponent<HybridConeRange>();
                return hybridCone.CheckRange(checkTag);
            default:
                break;
        }

        return null;
    }

    private void GetKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (my != null)
                Destroy(my);

            my = RangeManager.Instance.CreateRange(target, new RangePayload
            {
                Type = RangeType.Cone,
                Radius = radius,
                Angle = angle,
                DetectionMaterial = material,
            });

            type = RangeType.Cone;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (my != null)
                Destroy(my);

            my = RangeManager.Instance.CreateRange(target, new RangePayload
            {
                Type = RangeType.Circle,
                Radius = radius,
                DetectionMaterial = material,
            });

            type = RangeType.Circle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (my != null)
                Destroy(my);

            my = RangeManager.Instance.CreateRange(target, new RangePayload
            {
                Type = RangeType.Trapezoid,
                UpperBase = upperBase,
                LowerBase = lowerBase,
                Height = height,
                DetectionMaterial = material,
            });

            type = RangeType.Trapezoid;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (my != null)
                Destroy(my);

            my = RangeManager.Instance.CreateRange(target, new RangePayload
            {
                Type = RangeType.Rectangle,
                Width = width,
                Height = height,
                DetectionMaterial = material,
            });

            type = RangeType.Rectangle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (my != null)
                Destroy(my);

            my = RangeManager.Instance.CreateRange(target, new RangePayload
            {
                Type = RangeType.HybridCone,
                UpperBase = upperBase,
                Radius = radius,
                Angle = angle,
                DetectionMaterial = material,
            });

            type = RangeType.HybridCone;
        }
    }

    void OnGUI()
    {
        List<Transform> enemies = RangeCheck();
        if (enemies == null)
            return;

        // �۲� ��Ÿ���� �����ϰ� �����մϴ�.
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 20;

        // �ڽ��� ��ġ�� ũ�⸦ �����մϴ�.
        float boxWidth = 200;
        float boxHeight = 25 * transforms.Count + 10;
        float boxX = Screen.width - boxWidth - 10;
        float boxY = 10;

        // �ڽ��� �׸��ϴ�.
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "");

        // ���� �׸� ��ġ�� �����մϴ�.
        float x = boxX + 5;
        float y = boxY + 5;


        foreach (Transform enemy in enemies)
        {
            // �÷��̾�� �� ������ �Ÿ��� ����մϴ�.
            float distance = Vector3.Distance(target.root.position, enemy.position);

            // ȭ�鿡 �ؽ�Ʈ�� ǥ���մϴ�.
            GUI.Label(new Rect(x, y, boxWidth, 20), $"{enemy.name} (Distance: {distance})", style);

            // ���� ���� y ��ǥ�� �����մϴ�.
            y += 25;
        }

    }

    void OnDrawGizmos()
    {
        // �簢���� �߽ɰ� ������ ����մϴ�.
        Vector3 center = target.position;
        Quaternion direction = target.rotation;

        // Gizmos�� ����Ͽ� ���̺긮�� ������ �׸��ϴ�.
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, direction, Vector3.one);
        Gizmos.DrawRay(Vector3.zero, Quaternion.Euler(0, -angle / 2, 0) * Vector3.forward * radius);
        Gizmos.DrawRay(Vector3.zero, Quaternion.Euler(0, angle / 2, 0) * Vector3.forward * radius);
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(upperBase, 1, 0));
    }
}
