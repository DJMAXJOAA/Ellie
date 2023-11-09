using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGizmos : MonoBehaviour
{
    public GameObject testClient;

    public float radius = 10f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Gizmo ���� ����
        Gizmos.DrawWireSphere(testClient.transform.position, radius);
    }
}
