using System.Collections;
using System.Collections.Generic;
using ContactGloveSDK;
using UnityEngine;
using UnityEngine.Serialization;

#if STEAMVR_PRESENT
using Valve.VR;
using Valve.VR.InteractionSystem;

public class JoystickWalk : MonoBehaviour
{
    public float speed = 2.0f;
    public IContactGloveManager cgManager;
    public GameObject contactGloveObj;

    void Start()
    {
        cgManager = contactGloveObj.GetComponent<IContactGloveManager>();
    }

    void Update()
    {
        Vector3 playerPos = transform.position;
        
        // You can get the input of joystick.
        float x = cgManager.GetControllerInput(HandSides.Right, ControllerFloatInputType.JoystickX);
        float y = cgManager.GetControllerInput(HandSides.Right, ControllerFloatInputType.JoystickY);

        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(x, 0, y));
        playerPos.x += speed * Time.deltaTime * direction.x;
        playerPos.z += speed * Time.deltaTime * direction.z;

        transform.position = playerPos;
    }
}
#endif