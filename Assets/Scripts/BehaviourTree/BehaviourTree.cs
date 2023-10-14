using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
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

        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        nodes.Add(node);

        // ���� ������ �����ͺ��̽� �߰� �� ����
        // ��Ÿ�� ���¶�� ��带 �߰��� �� ���� ����
        if(!Application.isPlaying)
        {
            AssetDatabase.AddObjectToAsset(node, this);
        }

        Undo.RegisterCreatedObjectUndo(node, "Behavioure Tree (CreateNode");

        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        // ����Ʈ ����
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
        nodes.Remove(node);

        // ���� ������ �����ͺ��̽� ���� �� ����
        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        // DecoratorNode - �ڽ� ��尡 �׻� �Ѱ�, CompositeNode - �ڽ� ��尡 ������ ����
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
            decorator.child = child;
            EditorUtility.SetDirty(decorator);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
            composite.children.Add(child);
            EditorUtility.SetDirty(composite);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
            rootNode.child = child;
            EditorUtility.SetDirty(rootNode);
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
            decorator.child = null;
            EditorUtility.SetDirty(decorator);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
            composite.children.Remove(child);
            EditorUtility.SetDirty(composite);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode)
        {
            Undo.RecordObject(rootNode, "Behaviour Tree (RemoveChild)");
            rootNode.child = null;
            EditorUtility.SetDirty(rootNode);
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

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.child != null)
        {
            children.Add(rootNode.child);
        }

        return children;
    }

    // �ൿƮ���� �ڽ� ������ ��ȸ�ϸ鼭 ����
    public void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            foreach(var child in children)
            {
                Traverse(child, visiter);
            }
        }
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        tree.nodes = new List<Node>();
        Traverse(tree.rootNode, (n) => tree.nodes.Add(n));
        return tree;
    }
}
