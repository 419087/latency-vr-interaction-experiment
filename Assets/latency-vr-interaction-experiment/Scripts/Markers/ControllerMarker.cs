using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

[RequireComponent(typeof(HapticImpulsePlayer))]
public abstract class ControllerMarker : MonoBehaviour
{
    [SerializeField] private HapticImpulsePlayer _hapticImpulsePlayer;
    public HapticImpulsePlayer HapticImpulsePlayer => _hapticImpulsePlayer;
}
