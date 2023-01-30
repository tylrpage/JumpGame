using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Jump jump;
    [SerializeField] private Slider slider;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient fillGradient;
    [SerializeField] private Gradient backgroundGradient;

    private void Update()
    {
        if (jump == null)
        {
            slider.gameObject.SetActive(false);
            return;
        }

        if (slider.gameObject.activeSelf == false)
        {
            slider.gameObject.SetActive(true);
        }

        float t = jump.ChargeAmountNorm;
        slider.value = t;
        // Color
        fillImage.color = fillGradient.Evaluate(t);
        backgroundImage.color = backgroundGradient.Evaluate(t);
    }
}
