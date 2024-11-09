using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public int currentStamina; //El encargado de la que stamina baje

    public float timer;

    [SerializeField] private int maxStamina = 10;
    [SerializeField] private float timeToChargeStamina = 10f;

    DateTime nextStaminaTime; 
    DateTime lastStaminaTime;

    public bool recharging;


    private void Start()
    {
        LoadStamina();
        StartCoroutine(AutoRechargeStamina());
    }

    IEnumerator AutoRechargeStamina()
    {
        recharging = true;
        timer = 0;

        while (currentStamina < maxStamina)
        {
            DateTime currentTime = DateTime.Now;
            DateTime nextTime = nextStaminaTime;

            bool staminaAdded = false;

            while(currentTime > nextTime)
            {
                if (currentStamina >= maxStamina) break;    

                currentStamina += 1;

                staminaAdded |= true;

                if(lastStaminaTime > nextTime)
                {
                    nextTime = lastStaminaTime;
                }

                nextTime = nextTime.AddSeconds(timeToChargeStamina);
            }

            if(staminaAdded)
            {
                nextStaminaTime = nextTime;
                lastStaminaTime = DateTime.Now;
            }

            SaveStamina();

            //timer += Time.deltaTime;

            //if(timer >= timeToChargeStamina)
            //{
            //    currentStamina += 1;
            //    timer = 0;
            //}

            yield return new WaitForEndOfFrame();

        }

        recharging = false;

    }

    public bool UseStamina(int stamina)
    {
        if(currentStamina < stamina)
        return false;

        currentStamina -= stamina;

        if(!recharging && currentStamina < maxStamina)
        {
            nextStaminaTime = DateTime.Now.AddSeconds(timeToChargeStamina);
            StartCoroutine(AutoRechargeStamina());
        }
        return true;
    }

    public void RechargeStamina(int stamina)
    {
        currentStamina += stamina;

        if(recharging && currentStamina >= maxStamina)
        {
            StopAllCoroutines();
            recharging = false;
        }
    }

    public void SaveStamina()
    {
        PlayerPrefs.SetInt("CurrentStamina", currentStamina);
        PlayerPrefs.SetString("NextStaminaTime", nextStaminaTime.ToString());
        PlayerPrefs.SetString("LastStaminaTime", lastStaminaTime.ToString());
        PlayerPrefs.Save();
    }
    public void LoadStamina()
    {
        currentStamina = PlayerPrefs.GetInt("CurrentStamina", maxStamina);
        nextStaminaTime = StringToDateTime(PlayerPrefs.GetString("NextStaminaTime"));
        lastStaminaTime = StringToDateTime(PlayerPrefs.GetString("LastStaminaTime"));
    }

    DateTime StringToDateTime(string date)
    {
        if (string.IsNullOrEmpty(date))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(date);
        }
    }
}
