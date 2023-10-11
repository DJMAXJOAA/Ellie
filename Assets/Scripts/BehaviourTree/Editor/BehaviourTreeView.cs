using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    public BehaviourTreeView()
    {
        // �׸��� ��׶��� ����
        Insert(0, new GridBackground());

        // 4���� ���۱� �߰��ϱ�
        this.AddManipulator(new ContentZoomer());           // Ȯ��, ���
        this.AddManipulator(new ContentDragger());          // �׷��� â�� �̵�
        this.AddManipulator(new SelectionDragger());        // ��� ���� �巡��
        this.AddManipulator(new RectangleSelector());       // ���� ���� �巡��

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
