using System.Linq;
using System;
using UnityEngine;

namespace ContactGloveSDK
{
    public delegate void ControllerInputHandler();
    
    internal class ControllerInput : IControllerInput
    {

        private static int GetEnumSize(Type t)
        {
            return Enum.GetNames(t).Length;
        }

        private ControllerInputHandler[,] OnControllerInputHandler
            = new ControllerInputHandler[GetEnumSize(typeof(HandSides)), GetEnumSize(typeof(ControllerButtonType))];
        
        private ControllerInputHandler[,] OffControllerInputHandler
            = new ControllerInputHandler[GetEnumSize(typeof(HandSides)), GetEnumSize(typeof(ControllerButtonType))];

        public ControllerInput()
        {
            left = new ControllerData(HandSides.Left);
            right = new ControllerData(HandSides.Right);
        }

        private ControllerData left, right;

        public void StoreControllerInput(ControllerData nowData)
        {
            if (!nowData.Valid)
            {
                return;
            }

            var hand = nowData.hand;
            var preData = GetControllerData(hand);
            UpdateControllerData(hand, nowData);

            foreach (ControllerButtonType type in Enum.GetValues(typeof(ControllerButtonType)))
            {
                if (nowData.IsPressed(type) && !preData.IsPressed(type))
                {
                    OnControllerInputHandler[(int)nowData.hand, (int)type]?.Invoke();
                }

                if (!nowData.IsPressed(type) && preData.IsPressed(type))
                {
                    OffControllerInputHandler[(int)nowData.hand, (int)type]?.Invoke();
                }
            }
        }

        public void ShowControllerInput()
        {
            left.ShowControllerInput();
            right.ShowControllerInput();
        }

        private ControllerData GetControllerData(HandSides hand)
        {
            return hand == HandSides.Left ? left : right;
        }

        private void UpdateControllerData(HandSides hand, ControllerData data)
        {
            if (hand == HandSides.Left)
            {
                left = data;
            }
            else
            {
                right = data;
            }
        }

        public ButtonState GetControllerInput(HandSides hand, ControllerButtonType type)
        {
            return GetControllerData(hand).GetControllerInput(type);
        }

        public float GetControllerInput(HandSides hand, ControllerFloatInputType type)
        {
            return GetControllerData(hand).GetControllerInput(type);
        }

        public void AddOnControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler)
        {
            OnControllerInputHandler[(int)hand, (int)type] += handler;
        }

        public void AddOffControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler)
        {
            OffControllerInputHandler[(int)hand, (int)type] += handler;
        }
    };
}