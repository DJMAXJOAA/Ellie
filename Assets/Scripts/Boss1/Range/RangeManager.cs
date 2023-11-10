using System;
using System.Collections;
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
    public bool isParentOn = false;

    private Material detectionMaterial;

    private void Start()
    {
        detectionMaterial = new Material(Shader.Find("Standard"));
    }

    public GameObject CreateRange(Transform parent, RangePayload payload)
    {
        if (payload == null)
            return null;

        Material material = payload.DetectionMaterial ?? detectionMaterial;
        GameObject rangeObject;
        if (isParentOn)
        {
            rangeObject = InitGameObject("Range", parent);
        }
        else
        {
            rangeObject = InitGameObject("Range", parent.position, parent.rotation);
        }

        if (payload.DetectionMaterial != null)
            material = payload.DetectionMaterial;

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
            case RangeType.HybridCone:
                CreateHybrid(rangeObject, payload.Radius, payload.Angle, payload.UpperBase, material);
                break;
        }
        return rangeObject;
    }

    private GameObject InitGameObject(string objName, Transform parent)
    {
        GameObject sectorObject = new GameObject(objName);
        sectorObject.transform.SetParent(parent, false); // ������ �θ� ������Ʈ�� ���� ��ǥ�� �����ϴ�.
        sectorObject.transform.localPosition = new Vector3(0, 0.3f, 0); // �θ� ������Ʈ�� ��ġ�� ����ϴ�.
        sectorObject.transform.localRotation = Quaternion.identity; // �θ� ������Ʈ�� ȸ���� ����ϴ�.

        return sectorObject;
    }

    private GameObject InitGameObject(string objName, Vector3 position, Quaternion rotation)
    {
        // Ground ���̾ ������ �ִ� ������Ʈ�� �����ϴ� ���̾� ����ũ�� �����մϴ�.
        int groundLayer = LayerMask.GetMask("Ground");

        // -Vector3.up �������� Raycast�� �߻��մϴ�.
        RaycastHit hit;
        Vector3 checkPosition = position + new Vector3(0, 2.0f, 0);
        if (Physics.Raycast(checkPosition, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            // Raycast�� Ground ���̾ ������ �ִ� ������Ʈ�� �¾Ҵٸ�, �� ��ġ�� ���� ������Ʈ�� �����մϴ�.
            position = hit.point + new Vector3(0, 0.3f, 0);
        }

        // ������Ʈ�� �����մϴ�.
        GameObject sectorObject = new GameObject(objName);
        sectorObject.transform.position = position;
        sectorObject.transform.rotation = rotation;

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
            triangles[i * 3 + 1] = i + 2;
            triangles[i * 3 + 2] = i + 1;
        }

        //// ������ �ﰢ���� �����մϴ�.
        //triangles[(segments - 1) * 3] = 0;
        //triangles[(segments - 1) * 3 + 1] = segments;
        //triangles[(segments - 1) * 3 + 2] = 1;

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