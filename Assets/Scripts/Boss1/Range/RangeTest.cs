using Assets.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
    [SerializeField] private RangeType type = RangeType.None;
    public Transform target;
    public Material material;
    public string checkTag = "Monster";

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
                return CheckEnemiesInCone(my.transform, radius, angle, checkTag);
            case RangeType.Circle:
                return CheckEnemiesInCircle(my.transform, radius, checkTag);
            case RangeType.Trapezoid:
                return CheckEnemiesInTrapezoid(my.transform, upperBase, lowerBase, height, checkTag);
            case RangeType.Rectangle:
                return CheckEnemiesInRectangle(my.transform, width, height, checkTag);
            case RangeType.HybridCone:
                return CheckEnemiesInHybrid(my.transform, radius, angle, upperBase, checkTag);
            default:
                break;
        }

        return null;
    }

    private void GetKey()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RangeManager.Instance.isParentOn = !RangeManager.Instance.isParentOn;
        }

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
    public List<Transform> CheckEnemiesInCone(Transform rangeObject, float radius, float angle, string enemyTag = "Monster")
    {
        // ��ä���� �߽ɰ� ������ ����մϴ�.
        Vector3 center = rangeObject.position;
        Quaternion direction = rangeObject.rotation;

        // ��ä�� ���� ���� ��� �ݶ��̴��� �����մϴ�.
        Collider[] collidersInCone = Physics.OverlapSphere(center, radius);

        // �� �ݶ��̴��� �߿��� "��" �±׸� ���� �͵鸸 �����մϴ�.
        List<Transform> enemies = new List<Transform>();
        foreach (Collider collider in collidersInCone)
        {
            if (collider.tag == enemyTag)
            {
                // �� �ݶ��̴��� ��ġ�� ��ä�� ���� ���� �ִ��� Ȯ���մϴ�.
                Vector3 directionToCollider = collider.transform.position - rangeObject.position;
                float distanceToCollider = directionToCollider.magnitude;
                if (distanceToCollider < radius)
                {
                    float angleToCollider = Vector3.Angle(direction * Vector3.forward, directionToCollider);
                    if (Mathf.Abs(angleToCollider) < angle / 2)
                    {
                        enemies.Add(collider.transform);
                    }
                }
            }
        }

        // "��" �±׸� ���� �ݶ��̴��� Transform[]�� ��ȯ�մϴ�.
        return enemies;
    }


    public List<Transform> CheckEnemiesInRectangle(Transform rangeObject, float width, float height, string enemyTag = "Monster")
    {
        // �簢���� �߽ɰ� ������ ����մϴ�.
        Vector3 center = rangeObject.position;
        Quaternion direction = rangeObject.rotation;

        // �簢�� ���� ���� ��� �ݶ��̴��� �����մϴ�.
        Collider[] collidersInRectangle = Physics.OverlapBox(center, new Vector3(width / 2, 1, height / 2), direction);

        // �� �ݶ��̴��� �߿��� "��" �±׸� ���� �͵鸸 �����մϴ�.
        List<Transform> enemies = new List<Transform>();
        foreach (Collider collider in collidersInRectangle)
        {
            if (collider.tag == enemyTag)
            {
                // �� �ݶ��̴��� ��ġ�� �簢���� ���� ��ǥ��� ��ȯ�մϴ�.
                Vector3 localColliderPosition = rangeObject.InverseTransformPoint(collider.transform.position);

                // �� �ݶ��̴��� ���� ��ġ�� �簢�� ���� ���� �ִ��� Ȯ���մϴ�.
                if (Mathf.Abs(localColliderPosition.x) < width / 2 && Mathf.Abs(localColliderPosition.z) < height / 2)
                {
                    enemies.Add(collider.transform);
                }
            }
        }

        // "��" �±׸� ���� �ݶ��̴��� Transform[]�� ��ȯ�մϴ�.
        return enemies;
    }


    public List<Transform> CheckEnemiesInTrapezoid(Transform origin, float upperBase, float lowerBase, float height, string enemyTag = "Monster")
    {
        // ��ٸ����� �ʺ��� ������ ����մϴ�.
        float halfWidthAtBase = upperBase / 2f;
        float halfWidthAtTop = lowerBase / 2f;

        // ��ٸ����� ���̸� ����մϴ�.
        float area = (upperBase + lowerBase) * height / 2f;

        // ��ٸ����� �߽ɰ� ������ ����մϴ�.
        Vector3 center = origin.position + origin.forward * (height / 2f);
        Quaternion direction = origin.rotation;

        // ��ٸ��� ���� ���� ��� �ݶ��̴��� �����մϴ�.
        Collider[] collidersInTrapezoid = Physics.OverlapSphere(center, Mathf.Sqrt(area));

        // �� �ݶ��̴��� �߿��� "��" �±׸� ���� �͵鸸 �����մϴ�.
        List<Transform> enemies = new List<Transform>();
        foreach (Collider collider in collidersInTrapezoid)
        {
            if (collider.tag == enemyTag)
            {
                // �� �ݶ��̴��� ��ġ�� ��ٸ��� ���� ���� �ִ��� Ȯ���մϴ�.
                Vector3 localPoint = origin.InverseTransformPoint(collider.transform.position);
                if (localPoint.z > 0 && localPoint.z < height)
                {
                    float halfWidthAtThisPoint = Mathf.Lerp(halfWidthAtBase, halfWidthAtTop, localPoint.z / height);
                    if (Mathf.Abs(localPoint.x) < halfWidthAtThisPoint)
                    {
                        enemies.Add(collider.transform);
                    }
                }
            }
        }

        // "��" �±׸� ���� �ݶ��̴��� Transform[]�� ��ȯ�մϴ�.
        return enemies;
    }

    public List<Transform> CheckEnemiesInCircle(Transform rangeObject, float radius, string enemyTag = "Monster")
    {
        // ���� �߽ɰ� ������ ����մϴ�.
        Vector3 center = rangeObject.position;
        Quaternion direction = rangeObject.rotation;

        // �� ���� ���� ��� �ݶ��̴��� �����մϴ�.
        Collider[] collidersInCircle = Physics.OverlapSphere(center, radius);

        // �� �ݶ��̴��� �߿��� "��" �±׸� ���� �͵鸸 �����մϴ�.
        List<Transform> enemies = new List<Transform>();
        foreach (Collider collider in collidersInCircle)
        {
            if (collider.tag == enemyTag)
            {
                // �� �ݶ��̴��� ��ġ�� �� ���� ���� �ִ��� Ȯ���մϴ�.
                Vector3 localPoint = rangeObject.InverseTransformPoint(collider.transform.position);
                float distanceToCollider = Vector3.Distance(localPoint, Vector3.zero);
                if (distanceToCollider < radius)
                {
                    enemies.Add(collider.transform);
                }
            }
        }

        // "��" �±׸� ���� �ݶ��̴��� Transform[]�� ��ȯ�մϴ�.
        return enemies;
    }

    public List<Transform> CheckEnemiesInHybrid(Transform rangeObject, float radius, float angle, float upperBase, string enemyTag = "Monster")
    {
        // ���̺긮�� ������ �߽ɰ� ������ ����մϴ�.
        Vector3 center = rangeObject.position;
        Quaternion direction = rangeObject.rotation;

        // ��ä�� ������ ��(Chord)�� ���̸� ����մϴ�.
        float lowerBase = 2 * radius * Mathf.Sin(Mathf.Deg2Rad * angle / 2);

        // ���̺긮�� ���� ���� ��� �ݶ��̴��� �����մϴ�.
        Collider[] collidersInHybrid = Physics.OverlapSphere(center, radius);

        // �� �ݶ��̴��� �߿��� "��" �±׸� ���� �͵鸸 �����մϴ�.
        List<Transform> enemies = new List<Transform>();
        foreach (Collider collider in collidersInHybrid)
        {
            if (collider.tag == enemyTag)
            {
                // �� �ݶ��̴��� ��ġ�� ���̺긮�� ������ ���� ��ǥ��� ��ȯ�մϴ�.
                Vector3 localPoint = rangeObject.InverseTransformPoint(collider.transform.position);

                // ��ä�� ���� ���� �ִ��� Ȯ���մϴ�.
                if (localPoint.z >= 0 && localPoint.z <= radius && Mathf.Abs(Mathf.Atan2(localPoint.x, localPoint.z) * Mathf.Rad2Deg) <= angle / 2)
                {
                    enemies.Add(collider.transform);
                }
                // ��ٸ��� ���� ���� �ִ��� Ȯ���մϴ�.
                else if (localPoint.z >= 0 && localPoint.z <= radius)
                {
                    float halfWidthAtThisPoint = Mathf.Lerp(upperBase / 2, lowerBase / 2, localPoint.z / radius);
                    if (Mathf.Abs(localPoint.x) <= halfWidthAtThisPoint)
                    {
                        enemies.Add(collider.transform);
                    }
                }
            }
        }

        // "��" �±׸� ���� �ݶ��̴��� Transform[]�� ��ȯ�մϴ�.
        return enemies;
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
