using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView treeView;
    InspectorView inspectorView;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        // UXML �߰�
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        // UI�� ������ҵ��� �����ϱ�
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();

        // ��� ���� ���� �̺�Ʈ ����
        treeView.nodeSelectedAction = OnNodeSelectionChanged;

        OnSelectionChange();
    }

    // ui�� ������ ����Ǿ��� �� ����Ǵ� �Լ�
    private void OnSelectionChange()
    {
        // Ʈ���� Ȱ��ȭ �Ǿ��ִ��� Ȯ���ϰ�
        // Ȱ��ȭ �Ǿ� �ִٸ� Ʈ�� ���θ� ä���
        BehaviorTree tree = Selection.activeObject as BehaviorTree;
        if(tree)
        {
            treeView.PopulateView(tree);
        }
    }

    // ��� ������ ����� �� ȣ��
    private void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateView(node);
    }
}