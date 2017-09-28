using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityFinger
{
    public class MobileInput : ScreenInputBase
    {
        List<int> removeFingerIds = new List<int>();
        List<int> screenFingerIds = new List<int>();
        List<int> uiFingerIds = new List<int>();

        public override int FingerCount {
            get { return screenFingerIds.Count; }
        }

        public override void Update()
        {
            removeFingerIds.Clear();
            foreach (var fingerId in uiFingerIds) {
                if (!ContainsInArray(Input.touches, fingerId)) {
                    removeFingerIds.Add(fingerId);
                }
            }
            foreach (var fingerId in screenFingerIds) {
                if (!ContainsInArray(Input.touches, fingerId)) {
                    removeFingerIds.Add(fingerId);
                }
            }

            foreach (var fingerId in removeFingerIds) {
                uiFingerIds.Remove(fingerId);
                screenFingerIds.Remove(fingerId);
            }

            foreach (var touch in Input.touches) {
                // If the finger is still not managed?
                if (!uiFingerIds.Contains(touch.fingerId) && !screenFingerIds.Contains(touch.fingerId)) {
                    if (!ScreenInput.IgnoreOverGameObject && EventSystem.IsPointerOverGameObject(touch.fingerId)) {
                        uiFingerIds.Add(touch.fingerId);
                    } else {
                        screenFingerIds.Add(touch.fingerId);
                    }
                }
            }
        }

        bool ContainsInArray(Touch[] touches, int element)
        {
            foreach (var touch in touches) {
                if (touch.fingerId == element) {
                    return true;
                }
            }
            return false;
        }

        public override Vector2 GetPosition()
        {
            return GetPositionByIndex(0);
        }

        public override Vector2 GetSecondPosition()
        {
            return GetPositionByIndex(1);
        }

        Vector2 GetPositionByIndex(int index)
        {
            if (screenFingerIds.Count <= index) {
                throw new System.InvalidOperationException(string.Format("There is no {0}'s finger on screen", index));
            }

            var fingerId = screenFingerIds[index];
            Touch? touch = null;
            foreach (var t in Input.touches) {
                if (t.fingerId == fingerId) {
                    touch = t;
                    break;
                }
            }

            if (!touch.HasValue) {
                throw new System.ArgumentOutOfRangeException(string.Format("Finger id {0} is not in touches", fingerId));
            }

            return new Vector2(touch.Value.position.x, touch.Value.position.y);
        }

    }
}

