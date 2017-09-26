using UnityEngine;
using System;

namespace UnityFinger
{
    /// <summary>
    /// Unity Editor input
    /// </summary>
    public class EditorInput : ScreenInputBase
    {
        enum State
        {
            None,
            UI,
            Screen
        }

        State currentState = State.None;

        public override int FingerCount {
            get { return currentState == State.Screen ? 1 : 0; }
        }

        public override void Update()
        {
            if (!Input.GetMouseButton(0)) {
                currentState = State.None;
                return;
            }

            if (currentState == State.None) {
                if (!ScreenInput.IgnoreOverGameObject && EventSystem.IsPointerOverGameObject()) {
                    currentState = State.UI;
                } else {
                    currentState = State.Screen;
                }
            }
        }

        public override Vector2 GetPosition()
        {
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        public override Vector2 GetSecondPosition()
        {
            throw new InvalidOperationException("Editor Input can not execute multiple taps");
        }
    }
}

