
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContactGloveSDK
{
    public enum ControllerButtonType
    {
        A = 0,
        B = 1,
        System = 2,
        TriggerButton = 3,
        JoystickButton = 4,
        TrackpadTouch = 5
    }

    public enum ControllerFloatInputType
    {
        JoystickX = 0,
        JoystickY = 1,
        Trigger = 2,
        GripValue = 3, 
        GripForce = 4,
        TrackpadX = 5,
        TrackpadY = 6
    }

    public enum ButtonState
    {
        Idle,
        Touched,
        Pressed,
    }
    
    public class ControllerData
    {
        public readonly HandSides hand;
        public readonly ButtonState A, B, System, TriggerButton, JoystickButton, TrackpadTouch;
        public readonly float JoystickX, JoystickY, Trigger, GripValue, GripForce, tractpadX, trackpadY;
        public readonly bool Valid;

        public ControllerData(HandSides hand)
        {
            this.hand = hand;
            this.Valid = true;
        }
        
        private static ButtonState ConvertIntToButtonState(int value)
        {
            return value switch
            {
                0 => ButtonState.Idle,
                1 => ButtonState.Touched,
                2 => ButtonState.Pressed,
                _ => throw new AggregateException("Illegal Button State Value is received")
            };
        }
        
        public ControllerData(HandSides hand, int[] intValues, float[] floatValues)
        {
            if (intValues.Length < 6 || floatValues.Length < 7)
            {
                Debug.LogError("Controller input data length is too short");
                this.Valid = false;
                return;
            }
            this.hand = hand;
            this.A = ConvertIntToButtonState(intValues[0]);
            this.B = ConvertIntToButtonState(intValues[1]);
            this.System = ConvertIntToButtonState(intValues[2]);
            this.TriggerButton = ConvertIntToButtonState(intValues[3]);
            this.JoystickButton = ConvertIntToButtonState(intValues[4]);
            this.TrackpadTouch = ConvertIntToButtonState(intValues[5]);
            this.Trigger = floatValues[0];
            this.GripValue = floatValues[1];
            this.GripForce = floatValues[2];
            this.JoystickX = floatValues[3];
            this.JoystickY = floatValues[4];
            this.tractpadX = floatValues[5];
            this.trackpadY = floatValues[6];
            this.Valid = true;
        }
        
        private static ButtonState ConvertBoolToButtonState(bool value)
        {
            return value ? ButtonState.Pressed : ButtonState.Idle;
        }
        
        public ControllerData(HandSides hand, NamedPipe.ControllerInput controllerInput)
        {
            this.hand = hand;
            this.A = ConvertBoolToButtonState(controllerInput.a_pressed);
            this.B = ConvertBoolToButtonState(controllerInput.b_pressed);
            this.System = ConvertBoolToButtonState(controllerInput.sys_pressed);
            this.JoystickButton = ConvertBoolToButtonState(false);
            this.JoystickButton = ConvertBoolToButtonState(controllerInput.joystick_pressed);
            this.TrackpadTouch = ConvertBoolToButtonState(controllerInput.trackpad_pressed);
            this.Trigger = controllerInput.trigger;
            this.GripValue = controllerInput.grip_value;
            this.GripForce = controllerInput.grip_force;
            this.JoystickX = controllerInput.joystick_x;
            this.JoystickY = controllerInput.joystick_y;
            this.Valid = true;
        }

        public ButtonState GetControllerInput(ControllerButtonType type)
        {
            return type switch
            {
                ControllerButtonType.A => A,
                ControllerButtonType.B => B,
                ControllerButtonType.System => System,
                ControllerButtonType.TriggerButton => TriggerButton,
                ControllerButtonType.JoystickButton => JoystickButton,
                ControllerButtonType.TrackpadTouch => TrackpadTouch,
                _ => throw new NotImplementedException()
            };
        }
        
        public float GetControllerInput(ControllerFloatInputType type)
        {
            return type switch
            {
                ControllerFloatInputType.Trigger => Trigger,
                ControllerFloatInputType.GripForce => GripForce,
                ControllerFloatInputType.GripValue => GripValue,
                ControllerFloatInputType.JoystickX => JoystickX,
                ControllerFloatInputType.JoystickY => JoystickY,
                ControllerFloatInputType.TrackpadX => tractpadX,
                ControllerFloatInputType.TrackpadY => trackpadY,
                _ => throw new NotImplementedException()
            };
        }
        
        public void ShowControllerInput()
        {
            var suffix = HandSides.Left == hand ? "L" : "R";
            Debug.Log($"A_{suffix}: {A}");
            Debug.Log($"B_{suffix}: {B}");
            Debug.Log($"System_{suffix}: {System}");
            Debug.Log($"TriggerButton_{suffix}: {TriggerButton}");
            Debug.Log($"JoystickButton_{suffix}: {JoystickButton}");
            Debug.Log($"TrackpadTouch_{suffix}: {TrackpadTouch}");
            Debug.Log($"Trigger_{suffix}: {Trigger}");
            Debug.Log($"GripForce_{suffix}: {GripForce}");
            Debug.Log($"GripValue_{suffix}: {GripValue}");
            Debug.Log($"JoystickX_{suffix}: {JoystickX}");
            Debug.Log($"JoystickY_{suffix}: {JoystickY}");
        }

        public bool IsPressed(ControllerButtonType type)
        {
            return GetControllerInput(type) == ButtonState.Pressed;
        }
    }
}