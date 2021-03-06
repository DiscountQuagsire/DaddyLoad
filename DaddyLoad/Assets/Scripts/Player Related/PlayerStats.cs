﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Meter angle constants
    public const float MIN_ANGLE = 232.0f;
    public const float MAX_ANGLE = -53.0f;

    public float totalAngle;

    //HP
    public float HP;
    public float maxHP;
    private Transform HealthBar;
    //Pressure
    public float pressure = 50;
    private Transform pressureNeedle;
    private float maxPressure = 100;
    private float minPressureThreshold = 40;
    private Transform minPressureNeedle;
    private float maxPressureThreshold = 60;
    private Transform maxPressureNeedle;
    //Radiation
    public float radiation;
    private float maxRadiation = 100;
    private Transform RadiationBar;
    //Temperature
    public float temperature = 50;
    private Transform temperatureNeedle;
    private float minTemperature = 0;
    private float maxTemperature = 100;
    private float minTemperatureThreshold = 30;
    private Transform minTemperatureNeedle;
    private float maxTemperatureThreshold = 70;
    private Transform maxTemperatureNeedle;

    

    private void Start()
    {
        totalAngle = Mathf.Abs(MAX_ANGLE - MIN_ANGLE);
        HP = maxHP;
        radiation = 0;
        HealthBar = transform.Find("Canvas").Find("HealthBar").Find("HealthBar Fill");
        pressureNeedle = transform.Find("Canvas").Find("Control Panel").Find("PressureBar").Find("Pressure Needle");
        minPressureNeedle = transform.Find("Canvas").Find("Control Panel").Find("PressureBar").Find("Pressure Min Threshold");
        maxPressureNeedle = transform.Find("Canvas").Find("Control Panel").Find("PressureBar").Find("Pressure Max Threshold");
        RadiationBar = transform.Find("Canvas").Find("Control Panel").Find("RadiationBar").Find("RadiationBar Fill");
        temperatureNeedle = transform.Find("Canvas").Find("Control Panel").Find("TemperatureBar").Find("Temperature Needle");
        minTemperatureNeedle = transform.Find("Canvas").Find("Control Panel").Find("TemperatureBar").Find("Temperature Min Threshold");
        maxTemperatureNeedle = transform.Find("Canvas").Find("Control Panel").Find("TemperatureBar").Find("Temperature Max Threshold");

        updatePressure();
        updateRadiation();
        updateTemperature();


    }

    private void Update()
    {
        //set pressure
        /*
        if (Input.GetKey("q"))
        {
            setPressure(pressure-1);
        }
        if (Input.GetKey("e"))
        {
            setPressure(pressure + 1);
        }

        //set temperature
        if (Input.GetKeyDown("1"))
        {
            setTemperature(temperature - 10);
        }

        if (Input.GetKeyDown("3"))
        {
            setTemperature(temperature + 10);
        }
        */

        



    }

    private void FixedUpdate()
    {



        if (pressure > maxPressureThreshold ^ pressure < minPressureThreshold)
        {
            //any damage function
            setHP(HP - 1);
        }

        if (temperature > maxTemperatureThreshold ^ temperature < minTemperatureThreshold)
        {
            //any damage function
            setHP(HP - 1);
        }

        /*
        //toggle radiation DPS
        if (Input.GetKey("r"))
        {
            setRadiation(radiation + 1);
        }
        */

        //Temperature setting

        setTemperature(BiomeManager.getBiomeAt((int)GameObject.FindGameObjectWithTag("Player").transform.position.x).getTemperature((int)GameObject.FindGameObjectWithTag("Player").transform.position.y));
        setPressure(BiomeManager.getBiomeAt((int)GameObject.FindGameObjectWithTag("Player").transform.position.x).getPressure((int)GameObject.FindGameObjectWithTag("Player").transform.position.y));

    }





    //updates HP UI, depending on HP/maxHP, Hp can't go below 0 or above maxHP
    public void updateHP()
    {        


            float hpRatio = HP / maxHP;
            HealthBar.localScale = new Vector3(hpRatio, 1f);


    }

    public void setHP(float value)
    {
        HP = value;
        if (HP > maxHP) HP = maxHP;
        else if(HP<0) HP = 0;
        updateHP();
    }

    //pressure value setters
    public void setPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        pressure = value;
        updatePressure();
    }

    public void setMinPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        minPressureThreshold = value;
        updatePressure();
    }

    public void setMaxPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        maxPressureThreshold = value;
        updatePressure();
    }
    //Updates all pressure needle UI, called only when there is a change in pressure or thresholds
    public void updatePressure()
    {
        pressureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(pressure, maxPressure));
        //maybe put threshold update in a different function using coroutine and properties
        minPressureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(minPressureThreshold, maxPressure));
        maxPressureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(maxPressureThreshold, maxPressure));
    }

    //updates Radiation UI
    public void updateRadiation()
    {        


        float radiationRatio = radiation / maxRadiation;
        RadiationBar.localScale = new Vector3(radiationRatio, 1f);


    }

    public void setRadiation(float value)
    {
        radiation = value;
        if (radiation > maxRadiation) radiation = maxRadiation;
        else if (radiation<0) radiation = 0;

        updateRadiation();
    }

    //temperature value setters
    public void setTemperature(float value)
    {
        if (value < minTemperature) value = 0;
        else if (value > maxTemperature) value = maxTemperature;
        temperature = value;
        updateTemperature();
    }

    public void setMinTemperature(float value)
    {
        if (value < minTemperature) value = 0;
        else if (value > maxTemperature) value = maxTemperature;
        minTemperatureThreshold = value;
        updateTemperature();
    }

    public void setmaxTemperature(float value)
    {
        if (value < minTemperature) value = 0;
        else if (value > maxTemperature) value = maxTemperature;
        maxTemperatureThreshold = value;
        updateTemperature();
    }
    //updates all temperature UI
    public void updateTemperature()
    {
        temperatureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(temperature, maxTemperature));
        //maybe put threshold update in a different function using coroutine and properties
        minTemperatureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(minTemperatureThreshold, maxTemperature));
        maxTemperatureNeedle.eulerAngles = new Vector3(0, 0, getNeedleRotation(maxTemperatureThreshold, maxTemperature));
    }

    public float getNeedleRotation(float value, float maxValue)
    {
        float normalizedValue = value / maxValue;
        return MIN_ANGLE - normalizedValue * totalAngle;
    }
        
        
}
