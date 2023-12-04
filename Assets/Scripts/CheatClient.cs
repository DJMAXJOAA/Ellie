using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatClient : OdinEditorWindow
{
    public Transform player;
    public Transform terrapupa;
    public Transform terra;
    public Transform pupa;

    [Title("����� ���� �ڷ���Ʈ")]
    [ValueDropdown("savePositionList")]
    public Vector3 savePosition = Vector3.zero;
    private List<Vector3> savePositionList = new List<Vector3>();

    [MenuItem("ġƮŰ/�ΰ��� ġƮŰ")]
    private static void OpenWindow()
    {
        GetWindow<CheatClient>().Show();
    }

    private void Awake()
    {
        FindObject();
    }

    private void FindObject()
    {
        player = FindObject("Player");
        terrapupa = FindObject("Terrapupa");
        terra = FindObject("Terra");
        pupa = FindObject("Pupa");
    }

    private Transform FindObject(string objName)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj != null)
        {
            return obj.transform;
        }
        else
        {
            Debug.LogError($"{objName} ������Ʈ�� �� �ȿ� �����ϴ�");
        }
        return null;
    }

    [Button("�÷��̾� ������ ����", ButtonSizes.Medium)]
    public void SavePosition()
    {
        savePositionList.Add(player.position);
    }

    [Button("�÷��̾� ���� ��ġ ����Ʈ ����", ButtonSizes.Medium)]
    public void ResetPositionList()
    {
        savePositionList.Clear();
        savePosition = Vector3.zero;
    }

    [Button("������ ������ġ�� �÷��̾� ��ġ �̵�", ButtonSizes.Medium)]
    public void SetPlayerPositionToSavedPosition()
    {
        player.position = savePosition;
    }

    [Title("���� ��� �̵�")]
    [Button("���� ���� �̵�", ButtonSizes.Small)]
    public void SetPlayerPosition1()
    {
        player.position = new Vector3(-5.44f, 7.03f, -10.4f);
    }

    [Button("���� �� �� �̵�", ButtonSizes.Small)]
    public void SetPlayerPosition2()
    {
        player.position = new Vector3(-152f, 13.92f, 645.51f);
    }

    [Button("�� ���� ���� �̵�", ButtonSizes.Small)]
    public void SetPlayerPosition3()
    {
        player.position = new Vector3(33.5f, 11.8f, 98.8f);
    }
}
