using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviorTree : ScriptableObject
{
    public Node rootNode;   // ���� ���� ���
    public Node.State treeState = Node.State.Running;   // ���� ����� ����
    public List<Node> nodes = new List<Node>();

    public Node.State Update()
    {
        // ���� Running ������ ���� ������Ʈ�� ����ǰ�
        if(rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }

        return treeState;
    }

    public Node CreateNode(System.Type type)
    {
        // ��ũ���ͺ� ������Ʈ �ν��Ͻ��� �����, ���� �� ����Ʈ�� ����
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        // ���� ������ �����ͺ��̽� �߰� �� ����
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        // ����Ʈ ����
        nodes.Remove(node);

        // ���� ������ �����ͺ��̽� ���� �� ����
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }
}
