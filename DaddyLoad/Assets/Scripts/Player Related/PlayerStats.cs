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
    private float maxRadiation;
    //Temperature
    public float temperature;
    private float minTemperature;
    private float maxTemperature;
    private float minTemperatureThreshold;
    private float maxTemperatureThreshold;

    private void Start()
    {
        HP = maxHP;
        HealthBar = transform.Find("Canvas").Find("HealthBar").Find("HealthBar Fill");
        pressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Needle");
        minPressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Min Threshold");
        maxPressureNeedle = transform.Find("Canvas").Find("PressureBar").Find("Pressure Max Threshold");
    }

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            setPressure(pressure-10);
        }
        if (Input.GetKeyDown("e"))
        {
            setPressure(pressure + 10);
        }
        updateHP();
        updatePressure();

    }

    private void FixedUpdate()
    {
        if (pressure > maxPressureThreshold) takeDamage(1);
        if (pressure < minPressureThreshold) takeDamage(1);
    }





    //updates HP UI, depending on HP/maxHP, Hp can't go below 0 or above maxHP
    public void updateHP()
    {
        if (HP >= 0)
        {
            float hpRatio = HP / maxHP;
            HealthBar.localScale = new Vector3(hpRatio, 1f);
        }
        else if (HP > maxHP) HP = maxHP;
        else HP = 0;
    }

    public void takeDamage(float damage)
    {
        HP -= damage;
    }

    //pressure value setters
    public void setPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        pressure = value;
    }

    public void setMinPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        minPressureThreshold = value;
    }

    public void setMaxPressure(float value)
    {
        if (value < 0) value = 0;
        else if (value > maxPressure) value = maxPressure;
        maxPressureThreshold = value;
    }
    //Updates all pressure needle UI
    public void updatePressure()
    {
        pressureNeedle.localPosition = new Vector3(2 * pressure / maxPressure * 100 - 100, 0, 0);
        minPressureNeedle.localPosition = new Vector3(2 * minPressureThreshold / maxPressure * 100 - 100, 0, 0);
        maxPressureNeedle.localPosition = new Vector3(2 * maxPressureThreshold / maxPressure * 100 - 100, 0, 0);
    }
}
