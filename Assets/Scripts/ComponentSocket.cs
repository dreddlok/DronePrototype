using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSocket : MonoBehaviour {

    public float radius;
    public string socketType;
    public AudioClip equipSFX;
    public float equipSFXCooldown = 2;
    public AudioClip highlightSFX;

    private bool compatibleComponentInVicinity = false;
    public GameObject equippedPart;
    public GameObject compatibleComponent;
    public bool partEquipped = false;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (equipSFXCooldown > 0)
        {
            equipSFXCooldown -= Time.deltaTime;
        }
        if (compatibleComponentInVicinity)
        {
            EquipComponent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == socketType)
        {
            AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
            
            if (partEquipped)
            {
                equippedPart.SetActive(false);
                GetComponent<MeshRenderer>().enabled = true;
            }
            compatibleComponentInVicinity = true;
            compatibleComponent = other.gameObject;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        if (other.gameObject.tag == socketType )
        {
            compatibleComponentInVicinity = false;
            if (partEquipped)
            {
                equippedPart.SetActive(true);
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void EquipComponent()
    {
        if (Input.GetAxis("RGripTrigger") < 1)
        {
            if (compatibleComponent != null)
            {
                if (equippedPart != null)
                {
                    Destroy(equippedPart);
                }
                equippedPart = (GameObject)Instantiate(compatibleComponent, transform.position, transform.rotation, transform);//Instantiate(compatibleComponent, transform);
                equippedPart.GetComponent<DroneComponent>().rotSpeed = Vector3.zero;
                equippedPart.GetComponent<DroneComponent>().onStand = false;
                equippedPart.GetComponent<Collider>().enabled = false;
                equippedPart.transform.localScale = Vector3.one;
            }
            equippedPart.tag = socketType;
            partEquipped = true;
            if (equipSFXCooldown <= 0)
            {
                AudioSource.PlayClipAtPoint(equipSFX, transform.position);
                equipSFXCooldown = 2;
            }
            ShipDetails shipDetails = transform.parent.GetComponent<ShipDetails>();
            switch (socketType)
            {
                
                case "Fuselage":
                    SocketPositions holoSocketPositions = transform.parent.GetComponent<SocketPositions>();
                    holoSocketPositions.SetSocketPositions(equippedPart);
                    shipDetails.fuselage = equippedPart;
                    break;
                case "Engine":
                    shipDetails.engine = equippedPart;
                    break;
                case "Nose":
                    shipDetails.nose = equippedPart;
                    break;
                case "Wing":
                    shipDetails.wing = equippedPart;
                    break;
            }
            transform.parent.GetComponent<ShipDetails>().RefreshStats();
            compatibleComponentInVicinity = false;
        }
    }
}
