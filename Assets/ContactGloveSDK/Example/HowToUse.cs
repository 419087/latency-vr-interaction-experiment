using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ContactGloveSDK;

public class HowToUse : MonoBehaviour
{
    [SerializeField] private GameObject contactGloveObj;

    // You can communicate with the glove by using IContactGloveManager
    private IContactGloveManager cgManager;
    void Start()
    {
        this.cgManager = contactGloveObj.GetComponent<IContactGloveManager>();
        
        // You can set the action invoked when A button is on pressed.
        this.cgManager.AddOnControllerInputHandler(HandSides.Left, ControllerButtonType.A, () =>
        {
            cgManager.SetVibration(HandSides.Left, 0.5f, 160, 0.1f);
        });
        
        this.cgManager.AddOnControllerInputHandler(HandSides.Left, ControllerButtonType.JoystickButton, () =>
        {
            cgManager.SetVibration(HandSides.Right, 0.5f, 160, 0.1f);
        });
    }
    
    void Update()
    {
        // You can set the vibration of the glove by using SetVibration
        // With the current hardware, the frequency is limited to 160Hz
        if (Input.GetKeyDown(KeyCode.F))
        {
            cgManager.SetVibration(HandSides.Left, 0.2f, 160.0f, 0.1f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            cgManager.SetVibration(HandSides.Right, 0.2f, 160.0f, 0.1f);
        }
    }
}
