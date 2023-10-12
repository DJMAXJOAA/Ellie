using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

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

    public void AddChild(Node parent, Node child)
    {
        // DecoratorNode - �ڽ� ��尡 �׻� �Ѱ�, CompositeNode - �ڽ� ��尡 ������ ����
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Add(child);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            decorator.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            composite.children.Remove(child);
        }
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }

        return children;
    }
}
