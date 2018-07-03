using System.Collections;
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
    public float firingRate;
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

    private bool droneAvailable;
    private Vector3 currentAngle;
    private bool inFlight = false;
    private Vector3 startPos;
    private Quaternion startRot;

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
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                InvokeRepeating("Firing", 0.0001f, firingRate);
            }
            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                CancelInvoke("Firing");
            }

            } else
        {
            if (Input.GetButtonDown(buttonName: "Submit"))
            {
                SetDroneActive(!droneAvailable);
                AudioSource.PlayClipAtPoint(toggleDroneSFX, transform.position);
            }
            if ( OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && droneAvailable)
            {
                LaunchDrone();
            }
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
