using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipDetails : MonoBehaviour {

    [Header("Parts")]
    public GameObject wing;
    public GameObject nose;
    public GameObject engine;
    public GameObject fuselage;

    [Header("Stats")]
    public float speed;
    public float armor;
    public float acceleration;
    public float handling;

    public Text speedDisplay;
    public Text armorDisplay;
    public Text accelDisplay;
    public Text handlingDisplay;
    
    public void RefreshStats()
    {
        speed = 0;
        armor = 0;
        acceleration = 0;
        handling = 0;

        if (wing != null)
        {
            speed += wing.GetComponent<DroneComponent>().speed;
            armor += wing.GetComponent<DroneComponent>().armor;
            acceleration += wing.GetComponent<DroneComponent>().acceleration;
            handling += wing.GetComponent<DroneComponent>().handling;
        }

        if (fuselage != null)
        {
            speed += fuselage.GetComponent<DroneComponent>().speed;
            armor += fuselage.GetComponent<DroneComponent>().armor;
            acceleration += fuselage.GetComponent<DroneComponent>().acceleration;
            handling += fuselage.GetComponent<DroneComponent>().handling;
        }

        if (engine != null)
        {
            speed += engine.GetComponent<DroneComponent>().speed;
            armor += engine.GetComponent<DroneComponent>().armor;
            acceleration += engine.GetComponent<DroneComponent>().acceleration;
            handling += engine.GetComponent<DroneComponent>().handling;
        }

        if (nose != null)
        {
            speed += nose.GetComponent<DroneComponent>().speed;
            armor += nose.GetComponent<DroneComponent>().armor;
            acceleration += nose.GetComponent<DroneComponent>().acceleration;
            handling += nose.GetComponent<DroneComponent>().handling;
        }

        speedDisplay.text = "SPEED: " + speed.ToString();
        armorDisplay.text = "ARMOR: " + armor.ToString();
        accelDisplay.text = "ACCELERATION: " + acceleration.ToString();
        handlingDisplay.text = "HANDLING: " + handling.ToString();
    }

    public bool FullyEquipped()
    {
        if (wing == null)
        {
            return false;
        } else if (engine == null)
        {
            return false;
        } else if (fuselage == null)
        {
            return false;
        } else if (nose == false)
        {
            return false;
        }
        return true;
    }
}
