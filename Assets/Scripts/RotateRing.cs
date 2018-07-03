using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRing : MonoBehaviour {

    public bool grabbed = false;
    public bool highlighted = false;
    public float rotSmoothSpeed = 50;
    public AudioClip rotateSFX;
    private GameObject triggerSphere;
    private Quaternion startRot;
    private Vector3 currentControllerPos;
    private Vector3 startControllerPos;
    private Quaternion rotatioWhenGrabbed;


    private void Start()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }
    private void Update()
    {
        if (Input.GetAxis("RGripTrigger") < 1)
        {
            grabbed = false;
            if (highlighted == false)
            {
                GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
                GetComponent<Renderer>().material.color = Color.blue;
                GetComponent<AudioSource>().Stop();
                startRot = transform.rotation;
            }
        }

        if (highlighted && Input.GetAxis("RGripTrigger") == 1)
        {
            if (grabbed == false)
            {
                grabbed = true;
                GetComponent<Renderer>().material.color = Color.yellow;
                startControllerPos = triggerSphere.transform.position;
            }
        }

        if (grabbed)
        {
            GetComponent<Renderer>().material.color = Color.red;
            RotateWithController(); 
            if (GetComponent<AudioSource>().isPlaying == false)
            {
                GetComponent<AudioSource>().Play();
            }
        }

        if (triggerSphere != null)
        {
            currentControllerPos = triggerSphere.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "TriggerSphere")
        //{
            triggerSphere = other.gameObject;
            GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
            GetComponent<Renderer>().material.color = Color.red;
            highlighted = true;
        //}        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TriggerSphere" && grabbed == false)
        {
            GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
            GetComponent<Renderer>().material.color = Color.blue;
        }
        highlighted = false;
    }

    private void RotateWithController()
    {
        Vector3 posDifference = startControllerPos - currentControllerPos;
        Quaternion target = Quaternion.Euler(0, startRot.y + posDifference.x * 300, 0); 
        transform.rotation = Quaternion.Slerp(transform.rotation, startRot * target, Time.deltaTime * rotSmoothSpeed);
    }
}
