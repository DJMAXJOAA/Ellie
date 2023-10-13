using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public class BehaviourTreeView : GraphView
{
    public Action<NodeView> nodeSelectedAction;

    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    private BehaviourTree tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        // �ൿ Ʈ�� GUI â���� �ǵ����� �� ȭ�� ������Ʈ �� �� �ְ� �̺�Ʈ ����
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    // �ൿƮ�� GUI ȭ���� �ʱ�ȭ 
    public void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // ��Ʈ ��尡 ������ ���� �����Ѵ�.
        if(tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;

            // ������ ��ƿ��Ƽ�� �ҷ��ͼ� �Ľ�. ������ ����
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // ��� �������� �ҷ��ͼ� �����Ѵ�
        foreach (var node in tree.nodes)
        {
            CreateNodeView(node);
        }

        // ������ ���� �������� �ҷ��ͼ� �����Ѵ�
        foreach (var node in tree.nodes)
        {
            CreateEdgeView(node);
        }
    }

    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    // startPort�� ȣȯ�� �Ǵ� ��Ʈ���� ����� ��ȯ�ϴ� �Լ� (ȣȯ�� üũ)
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        // �Է� ��Ʈ�� ��� ��Ʈ�� ȣȯ�� �Ǵ� (startPort�� endPort�� ������ �޶�� �Ѵ�)
        // LINQ�� Where()�� ���ǿ� �´� ��Ʈ�鸸 ���͸� -> ����� ����Ʈ�� ��ȯ
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    // ��Ŀ���� �ٸ�â���� �Ѿ�ٰ� �ٽ� �ൿƮ�� GUI�� �Ѿ�� �� ȣ��
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // ������Ʈ���� �ٸ� ������ �� ��(��Ŀ���� �ٸ� â���� �Ѿ��) ������ ���Ž������ �Ѵ�.
        // üũ�� ������ ������ ��峪 ������ �ߺ����� �����ȴ�.
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var elem in graphViewChange.elementsToRemove)
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.RemoveChild(parentView.node, childView.node);
                }
            }
        }

        // ������ �����Ǿ��� �� ����� �ڽ��� �߰������ش�.
        if(graphViewChange.edgesToCreate != null)
        {
            foreach(var edge in graphViewChange.edgesToCreate)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            }
        }

        return graphViewChange;
    }

    // �ൿƮ�� GUI�� ������ Ŭ�� �޴��� �߰��ϴ� �Լ�
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        {
            // TypeCache�� Ư�� ����(���⼱ ActionNode)���� �Ļ��� ��� Ÿ���� ��ȯ����
            // ActionNode���� �Ļ��� Ŭ������(������ �����ϴ� �׼� ������ ���)�� ǥ�ý�Ű�� ����
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                // �޴��� '[�⺻Ÿ�� �̸�] �Ļ��� Ÿ�� �̸�' ���� ǥ��
                // ex : [Action Node] DebugLogNode
                // �޴��� ����Ŭ������ �����ϸ� (a) => CreateNode(type) �Լ��� ȣ��Ǿ� ��尡 ������
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    private void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.nodeSelectedAction = nodeSelectedAction;
        AddElement(nodeView);
    }

    private void CreateEdgeView(Node node)
    {
        var children = tree.GetChildren(node);
        foreach (var child in children)
        {
            NodeView parentView = FindNodeView(node);
            NodeView childView = FindNodeView(child);

            Edge edge = parentView.output.ConnectTo(childView.input);
            AddElement(edge);
        }
    }
}
