using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditButton : MonoBehaviour {
    
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
        shipDetails.fuselage = displayModel.fuselage;

        shipDetails.engine = displayModel.engine;

        shipDetails.nose = displayModel.nose;

        shipDetails.wing = displayModel.wing;

        shipDetails.RefreshStats();

        ComponentSocket[] componentSockets = FindObjectsOfType<ComponentSocket>();
        for (int i = 0; i < componentSockets.Length; i++)
        {
            switch (componentSockets[i].socketType)
            {
                case "Wing":
                    EquipPartToSlot(componentSockets[i], shipDetails.wing);
                    break;
                case "Fuselage":
                    EquipPartToSlot(componentSockets[i], shipDetails.fuselage);
                    break;
                case "Nose":
                    EquipPartToSlot(componentSockets[i], shipDetails.nose);
                    break;
                case "Engine":
                    EquipPartToSlot(componentSockets[i], shipDetails.engine);
                    break;
            }
        }

        //TODO fix occlusion rendering on spawned drone
        for (int i = 0; i < displayModel.engine.GetComponent<Renderer>().materials.Length; i++)
        {
            //engine.GetComponent<Renderer>().materials[i].shader = Shader.Find("BumpedOutline");
        }
    }

    private void EquipPartToSlot(ComponentSocket socket, GameObject part)
    {
        if (socket.equippedPart != null)
        {
            Destroy(socket.equippedPart);
        }
            socket.equippedPart = (GameObject)Instantiate(part, transform.position, transform.rotation, transform);//Instantiate(compatibleComponent, transform);
            socket.equippedPart.GetComponent<DroneComponent>().rotSpeed = Vector3.zero;
            socket.equippedPart.GetComponent<DroneComponent>().onStand = false;
            socket.equippedPart.GetComponent<Collider>().enabled = false;
            socket.equippedPart.transform.localScale = Vector3.one;
            socket.equippedPart.tag = socket.socketType;
            Renderer eqPartRenderer = socket.equippedPart.GetComponent<Renderer>();
            for (int i = 0; i<eqPartRenderer.materials.Length; i++)
            {
                eqPartRenderer.materials[i].shader = Shader.Find("Standard");
            }
            socket.partEquipped = true;
    }
}
