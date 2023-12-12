using Assets.Scripts.Centers;
using Assets.Scripts.Data.GoogleSheet;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Inventory;
using Boss.Terrapupa;
using Channels.Components;
using Channels.Type;
using Channels.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatClient : SerializedMonoBehaviour
{
    public Transform player;
    public Transform terrapupa;
    public Transform terra;
    public Transform pupa;
    public List<Transform> minions;

    private GameObject canvas;

    private bool IsRuntime
    {
        get { return Application.isPlaying; }
    }
    private bool IsParsingDone
    {
        get { return DataManager.Instance.isParseDone; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            OnOffCanvas();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SaveLoadManager.Instance.SaveData();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveLoadManager.Instance.LoadData();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            KillTerrapupa();
            KillTerraAndPupa();
            KillMinions();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DamageTerrapupa();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeactivateBoss();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SkipToEnding();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // TELEPORT START
            SetPlayerPosition1();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            // TELEPORT BOSS ROOM
            SetPlayerPosition2();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            // TELEPORT STONE FOOTBOARD PUZZLE
            SetPlayerPosition3();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            // TELEPORT BREAKABLE STONE PUZZLE
            SetPlayerPosition4();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            // TELEPORT PILLAR PUZZLE
            SetPlayerPosition5();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            // TELEPORT RAIL START
            SetPlayerPosition6();
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            // TELEPORT NPC1
            SetPlayerPosition7();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            // TELEPORT NPC2
            SetPlayerPosition8();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            // TELEPORT NPC3
            SetPlayerPosition9();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {

        }
        if (Input.GetKeyDown(KeyCode.F11))
        {

        }
        if (Input.GetKeyDown(KeyCode.F12))
        {

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem();
        }
    }

    /// <summary>
    /// ����� ���� �̵�
    /// </summary>

    [Title("����� ���� �ڷ���Ʈ")]
    [ValueDropdown("savePositionList")]
    public Vector3 savePosition = Vector3.zero;
    private List<Vector3> savePositionList = new List<Vector3>();

    [Button("�÷��̾� ������ ����", ButtonSizes.Large)]
    public void SavePosition()
    {
        savePositionList.Add(player.position);
    }

    [Button("�÷��̾� ���� ��ġ ����Ʈ ����", ButtonSizes.Large)]
    public void ResetPositionList()
    {
        savePositionList.Clear();
        savePosition = Vector3.zero;
    }

    [Button("������ ������ġ�� �÷��̾� ��ġ �̵�", ButtonSizes.Large)]
    public void SetPlayerPositionToSavedPosition()
    {
        player.position = savePosition;
    }

    /// <summary>
    /// ���� ��ġ �̵�
    /// </summary>

    [Title("���� ��� �̵�")]
    [Button("���� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition1()
    {
        player.position = new Vector3(-5.44f, 7.03f, -10.4f);
    }

    [Button("���� �� �� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition2()
    {
        player.position = new Vector3(-152f, 13.92f, 645.51f);
    }

    [Button("�� ���� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition3()
    {
        player.position = new Vector3(33.5f, 11.8f, 98.8f);
    }

    [Button("�� �μ��� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition4()
    {
        player.position = new Vector3(-115.6f, 13.02f, 390.79f);
    }

    [Button("�� �ֱ� ���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition5()
    {
        player.position = new Vector3(39.0f, 3.28f, 14.81f);
    }

    [Button("���� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition6()
    {
        player.position = new Vector3(32.96f, 13.56f, 155.13f);
    }

    [Button("NPC 1�� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition7()
    {
        player.position = new Vector3(-68.019f, 3.16f, 74.72f);
    }

    [Button("NPC 2�� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition8()
    {
        player.position = new Vector3(-68.01f, 3.16f, 112.71f);
    }

    [Button("NPC 3�� �̵�", ButtonSizes.Large)]
    public void SetPlayerPosition9()
    {
        player.position = new Vector3(79.38f, 3.16f, 85.80f);
    }

    /// <summary>
    /// ���� ġƮŰ
    /// </summary>

    [Title("���� ġƮŰ")]
    [EnableIf("IsRuntime")]
    [Button("1������ ��ŵ", ButtonSizes.Large)]
    public void KillTerrapupa()
    {
        Debug.Log("�׶�Ǫ�� ��� ġƮ");
        terrapupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("2������ ��ŵ", ButtonSizes.Large)]
    public void KillTerraAndPupa()
    {
        Debug.Log("�׶�, Ǫ�� ��� ġƮ");

        terra.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
        pupa.GetComponent<TerrapupaBTController>().terrapupaData.currentHP.Value = 0;
    }
    [EnableIf("IsRuntime")]
    [Button("3������ ��ŵ", ButtonSizes.Large)]
    public void KillMinions()
    {
        Debug.Log("�̴Ͼ� ��� ġƮ");

        foreach (var minion in minions)
        {
            minion.GetComponent<TerrapupaMinionBTController>().minionData.currentHP.Value = 0;
        }
    }
    [EnableIf("IsRuntime")]
    [Button("�׶�Ǫ�� ������", ButtonSizes.Large)]
    public void DamageTerrapupa()
    {
        Debug.Log("�׶�, Ǫ�� ������ ������");

        terrapupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        terra.GetComponent<TerrapupaBTController>().GetDamaged(1);
        pupa.GetComponent<TerrapupaBTController>().GetDamaged(1);
        foreach (var minion in minions)
        {
            minion.GetComponent<TerrapupaMinionBTController>().GetDamaged(1);
        }
    }
    [EnableIf("IsRuntime")]
    [Button("�׶�Ǫ�� ������", ButtonSizes.Large)]
    public void DeactivateBoss()
    {
        Debug.Log("���� ��Ȱ��ȭ");

        terrapupa.gameObject.SetActive(false);
        terra.gameObject.SetActive(false);
        pupa.gameObject.SetActive(false);
        foreach (var minion in minions)
        {
            minion.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ������ ġƮŰ
    /// </summary>
    /// 
    [Title("������ ȹ��")]
    [EnableIf("IsRuntime")]
    [EnableIf("IsParsingDone")]
    [Button("������ ȹ��", ButtonSizes.Large)]
    public void AddItem()
    {
        TicketMachine ticketMachine = player.GetComponent<TicketMachine>();

        for (int i = 0; i < 20; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4000));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4003));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4017));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4019));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4020));
            ticketMachine.SendMessage(ChannelType.UI, GenerateStoneAcquirePayloadTest(4021));
        }

        for (int i = 0; i < 5; i++)
        {
            ticketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                groupType = GroupType.Item,
                slotAreaType = SlotAreaType.Item,
                actionType = ActionType.AddSlotItem,
                itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4100),
            });

            ticketMachine.SendMessage(ChannelType.UI, new UIPayload
            {
                uiType = UIType.Notify,
                groupType = GroupType.Item,
                slotAreaType = SlotAreaType.Item,
                actionType = ActionType.AddSlotItem,
                itemData = DataManager.Instance.GetIndexData<ItemData, ItemDataParsingInfo>(4101),
            });
        }
    }


    /// <summary>
    /// ��Ÿ
    /// </summary>
    /// 
    [Title("��Ÿ")]
    [EnableIf("IsRuntime")]
    [Button("���� ��ŵ", ButtonSizes.Large)]
    public void SkipToEnding()
    {
        Debug.Log("�������� ��ŵ");

        SceneLoadManager.Instance.LoadScene(SceneName.Closing);
    }

    [EnableIf("IsRuntime")]
    [Button("ĵ���� �״ٲ���", ButtonSizes.Large)]
    public void OnOffCanvas()
    {
        if(canvas == null)
        {
            canvas = GameObject.Find("@UI_Root");
        }

        if(canvas.active == true)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
        }
    }

    private UIPayload GenerateStoneAcquirePayloadTest(int index)
    {
        //for test
        UIPayload payload = new UIPayload();
        payload.uiType = UIType.Notify;
        payload.actionType = ActionType.AddSlotItem;
        payload.slotAreaType = SlotAreaType.Item;
        payload.groupType = GroupType.Stone;
        payload.itemData = DataManager.Instance.GetIndexData<StoneData, StoneDataParsingInfo>(index);
        return payload;
    }
}
