using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    None,
    Cone,
    Circle,
    Trapezoid,
    Rectangle,
    Hybrid,
}

public class RangePayload
{
    public RangeType Type { get; set; }
    // ��ä��, ��
    public Material DetectionMeterial { get; set; }
    public float Radius { get; set; }
    public float Angle { get; set; }
    // �簢��(Width, Height), ��ٸ���(���ۺκ��� Upper)
    public float Height { get; set; }
    public float Width { get; set; }
    public float UpperBase { get; set; }
    public float LowerBase { get; set; }
}

public class RangeManager : Singleton<RangeManager>
{
    private Material detectionMaterial;

    private void Start()
    {
        detectionMaterial = new Material(Shader.Find("Standard"));
    }

    public GameObject CreateRange(Transform parent, RangePayload payload)
    {
        if (payload == null)
            return null;

        Material material = payload.DetectionMeterial ?? detectionMaterial;
        GameObject rangeObject = InitGameObject("Range", parent);

        if (payload.DetectionMeterial != null)
            material = payload.DetectionMeterial;

        switch (payload.Type)
        {
            case RangeType.Cone:
                CreateCone(rangeObject, payload.Radius, payload.Angle, material);
                break;
            case RangeType.Circle:
                CreateCone(rangeObject, payload.Radius, 360.0f, material);
                break;
            case RangeType.Trapezoid:
                CreateTrapezoid(rangeObject, payload.UpperBase, payload.LowerBase, payload.Height, material);
                break;
            case RangeType.Rectangle:
                CreateRectangle(rangeObject, payload.Width, payload.Height, material);
                break;
            case RangeType.Hybrid:
                CreateHybrid(rangeObject, payload.Radius, payload.Angle, payload.UpperBase, material);
                break;
        }
        return rangeObject;
    }

    private GameObject InitGameObject(string objName, Transform parent)
    {
        GameObject sectorObject = new GameObject(objName);
        sectorObject.transform.SetParent(parent, false); // ������ �θ� ������Ʈ�� ���� ��ǥ�� �����ϴ�.
        sectorObject.transform.localPosition = new Vector3(0, 0.2f, 0); // �θ� ������Ʈ�� ��ġ�� ����ϴ�.
        sectorObject.transform.localRotation = Quaternion.identity; // �θ� ������Ʈ�� ȸ���� ����ϴ�.

        return sectorObject;
    }

    private void CreateCone(GameObject target, float radius, float angle, Material detectionMaterial)
    {
        // MeshFilter�� MeshRenderer ������Ʈ�� �߰��մϴ�.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // ��ä���� �߽ɰ��� ����Ͽ� ���׸�Ʈ�� ���� �����մϴ�.
        int segments = Mathf.CeilToInt(angle); // ���⿡�� ���׸�Ʈ�� ���� �����ϰ� �����մϴ�.
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // ���� �߽���
        float currentAngle = -angle / 2; // ��ä���� ���� ����
        float deltaAngle = angle / segments; // �� ���׸�Ʈ ������ ���� ����

        for (int i = 0; i <= segments; i++)
        {
            float radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * radius, 0, Mathf.Cos(radian) * radius);
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // ������ �ﰢ���� �����մϴ�.
        triangles[(segments - 1) * 3] = 0;
        triangles[(segments - 1) * 3 + 1] = segments;
        triangles[(segments - 1) * 3 + 2] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = detectionMaterial;
    }


    private void CreateRectangle(GameObject target, float width, float height, Material detectionMaterial)
    {
        // MeshFilter�� MeshRenderer ������Ʈ�� �߰��մϴ�.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // �簢���� �� �������� �����մϴ�.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-width / 2, 0, -height / 2); // ���� �Ʒ� ������
        vertices[1] = new Vector3(-width / 2, 0, height / 2); // ���� �� ������
        vertices[2] = new Vector3(width / 2, 0, -height / 2); // ������ �Ʒ� ������
        vertices[3] = new Vector3(width / 2, 0, height / 2); // ������ �� ������

        // �� ���� �ﰢ���� �̾ �簢���� ����ϴ�.
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = detectionMaterial;
    }


    private void CreateTrapezoid(GameObject target, float upperBase, float lowerBase, float height, Material detectionMaterial)
    {
        // MeshFilter�� MeshRenderer ������Ʈ�� �߰��մϴ�.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // ��ٸ����� �� �������� �����մϴ�.
        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(-upperBase / 2, 0, 0); // ���� �Ʒ� ������
        vertices[1] = new Vector3(upperBase / 2, 0, 0); // ������ �Ʒ� ������
        vertices[2] = new Vector3(-lowerBase / 2, 0, height); // ���� �� ������
        vertices[3] = new Vector3(lowerBase / 2, 0, height); // ������ �� ������

        // �� ���� �ﰢ���� �̾ ��ٸ����� ����ϴ�.
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = detectionMaterial;
    }

    private void CreateHybrid(GameObject target, float radius, float angle, float upperBase, Material detectionMaterial)
    {
        // MeshFilter�� MeshRenderer ������Ʈ�� �߰��մϴ�.
        MeshFilter meshFilter = target.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = target.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        // ��ä���� �߽ɰ��� ����Ͽ� ���׸�Ʈ�� ���� �����մϴ�.
        int segments = Mathf.CeilToInt(angle);
        Vector3[] vertices = new Vector3[segments + 3];
        int[] triangles = new int[(segments + 2) * 3];

        float currentAngle = -angle / 2;
        float deltaAngle = angle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float radian = currentAngle * Mathf.Deg2Rad;
            vertices[i + 1] = new Vector3(Mathf.Sin(radian) * radius, 0, Mathf.Cos(radian) * radius);
            currentAngle += deltaAngle;
        }

        // �߽������� UpperBase��ŭ ������ ��ġ�� �� ���� �������� �߰��մϴ�.
        vertices[0] = new Vector3(-upperBase / 2, 0, 0);
        vertices[segments + 2] = new Vector3(upperBase / 2, 0, 0);

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        // ��ٸ��� �κ��� �ﰢ���� �����մϴ�.
        triangles[segments * 3] = 0;
        triangles[segments * 3 + 1] = segments + 1;
        triangles[segments * 3 + 2] = segments + 2;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        meshRenderer.material = detectionMaterial;
    }
}