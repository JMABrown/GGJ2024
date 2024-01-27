using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceCanvas : MonoBehaviour
{
    [SerializeField]
    private Slider Slider;
    
    void Update()
    {
        Slider.value = AudienceManager.Instance.LaughAsFraction;
    }
}
