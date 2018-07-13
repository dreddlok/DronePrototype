using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayButton : MonoBehaviour {
    
    public GameObject buttonHighlight;
    public bool selected;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public AudioClip failureSFX;
    public Material highlightMaterial;
    public Vector3 offset;

    private Material ogMaterial;
    private Renderer meshRenderer;
    private bool triggerPressed;
    private ShipDetails shipDetails;
    private DisplayModel displayModel;
    private Transform ring;

    // Use this for initialization
    void Start () {
        shipDetails = FindObjectOfType<ShipDetails>();
        displayModel = transform.parent.GetComponent<DisplayModel>();
        meshRenderer = GetComponent<Renderer>();
        ogMaterial = meshRenderer.material;
        ring = transform.parent.Find("Ring");
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (highlighted && triggerPressed == false)
        {
            if (Input.GetAxis("FireTrigger") == 1)
            {
                ButtonEffect();
                triggerPressed = true;
            }
        }

        if (Input.GetAxis("FireTrigger") < 1)
        {
            triggerPressed = false;
        }

    }

    private void ButtonEffect()
    {
        if (shipDetails.FullyEquipped())
        {
            DisplayDrone();
            AudioSource.PlayClipAtPoint(selectSFX, transform.position);
            Debug.Log("drone displayed" + Time.time);
        }
        else
        {
            AudioSource.PlayClipAtPoint(failureSFX, transform.position);
            Debug.Log("drone not fully equipped" + Time.time);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TriggerSphere")
        {
            highlighted = true;
            meshRenderer.material = highlightMaterial;
            AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TriggerSphere")
        {
            highlighted = false;
            meshRenderer.material = ogMaterial;
        }
    }

    private void DisplayDrone()
    {
        Destroy(displayModel.fuselage);
        displayModel.fuselage = (GameObject)Instantiate(shipDetails.fuselage, ring.transform );
        SocketPositions socketPositions = displayModel.fuselage.GetComponent<SocketPositions>();
        displayModel.fuselage.GetComponent<Renderer>().material.shader = displayModel.droneShader;

        Destroy(displayModel.engine);
        displayModel.engine = (GameObject)Instantiate(shipDetails.engine, ring.transform.position, ring.transform.rotation, ring.transform);
        displayModel.engine.transform.localPosition = socketPositions.enginePosition;
        displayModel.engine.GetComponent<Renderer>().material.shader = displayModel.droneShader;

        Destroy(displayModel.wing);
        displayModel.wing = (GameObject)Instantiate(shipDetails.wing, ring.transform.position, ring.transform.rotation, ring.transform);
        displayModel.wing.transform.localPosition = socketPositions.wingPosition;
        displayModel.wing.GetComponent<Renderer>().material.shader = displayModel.droneShader;

        Destroy(displayModel.nose);
        displayModel.nose = (GameObject)Instantiate(shipDetails.nose, ring.transform.position, ring.transform.rotation, ring.transform);
        displayModel.nose.transform.localPosition = socketPositions.NosePosition;
        displayModel.nose.GetComponent<Renderer>().material.shader = displayModel.droneShader;

        //TODO fix occlusion rendering on spawned drone
        for (int i = 0; i < displayModel.engine.GetComponent<Renderer>().materials.Length; i++)
        {
            //engine.GetComponent<Renderer>().materials[i].shader = Shader.Find("BumpedOutline");
        }
    }
}
