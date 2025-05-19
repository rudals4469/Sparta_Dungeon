using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    Condition health
    {
        get { return uiCondition.health; }
    }
    Condition hunger
    {
        get { return uiCondition.hunger; }
    }

    Condition stamina
    {
        get { return uiCondition.stamina; }
    }

    public float noHungerHealthDecay;

    private void Update()
    {
        hunger.Add(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("플레이어 사망");
    }
}
