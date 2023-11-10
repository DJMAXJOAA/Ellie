using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseRange : Poolable
{
	// �����ϴ� ���� ������Ʈ
	public GameObject RangeObject { get; private set; }
    public Transform Parent { get; private set; }

	public Material DetectionMaterial { get; protected set; }

    private const float RAYCAST_OFFSET = 2.0f;
	private const float POSITION_OFFSET = 0.3f;

	public virtual void Init(GameObject rangeObject, Transform objTransform, bool isSetParent)
	{
        RangeObject = rangeObject;

        if (isSetParent)
        {
            InitSetParent(objTransform);
        }
        else
        {
            InitSetNone(objTransform);
        }
    }

	private void InitSetNone(Transform objTransform)
	{
        // Ground ���̾ ������ �ִ� ������Ʈ�� �����ϴ� ���̾� ����ũ�� �����մϴ�.
        Vector3 position = objTransform.position;
        Quaternion rotation = objTransform.rotation;
        int groundLayer = LayerMask.GetMask("Ground");

        // -Vector3.up �������� Raycast�� �߻��մϴ�.
        RaycastHit hit;
        Vector3 checkPosition = position + new Vector3(0, RAYCAST_OFFSET, 0);
        if (Physics.Raycast(checkPosition, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            // Raycast�� Ground ���̾ ������ �ִ� ������Ʈ�� �¾Ҵٸ�, �� ��ġ�� ���� ������Ʈ�� �����մϴ�.
            position = hit.point + new Vector3(0, POSITION_OFFSET, 0);
        }

        RangeObject.transform.position = position;
        RangeObject.transform.rotation = rotation;
    }

	private void InitSetParent(Transform objTransform)
	{
        Parent = objTransform;
        RangeObject.transform.SetParent(objTransform, false); // ������ �θ� ������Ʈ�� ���� ��ǥ�� �����ϴ�.
        RangeObject.transform.localPosition = new Vector3(0, POSITION_OFFSET, 0); // �θ� ������Ʈ�� ��ġ�� ����ϴ�.
        RangeObject.transform.localRotation = Quaternion.identity; // �θ� ������Ʈ�� ȸ���� ����ϴ�.
    }

	public abstract void CreateRange(RangePayload payload);
	public abstract List<Transform> CheckRange(string checkTag = null, int layerMask = -1);
}
