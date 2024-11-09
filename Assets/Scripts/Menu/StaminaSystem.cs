using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public int currentStamina; //El encargado de la que stamina baje
    [SerializeField] private int maxStamina = 10;
    [SerializeField] private float timeToChargeStamina = 10f;

    public bool recharging;

    public bool UseStamina(int stamina)
    {
        if(currentStamina < stamina)
        return false;

        currentStamina -= stamina;

        if(!!recharging && currentStamina < maxStamina)
        {
            recharging = true;
        }
        return true;
    }

    public void RechargeStamina(int stamina)
    {
        currentStamina += stamina;
    }
}
