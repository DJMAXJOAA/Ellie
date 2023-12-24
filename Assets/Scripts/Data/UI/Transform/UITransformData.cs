using System.Collections.Generic;
using System.Linq;
using Data.ActionData;
using UI.Framework.Presets;
using UI.Inventory;
using UnityEngine;

namespace Data.UI.Transform
{
    public class TransformController
    {
        private readonly Queue<Rect> rectQueue = new();
        private readonly Queue<Vector2> scaleQueue = new();

        public void OnRectChange(Rect rect)
        {
            rectQueue.Enqueue(rect);
        }

        public void OnScaleChange(Vector2 scale)
        {
            scaleQueue.Enqueue(scale);
        }

        public void CheckQueue(RectTransform owner)
        {
            if (rectQueue.Any())
            {
                var rect = rectQueue.Dequeue();
                AnchorPresets.SetAnchorPreset(owner, AnchorPresets.MiddleCenter);
                owner.sizeDelta = rect.GetSize();
                owner.localPosition = rect.ToCanvasPos();
            }

            if (scaleQueue.Any())
            {
                var scale = scaleQueue.Dequeue();
                owner.localScale = scale;
            }
        }
    }

    [CreateAssetMenu(fileName = "UITransform", menuName = "UI/TransformData", order = 0)]
    public class UITransformData : ScriptableObject, ISerializationCallbackReceiver
    {
        [Header("x, y, width, height")] public Rect rect;
        [Header("x, y scale")] public Vector2 scale = Vector2.one;

        public readonly Data<Rect> actionRect = new();
        public readonly Data<Vector2> actionScale = new();

        public void OnBeforeSerialize()
        {
            rect = actionRect.Value;
            scale = actionScale.Value;
        }

        public void OnAfterDeserialize()
        {
            actionRect.Value = rect;
            actionScale.Value = scale;
        }
    }
}