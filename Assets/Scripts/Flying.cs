﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour {

    public OVRInput.Controller controller;
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float accelSpeed;
    public float turnSpeed;
    public float maxRotation;
    public float tiltAngle = 90;
    public float smooth = 1.5f; // speed at which the drone re-orients itself
    public float firingRate = 80;
    public Transform launchParentTransform;
    public enum FlightControlMode {MotionControls, Thumbstick };
    public FlightControlMode controlMode;
    public AudioClip respawnSFX;
    public AudioClip toggleDroneSFX;
    public GameObject laser;
    public AudioClip laserSFX;
    public AudioClip launchSFX;
    public GameObject engine;
    public GameObject fuselage;
    public GameObject nose;
    public GameObject wing;
    public ShipDetails shipDetails;
    public AudioClip errorSFX;

    private bool droneAvailable;
    private Vector3 currentAngle;
    private bool inFlight = false;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool fireTriggerReleased = true;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        SetDroneActive(false);
    }

    // Update is called once per frame
    void Update () {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (inFlight && Input.GetButtonDown(buttonName: "Submit"))
        {
            ReturnDrone();
        } else
        if (inFlight)
        {
            switch (controlMode)
            {
                case FlightControlMode.MotionControls: RotateWithController();
                    break;
                case FlightControlMode.Thumbstick: RotateWithThumsticks();
                    break;
            }
            Accelerate();            
            GetComponent<AudioSource>().pitch = speed; 
            transform.position += transform.forward * Time.deltaTime * speed;
            if (Input.GetAxis("FireTrigger") == 1 && fireTriggerReleased == false)
            {                
                Firing();
                fireTriggerReleased = true;
            }
            if (Input.GetAxis("FireTrigger") < 1)
            {
                fireTriggerReleased = false;
                CancelInvoke("Firing");
            }

            } else
        {
            if (Input.GetButtonDown(buttonName: "Submit"))
            {
                if (shipDetails.FullyEquipped())
                {
                    CreateDrone();
                    SetDroneActive(!droneAvailable);                    
                    AudioSource.PlayClipAtPoint(toggleDroneSFX, transform.position);
                } else
                {
                    AudioSource.PlayClipAtPoint(errorSFX,transform.position);
                }
            }
            if ( OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && droneAvailable)
            {
                LaunchDrone();
            }
        }
    }

    private void CreateDrone()
    {
        Destroy(fuselage);
        fuselage = (GameObject)Instantiate(shipDetails.fuselage, transform);
        SocketPositions socketPositions = fuselage.GetComponent<SocketPositions>();

        Destroy(engine);
        engine = (GameObject) Instantiate(shipDetails.engine, transform.position, transform.rotation, transform);
        engine.transform.localPosition = socketPositions.enginePosition;
        
        Destroy(wing);
        wing = (GameObject)Instantiate(shipDetails.wing, transform.position, transform.rotation, transform);
        wing.transform.localPosition = socketPositions.wingPosition;

        Destroy(nose);
        nose = (GameObject)Instantiate(shipDetails.nose, transform.position, transform.rotation, transform);
        nose.transform.localPosition = socketPositions.NosePosition;

        //TODO fix occlusion rendering on spawned drone
        for (int i = 0; i < engine.GetComponent<Renderer>().materials.Length; i++)
        {
            engine.GetComponent<Renderer>().materials[i].shader = Shader.Find("BumpedOutline");
        }
    }

    private void SetDroneActive(bool active)
    {
        droneAvailable = active;
        engine.SetActive(active);
        nose.SetActive(active);
        fuselage.SetActive(active);
        wing.SetActive(active);
    }

    private void LaunchDrone()
    {
        inFlight = true;
        AudioSource.PlayClipAtPoint(launchSFX, transform.position);
        GetComponent<TrailRenderer>().enabled = true;
        transform.parent = null;
        GetComponent<AudioSource>().Play();
    }

    private void ReturnDrone()
    {
        inFlight = false;
        GetComponent<TrailRenderer>().enabled = false;
        transform.parent = launchParentTransform;
        transform.localPosition = startPos;
        transform.localRotation = startRot;
        AudioSource.PlayClipAtPoint(respawnSFX, transform.position);
        GetComponent<AudioSource>().Stop();
    }

    private void Accelerate()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (speed < maxSpeed)
            {
                speed += accelSpeed * Time.deltaTime;
            }
        } else
        {
            if (speed > minSpeed)
            {
                speed -= 1 * Time.deltaTime;
            }
        }
    }

    private void Firing()
    {        
            var bullet = (GameObject)Instantiate(laser, transform.position,  transform.rotation);
            AudioSource.PlayClipAtPoint(laserSFX, transform.position);
            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);
    }

    private void RotateWithController()
    {        
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(controller);
        controllerRotation.x = Mathf.Clamp(controllerRotation.x, -maxRotation, maxRotation);
        controllerRotation.y = Mathf.Clamp(controllerRotation.y, -maxRotation, maxRotation);
        controllerRotation.z = Mathf.Clamp(controllerRotation.z, -maxRotation, maxRotation);
        transform.Rotate(new Vector3(-controllerRotation.x * turnSpeed, -controllerRotation.y * turnSpeed ));
        transform.Rotate(new Vector3(0, 0, -controllerRotation.z * turnSpeed ));
    }

    private void RotateWithThumsticks()
    {
        // Roll
        float tiltAroundZ = Input.GetAxis("Horizontal");
        // Pitch
        float tiltAroundX = Input.GetAxis("Vertical");
        // Yaw
        float tiltAroundY = Input.GetAxis("Yaw");
        
        transform.Rotate(new Vector3(-tiltAroundX * turnSpeed, 0, 0)); 
        transform.Rotate(new Vector3(0, 0, -tiltAroundZ * turnSpeed));
        transform.Rotate(new Vector3(0, -tiltAroundY * turnSpeed, 0 ));
        
        // original code: Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        Quaternion target = Quaternion.Euler(0, transform.localEulerAngles.y, 0);

        if (tiltAroundX == 0 && tiltAroundZ == 0)
        {
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        }
    }
}
