using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

    public GameObject grabbable;
    public LayerMask grabMask;
    public AudioClip grabSFX;
    public AudioClip grabFailSFX;
    public AudioClip dropSFX;
    
    private GameObject grabbedObject;
    private Transform componentBase;
    private bool grabbing = false;
    private bool rightTouch;

    private void Start()
    {
        if (transform.parent.GetComponent<TouchController>().controller == OVRInput.Controller.RTouch)
        {
            rightTouch = true;
        } else
        {
            rightTouch = false;
        }
    }

    // Update is called once per frame
    void Update () {

        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (grabbing)
        {                
            if (rightTouch)
            {
                if (Input.GetAxis("RGripTrigger") < 1)
                {
                     DropObject();
                }
            }
            else
            {
                if (Input.GetAxis("LGripTrigger") < 1)
                {
                    DropObject();
                }
            }
        }
        if (!grabbing)
        {              
            if (rightTouch) //check to see if this is the right or left controller
            {
                if (Input.GetAxis("RGripTrigger") == 1)
                {
                    GrabObject();
                }                
            } else
            {
                if (Input.GetAxis("LGripTrigger") == 1)
                {
                    GrabObject();
                }
            }
        }
        if (grabbing && grabbedObject != null)
        {
            RotateGrabbedObjectWithThumbstick();
        }
	}

    private void GrabObject()
    {
        grabbing = true;        
        grabbedObject = grabbable;
        if (grabbable != null)
        {
            AudioSource.PlayClipAtPoint(grabSFX, transform.position);
            grabbedObject = grabbable;
            //grabbedObject.transform.position = transform.position;
            if (grabbedObject.tag == "Wing" || grabbedObject.tag == "Nose" || grabbedObject.tag == "Fuselage" || grabbedObject.tag == "Engine")
            {
                componentBase = grabbedObject.transform.parent;
                DroneComponent droneComponent = grabbedObject.GetComponent<DroneComponent>();
                droneComponent.onStand = false;
                
            }
            grabbedObject.transform.parent = transform;
        }
        else
        {
            AudioSource.PlayClipAtPoint(grabFailSFX, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15)
        {
        if (grabbable != null)
        {
            grabbable.GetComponent<HighlightObject>().DeSelect(); 
        }
        grabbable = other.gameObject;
        HighlightObject highlightComponent = grabbable.GetComponent<HighlightObject>();
        if (highlightComponent != null)
        {
            highlightComponent.Select();
        }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 15)
        {            
            HighlightObject highlightComponent = other.GetComponent<HighlightObject>();
            if (highlightComponent != null)
            {
               highlightComponent.DeSelect();
            }
            grabbable = null;
        }
    }

    private void DropObject()
    {
        grabbing = false;
        if (grabbedObject !=null)
        {
            AudioSource.PlayClipAtPoint(dropSFX, transform.position);
            if (grabbedObject.tag == "Wing" || grabbedObject.tag == "Nose" || grabbedObject.tag == "Fuselage" || grabbedObject.tag == "Engine")
            {
                grabbedObject.transform.parent = componentBase;
                DroneComponent droneComponent = grabbedObject.GetComponent<DroneComponent>();
                grabbedObject.transform.position = droneComponent.defaultPos;
                grabbedObject.transform.rotation = droneComponent.defaultRot;
                droneComponent.onStand = true;
            }
            else
            {
                grabbedObject.transform.parent = null;
            }
        }
    }

    private void RotateGrabbedObjectWithThumbstick()
    {
        float zRotSpeed = Input.GetAxis("Horizontal");
        float xRotSpeed = Input.GetAxis("Vertical");

        transform.Rotate(new Vector3(xRotSpeed, 0, 0));
        transform.Rotate(new Vector3(0, 0, zRotSpeed));
    }

    Transform GetClosestGrabbable(List<Transform> grabbable)
    {
        Transform closestGrabbable = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in grabbable)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestGrabbable = potentialTarget;
            }
        }

        return closestGrabbable;
    }

}
