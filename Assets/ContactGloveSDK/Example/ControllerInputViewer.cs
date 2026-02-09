using UnityEngine;
using UnityEngine.UI;
using ContactGloveSDK;

public class ControllerInputViewer : MonoBehaviour
{
    [SerializeField] private GameObject contactGloveObj;
    [SerializeField] private HandSides handSide = HandSides.Left;

    [Header("Button Images")]
    [SerializeField] private Image buttonA;
    [SerializeField] private Image buttonB;
    [SerializeField] private Image buttonSystem;
    [SerializeField] private Image triggerButton;
    [SerializeField] private Image joystickButton;
    [SerializeField] private Image trackpadTouch;

    [Header("Button Colors")]
    [SerializeField] private Color pressedColor = Color.green;
    [SerializeField] private Color releasedColor = Color.gray;

    [Header("Joystick")]
    [SerializeField] private RectTransform joystickHandle;
    [SerializeField] private float joystickRadius = 50f;

    [Header("Trackpad")]
    [SerializeField] private RectTransform trackpadHandle;
    [SerializeField] private float trackpadRadius = 50f;

    [Header("Float Sliders")]
    [SerializeField] private Slider trigger;
    [SerializeField] private Slider gripValue;
    [SerializeField] private Slider gripForce;

    private IContactGloveManager cgManager;

    private void Awake()
    {
        ResolveManager();
    }

    private void OnValidate()
    {
        if (contactGloveObj != null)
        {
            ResolveManager();
        }
    }

    private void Update()
    {
        if (cgManager == null)
        {
            ResolveManager();
            if (cgManager == null) return;
        }
        UpdateButtonColor(buttonA, ControllerButtonType.A);
        UpdateButtonColor(buttonB, ControllerButtonType.B);
        UpdateButtonColor(buttonSystem, ControllerButtonType.System);
        UpdateButtonColor(triggerButton, ControllerButtonType.TriggerButton);
        UpdateButtonColor(joystickButton, ControllerButtonType.JoystickButton);
        UpdateButtonColor(trackpadTouch, ControllerButtonType.TrackpadTouch);

        UpdateJoystickPosition();
        UpdateTrackpadPosition();

        UpdateSliderValue(trigger, ControllerFloatInputType.Trigger);
        UpdateSliderValue(gripValue, ControllerFloatInputType.GripValue);
        UpdateSliderValue(gripForce, ControllerFloatInputType.GripForce);
    }

    private void ResolveManager()
    {
        if (contactGloveObj == null)
        {
            cgManager = null;
            return;
        }

        cgManager = contactGloveObj.GetComponent<IContactGloveManager>();
    }

    private void UpdateButtonColor(Image image, ControllerButtonType type)
    {
        if (image == null || cgManager == null) return;
        var state = cgManager.GetControllerInput(handSide, type);
        image.color = state == ButtonState.Pressed ? pressedColor : releasedColor;
    }

    private void UpdateJoystickPosition()
    {
        if (joystickHandle == null || cgManager == null) return;

        float x = cgManager.GetControllerInput(handSide, ControllerFloatInputType.JoystickX);
        float y = cgManager.GetControllerInput(handSide, ControllerFloatInputType.JoystickY);
        joystickHandle.anchoredPosition = new Vector2(x, y) * joystickRadius;
    }

    private void UpdateTrackpadPosition()
    {
        if (trackpadHandle == null || cgManager == null) return;

        float x = cgManager.GetControllerInput(handSide, ControllerFloatInputType.TrackpadX);
        float y = cgManager.GetControllerInput(handSide, ControllerFloatInputType.TrackpadY);
        trackpadHandle.anchoredPosition = new Vector2(x, y) * trackpadRadius;
    }

    private void UpdateSliderValue(Slider slider, ControllerFloatInputType type)
    {
        if (slider == null || cgManager == null) return;
        slider.value = cgManager.GetControllerInput(handSide, type);
    }
}
