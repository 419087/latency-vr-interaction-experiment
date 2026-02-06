namespace ContactGloveSDK
{
    public interface IControllerInput
    {
        public ButtonState GetControllerInput(HandSides hand, ControllerButtonType type);

        public float GetControllerInput(HandSides hand, ControllerFloatInputType type);

        public void AddOnControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler);
        
        public void AddOffControllerInputHandler(HandSides hand, ControllerButtonType type, ControllerInputHandler handler);
    }
}