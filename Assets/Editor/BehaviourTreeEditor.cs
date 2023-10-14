using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;
using System;

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

    // BehaviourTree ScriptableObject ������ ����Ŭ�� �ϸ�, ������ â�� ������ �����ϱ�
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if(Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
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

    // ��Ÿ�ӿ��� ������ �����ϵ�, �� ���������� ������ ���� �ʰ� �����ϱ�
    // ��ü�� ������ �ൿƮ���� ��Ÿ�ӿ��� ��������� �����ϰ� �׽�Ʈ �� �� �ְ� �����Ѵ�.
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }


    // ui�� ������ ����Ǿ��� �� ����Ǵ� �Լ�
    private void OnSelectionChange()
    {
        // Ʈ���� Ȱ��ȭ �Ǿ��ִ���, �� �� �ִ��� Ȯ���ϰ�
        // Ȱ��ȭ �Ǿ� �ִٸ� Ʈ�� ���θ� ä���
        BehaviourTree tree = Selection.activeObject as BehaviourTree;

        if(!tree)
        {
            if (Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();   
                if(runner != null)
                {
                    tree = runner.tree;
                }
            }
        }

        // ��Ÿ�ӿ����� ������ �����ϰ� ����
        if(Application.isPlaying)
        {
            if (tree)
            {
                treeView.PopulateView(tree);
            }
        }
        else
        {
            if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView.PopulateView(tree);
            }
        }

        if(tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
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