using UnityEngine;
using ContactGloveSDK;

public class VRConfigData
{
    public CameraMarker CameraMarker { get; }
    public LeftControllerMarker LeftControllerMarker { get; }
    public RightControllerMarker RightControllerMarker { get; }
    public ContactGloveManager ContactGloveManager { get; }

    public VRConfigData(
        CameraMarker cameraMarker,
        LeftControllerMarker leftControllerMarker,
        RightControllerMarker rightControllerMarker,
        ContactGloveManager contactGloveManager)
    {
        CameraMarker = cameraMarker;
        LeftControllerMarker = leftControllerMarker;
        RightControllerMarker = rightControllerMarker;
        ContactGloveManager = contactGloveManager;
    }

}
