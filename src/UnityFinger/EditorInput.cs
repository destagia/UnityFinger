using UnityEngine;
using System;

namespace UnityFinger
{
    /// <summary>
    /// Unity Editor input
    /// </summary>
    public class EditorInput : ScreenInputBase
    {
        private enum State
        {
            None,
            UI,
            Screen
        }

        private State currentState = State.None;

        public override int FingerCount {
            get { return currentState == State.Screen ? 1 : 0; }
        }

        public override void Update()
        {
            if (Input.GetMouseButton(0)) {
                if (currentState == State.None) {
                    if (!ScreenInput.IgnoreOverGameObject && EventSystem.IsPointerOverGameObject()) {
                        currentState = State.UI;
                    } else {
                        currentState = State.Screen;
                    }
                }
            } else {
                currentState = State.None;
            }

        }

        public override Vector2 GetPosition()
        {
            return new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        }

        public override Vector2 GetSecondPosition()
        {
            throw new InvalidOperationException("Editor Input can not execute multi taps");
        }
    }
}

