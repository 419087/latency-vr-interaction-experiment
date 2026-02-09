using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFingerValueSet : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Slider slider;
    
    public void SetTitle(string title)
    {
        this.title.text = title;
    }
    
    public void SetSliderValue(float value)
    {
        slider.value = value;
    }
}
