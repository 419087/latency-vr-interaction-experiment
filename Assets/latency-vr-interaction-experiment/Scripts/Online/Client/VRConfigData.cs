using UnityEngine;

public class VRConfigData
{
    public CameraMarker CameraMarker { get; }
    public LeftControllerMarker LeftControllerMarker { get; }
    public RightControllerMarker RightControllerMarker { get; }

    public VRConfigData(
        CameraMarker cameraMarker,
        LeftControllerMarker leftControllerMarker,
        RightControllerMarker rightControllerMarker)
    {
        CameraMarker = cameraMarker;
        LeftControllerMarker = leftControllerMarker;
        RightControllerMarker = rightControllerMarker;
    }

}
