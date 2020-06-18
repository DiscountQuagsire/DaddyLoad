using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //HP
    public float HP;
    public float maxHP;
    private Transform HealthBar;
    //Pressure
    public float pressure = 50;
    private Transform pressureNeedle;
    private float maxPressure = 100;
    private float minPressureThreshold = 30;
    private Transform minPressureNeedle;
    private float maxPressureThreshold = 70;
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
        HP = maxHP;
        radiation = 0;
        HealthBar = transform.Find("Canvas").Find("HealthBar").Find("HealthBar Fill");
        pressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Needle");
        minPressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Min Threshold");
        maxPressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Max Threshold");
        RadiationBar = transform.Find("Canvas").Find("RadiationBar").Find("RadiationBar Fill");
        temperatureNeedle = transform.Find("Canvas").Find("TemperatureBar").Find("Temperature Needle");
        minTemperatureNeedle = transform.Find("Canvas").Find("TemperatureBar").Find("Temperature Min Threshold");
        maxTemperatureNeedle = transform.Find("Canvas").Find("TemperatureBar").Find("Temperature Max Threshold");

        updatePressure();
        updateRadiation();
        updateTemperature();
    }

    private void Update()
    {
        //set pressure
        if (Input.GetKeyDown("q"))
        {
            setPressure(pressure-10);
        }
        if (Input.GetKeyDown("e"))
        {
            setPressure(pressure + 10);
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

        if (Input.GetKeyDown("tab"))
        {
            transform.Find("Canvas").Find("RadiationBar").gameObject.SetActive(!transform.Find("Canvas").Find("RadiationBar").gameObject.activeSelf);
        }



    }

    private void FixedUpdate()
    {
        if (pressure > maxPressureThreshold^pressure<minPressureThreshold) setHP(HP-1);

        if (temperature > maxTemperatureThreshold ^ temperature < minTemperatureThreshold) setHP(HP - 1);

        //toggle radiation DPS
        if (Input.GetKey("r"))
        {
            setRadiation(radiation + 1);
        }

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
        pressureNeedle.localPosition = new Vector3(2 * pressure / maxPressure * 100 - 100, 0, 0);
        minPressureNeedle.localPosition = new Vector3(2 * minPressureThreshold / maxPressure * 100 - 100, 0, 0);
        maxPressureNeedle.localPosition = new Vector3(2 * maxPressureThreshold / maxPressure * 100 - 100, 0, 0);
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
        temperatureNeedle.localPosition = new Vector3(2 * temperature / (maxTemperature-minTemperature) * 100 - 100, 0, 0);
        minTemperatureNeedle.localPosition = new Vector3(2 * minTemperatureThreshold / (maxTemperature - minTemperature) * 100 - 100, 0, 0);
        maxTemperatureNeedle.localPosition = new Vector3(2 * maxTemperatureThreshold / (maxTemperature - minTemperature) * 100 - 100, 0, 0);
    }
}
