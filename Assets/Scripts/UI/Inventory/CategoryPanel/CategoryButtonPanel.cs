using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public enum GroupType
    {
        Consumption,
        Stone,
        Etc
    }

    public readonly struct ToggleChangeInfo
    {
        public GroupType Type { get; }
        public bool IsOn { get; }

        private ToggleChangeInfo(GroupType type, bool isOn)
        {
            Type = type;
            IsOn = isOn;
        }

        public static ToggleChangeInfo Of(GroupType type, bool isOn)
        {
            return new ToggleChangeInfo(type, isOn);
        }
    }

    public class CategoryButtonPanel : UIBase
    {
        private RectTransform rect;
        private ToggleGroup toggleGroup;
        private CategoryToggleController[] toggles;
        private GroupType type = GroupType.Consumption;
        private ToggleChangeHandler toggleChangeCallback;

        private readonly IDictionary<SlotAreaType, List<InventorySlotArea>> slotAreas = new Dictionary<SlotAreaType, List<InventorySlotArea>>();

        private void Awake()
        {
            Init();
        }

        private void OnDisable()
        {
            toggleChangeCallback = null;
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            rect = GetComponent<RectTransform>();
            toggleGroup = GetComponent<ToggleGroup>();
        }

        private void InitObjects()
        {
            toggleGroup.SetAllTogglesOff();

            var groupTypes = Enum.GetValues(typeof(GroupType));
            toggles = new CategoryToggleController[groupTypes.Length];
            for (int i = 0; i < groupTypes.Length; i++)
            {
                var child = rect.GetChild(i);
                if (child == null)
                    return;

                var toggle = rect.GetChild(i).gameObject.GetOrAddComponent<CategoryToggleController>();
                toggles[i] = toggle;
                toggles[i].Init((GroupType)groupTypes.GetValue(i));
                toggles[i].Subscribe(ToggleChangeCallback);
            }

            var slotTypes = Enum.GetValues(typeof(SlotAreaType));
            for (int i = 0; i < slotTypes.Length; i++)
            {
                slotAreas.TryAdd((SlotAreaType)slotTypes.GetValue(i), new List<InventorySlotArea>());
            }
        }

        private void ToggleChangeCallback(ToggleChangeInfo changeInfo)
        {
            if (changeInfo.IsOn)
            {
                type = changeInfo.Type; // 현재 활성화된 슬롯 타입
            }

            toggleChangeCallback?.Invoke(changeInfo);
        }

        public void MoveSlotArea(SlotAreaType areaType, GroupType groupType, Transform target, Transform parent, Rect size)
        {
            if (slotAreas.TryGetValue(areaType, out var slots))
            {
                slots[(int)groupType].MoveSlotArea(target, parent, size);
            }
        }

        public void AddSlot(SlotAreaType slotAreaType, InventorySlotArea area)
        {
            if (slotAreas.TryGetValue(slotAreaType, out var slots))
            {
                slots.Add(area);
            }
        }

        public void Subscribe(ToggleChangeHandler listener)
        {
            toggleChangeCallback -= listener;
            toggleChangeCallback += listener;
        }

        public void ActivateToggle(GroupType groupType, bool isOn)
        {
           toggles[(int)groupType].ActivateToggle(isOn);
        }
    }
}