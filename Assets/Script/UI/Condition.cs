using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiaBar;

    public TextMeshProUGUI valueText;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        if (curValue < maxValue)
        {
            curValue += passiveValue * Time.deltaTime;
            curValue = Mathf.Clamp(curValue, 0, maxValue);
        }
        
        uiaBar.fillAmount = GetPercentage();
        valueText.text = $"{Mathf.FloorToInt(curValue)} / {Mathf.FloorToInt(maxValue)}";
    }

    float GetPercentage()
    {
        return maxValue == 0 ? 0 : curValue / maxValue;
    }

    public void Add(float value)
    {
        curValue += value;
        curValue = Mathf.Clamp(curValue, 0, maxValue);
    }

    public void Subtract(float value)
    {
        curValue -= value;
        curValue = Mathf.Clamp(curValue, 0, maxValue);
    }
}
