using System;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.UI.Item.PopupInven;
using Assets.Scripts.Utils;
using Channels.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlot : UIBase, ISettable
    {
        private enum Images
        {
            ItemImage
        }

        private readonly int fontSize = 28;
        private readonly float lineHeight = 25.0f;

        public int Index { get; set; }
        public BaseItem SlotItemData { get; set; }
        public SlotAreaType SlotType { get; set; }
        public SlotItemPosition SlotItemPosition { get; private set; }

        private RectTransform rect;
        private readonly List<InventorySlot> copylist = new List<InventorySlot>();
        private Image itemImage;

        private Action<InventoryEventPayload> slotInventoryAction;

        // for Equipment Frame
        // public 

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<Image>(typeof(Images));

            itemImage = GetImage((int)Images.ItemImage);
            SlotItemPosition = itemImage.gameObject.GetOrAddComponent<SlotItemPosition>();

            gameObject.BindEvent(OnDropHandler, UIEvent.Drop);
        }

        private void InitObjects()
        {
            SlotItemPosition.slot = this;
            SlotItemData = null;

            rect = GetComponent<RectTransform>();
            AnchorPresets.SetAnchorPreset(rect, AnchorPresets.MiddleCenter);
            rect.sizeDelta = InventoryConst.SlotRect.GetSize();
            rect.localPosition = InventoryConst.SlotRect.ToCanvasPos();
        }

        public void SetViewMode(bool isTrue)
        {
            var itemColor = itemImage.color;
            itemColor.a = isTrue ? 1.0f : 0.0f;
            itemImage.color = itemColor;
        }

        public void SetSprite(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        // baseSlotItem에서 이벤트 호출할 수 있도록 열어둠
        public void InvokeSlotItemEvent(InventoryEventPayload payload)
        {
            slotInventoryAction?.Invoke(payload);
        }

        // !TODO: 이전 장착 슬롯 클리어하는 함수인데 필요 없을 수도 있음
        public void InvokeClearEquipFrame(BaseSlotItem baseSlotItem)
        {
            var payload = new InventoryEventPayload()
            {
                eventType = InventoryEventType.UnEquipItem,
                groupType = baseSlotItem.SlotItemData.itemData.groupType,
                baseSlotItem = baseSlotItem,
                slot = this,
            };

            slotInventoryAction?.Invoke(payload);
        }

        // !TODO: 여기에서 equipment 미러링 중계해야 할 수도 있음
        public void InvokeCopyOrMove(BaseSlotItem baseSlotItem)
        {
            InvokeClearEquipFrame(baseSlotItem);

            var payload = new InventoryEventPayload
            {
                baseSlotItem = baseSlotItem,
                slot = this,
            };

            // origin items
            if (baseSlotItem.IsOrigin())
            {
                // copy
                if (SlotType == SlotAreaType.Equipment)
                {
                    payload.eventType = InventoryEventType.CopyItemWithDrag;
                }
                else
                {
                    // move
                    payload.eventType = InventoryEventType.MoveItem;
                }
            }
            // copy items
            else
            {
                if (SlotType == SlotAreaType.Equipment)
                {
                    // move
                    payload.eventType = InventoryEventType.MoveItem;
                }
                else
                {
                    // do nothing
                    return;
                }
            }

            slotInventoryAction?.Invoke(payload);
        }

        // 슬롯에 아이템 장착
        // 아이템 정보, 슬롯 인덱스
        private void OnDropHandler(PointerEventData data)
        {
            // Description은 읽기 전용
            if (SlotType == SlotAreaType.Description)
                return;

            if (SlotItemData != null)
            {
                Debug.Log($"이미 아이템이 존재하는 슬롯 {SlotItemData.ItemName}");
                return;
            }

            var droppedItem = data.pointerDrag;
            var baseSlotItem = droppedItem.GetComponent<BaseSlotItem>();
            if (baseSlotItem == null)
                return;

            InvokeCopyOrMove(baseSlotItem);
        }

        public void CreateSlotItem(UIPayload payload)
        {
            BaseItem baseItem = new BaseItem();
            baseItem.itemData = payload.itemData;
            baseItem.InitResources();

            var origin = UIManager.Instance.MakeSubItem<InventorySlotItem>(transform, InventorySlotItem.Path);
            origin.InitBaseSlotItem();
            origin.SetSlot(SlotItemPosition);
            origin.SetItemData(baseItem);
            origin.SetOnDragParent(payload.onDragParent);

            baseItem.slotItems[SlotType] = origin;
            baseItem.slots[SlotType] = this;

            // !TODO
            // 아이템을 처음 습득했을 때, 장착 가능한 슬롯이 있으면 장착
            InvokeEquipmentFrameEvent(InventoryEventType.EquipItem, payload.itemData.groupType, origin);
        }

        public void InvokeEquipmentFrameEvent(InventoryEventType eventType, GroupType groupType, BaseSlotItem baseSlotItem)
        {
            var inventoryEventPayload = new InventoryEventPayload
            {
                eventType = eventType,
                groupType = groupType,
                baseSlotItem = baseSlotItem,
            };

            slotInventoryAction?.Invoke(inventoryEventPayload);
        }

        public void ClearSlotItemPosition()
        {
            SlotItemPosition.ClearItem();
        }

        public void Subscribe(Action<InventoryEventPayload> listener)
        {
            slotInventoryAction -= listener;
            slotInventoryAction += listener;
        }

        private void OnDestroy()
        {
            slotInventoryAction = null;
        }
    }
}