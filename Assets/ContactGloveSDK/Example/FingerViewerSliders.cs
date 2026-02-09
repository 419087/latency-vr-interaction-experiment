using UnityEngine;
using ContactGloveSDK;

public class FingerViewerSliders : MonoBehaviour
{
    [SerializeField] private GameObject contactGloveObj;
    [SerializeField] private HandSides handSide = HandSides.Left;

    // If enabled, logs values each frame (can be noisy)
    [SerializeField] private bool showFingerValue = false;

    // Prepare references to sliders in the scene.
    // If a slider is assigned, its value will be displayed; if not, it will be ignored.
    [Header("Finger Flex (Rotation Amplitude)")]
    [SerializeField] private SliderFingerValueSet littleProximal;
    [SerializeField] private SliderFingerValueSet littleIntermediate;
    [SerializeField] private SliderFingerValueSet littleDistal;
    [SerializeField] private SliderFingerValueSet ringProximal;
    [SerializeField] private SliderFingerValueSet ringIntermediate;
    [SerializeField] private SliderFingerValueSet ringDistal;
    [SerializeField] private SliderFingerValueSet middleProximal;
    [SerializeField] private SliderFingerValueSet middleIntermediate;
    [SerializeField] private SliderFingerValueSet middleDistal;
    [SerializeField] private SliderFingerValueSet indexProximal;
    [SerializeField] private SliderFingerValueSet indexIntermediate;
    [SerializeField] private SliderFingerValueSet indexDistal;
    [SerializeField] private SliderFingerValueSet thumbProximal;
    [SerializeField] private SliderFingerValueSet thumbIntermediate;
    [SerializeField] private SliderFingerValueSet thumbDistal;

    [Header("Finger Splay")]
    [SerializeField] private SliderFingerValueSet littleSplay;
    [SerializeField] private SliderFingerValueSet ringSplay;
    [SerializeField] private SliderFingerValueSet middleSplay;
    [SerializeField] private SliderFingerValueSet indexSplay;
    [SerializeField] private SliderFingerValueSet thumbSplay;

    private IContactGloveManager cgManager;

    private void Awake()
    {
        ResolveManager();
    }

    private void OnValidate()
    {
        // Keep it resilient when editing in Inspector
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

        string debugText = "";
        UpdateSliderIfPresent(littleProximal, FingerRotationAmplitude_e.LittleProximal, ref debugText);
        UpdateSliderIfPresent(littleIntermediate, FingerRotationAmplitude_e.LittleIntermediate, ref debugText);
        UpdateSliderIfPresent(littleDistal, FingerRotationAmplitude_e.LittleDistal, ref debugText);
        UpdateSliderIfPresent(ringProximal, FingerRotationAmplitude_e.RingProximal, ref debugText);
        UpdateSliderIfPresent(ringIntermediate, FingerRotationAmplitude_e.RingIntermediate, ref debugText);
        UpdateSliderIfPresent(ringDistal, FingerRotationAmplitude_e.RingDistal, ref debugText);
        UpdateSliderIfPresent(middleProximal, FingerRotationAmplitude_e.MiddleProximal, ref debugText);
        UpdateSliderIfPresent(middleIntermediate, FingerRotationAmplitude_e.MiddleIntermediate, ref debugText);
        UpdateSliderIfPresent(middleDistal, FingerRotationAmplitude_e.MiddleDistal, ref debugText);
        UpdateSliderIfPresent(indexProximal, FingerRotationAmplitude_e.IndexProximal, ref debugText);
        UpdateSliderIfPresent(indexIntermediate, FingerRotationAmplitude_e.IndexIntermediate, ref debugText);
        UpdateSliderIfPresent(indexDistal, FingerRotationAmplitude_e.IndexDistal, ref debugText);
        UpdateSliderIfPresent(thumbProximal, FingerRotationAmplitude_e.ThumbProximal, ref debugText);
        UpdateSliderIfPresent(thumbIntermediate, FingerRotationAmplitude_e.ThumbIntermediate, ref debugText);
        UpdateSliderIfPresent(thumbDistal, FingerRotationAmplitude_e.ThumbDistal, ref debugText);
        UpdateSliderIfPresent(littleSplay, FingerRotationAmplitude_e.LittleSplay, ref debugText);
        UpdateSliderIfPresent(ringSplay, FingerRotationAmplitude_e.RingSplay, ref debugText);
        UpdateSliderIfPresent(middleSplay, FingerRotationAmplitude_e.MiddleSplay, ref debugText);
        UpdateSliderIfPresent(indexSplay, FingerRotationAmplitude_e.IndexSplay, ref debugText);
        UpdateSliderIfPresent(thumbSplay, FingerRotationAmplitude_e.ThumbSplay, ref debugText);

        if (showFingerValue && debugText.Length > 0)
        {
            Debug.Log($"{{\"hand\":\"{handSide}\",\"values\":{{{debugText}}}}}");
        }
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

    private void UpdateSliderIfPresent(SliderFingerValueSet slider, FingerRotationAmplitude_e bone, ref string debugText)
    {
        // Only query values when needed (slider assigned or debug enabled)
        if (slider == null && !showFingerValue) return;

        float value = cgManager.GetFingerRotationAmplitude(handSide, bone);
        if (slider != null)
        {
            slider.SetSliderValue(value);
        }

        if (showFingerValue)
        {
            if (debugText.Length > 0)
                debugText += ",";

            debugText += "\"" + bone + "\":" + value;
        }
    }
}
