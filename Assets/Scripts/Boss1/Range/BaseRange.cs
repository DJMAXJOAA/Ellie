using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseRange : Poolable
{
	// �����ϴ� ���� ������Ʈ
	protected GameObject rangeObject;

	public Material DetectionMaterial { get; protected set; }

    private const float RAYCAST_OFFSET = 2.0f;
	private const float POSITION_OFFSET = 0.3f;

	public BaseRange(Transform objTransform, bool isSetParent = false)
	{
        rangeObject = new GameObject();

		if(isSetParent)
		{
			InitSetParent(objTransform);
		}
		else
		{
			Init(objTransform);
		}
	}

	private void Init(Transform objTransform)
	{
        // Ground ���̾ ������ �ִ� ������Ʈ�� �����ϴ� ���̾� ����ũ�� �����մϴ�.
        int groundLayer = LayerMask.GetMask("Ground");

        // -Vector3.up �������� Raycast�� �߻��մϴ�.
        RaycastHit hit;
        Vector3 checkPosition = objTransform.position + new Vector3(0, RAYCAST_OFFSET, 0);
        if (Physics.Raycast(checkPosition, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
        {
            // Raycast�� Ground ���̾ ������ �ִ� ������Ʈ�� �¾Ҵٸ�, �� ��ġ�� ���� ������Ʈ�� �����մϴ�.
            objTransform.position = hit.point + new Vector3(0, POSITION_OFFSET, 0);
        }

        rangeObject.transform.position = objTransform.position;
        rangeObject.transform.rotation = objTransform.rotation;
    }

	private void InitSetParent(Transform objTransform)
	{
        rangeObject.transform.SetParent(objTransform, false); // ������ �θ� ������Ʈ�� ���� ��ǥ�� �����ϴ�.
        rangeObject.transform.localPosition = new Vector3(0, POSITION_OFFSET, 0); // �θ� ������Ʈ�� ��ġ�� ����ϴ�.
        rangeObject.transform.localRotation = Quaternion.identity; // �θ� ������Ʈ�� ȸ���� ����ϴ�.
    }

	public abstract void CreateRange(RangePayload payload);
	public abstract List<Transform> CheckRange(string checkTag = null, int layerMask = -1);
}
