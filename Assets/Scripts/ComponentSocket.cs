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
    private GameObject compatibleComponent;
    private bool partEquipped = false;


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
            if (Input.GetAxis("RGripTrigger") < 1)
            {
                GetComponent<MeshFilter>().mesh = compatibleComponent.GetComponent<MeshFilter>().mesh;                
                partEquipped = true;
                compatibleComponentInVicinity = false;
                if (equipSFXCooldown <= 0)
                {
                    AudioSource.PlayClipAtPoint(equipSFX, transform.position);
                    equipSFXCooldown = 2;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == socketType)
        {
            AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
            GetComponent<MeshRenderer>().enabled = true;
            compatibleComponentInVicinity = true;
            compatibleComponent = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == socketType && partEquipped == false)
        {
            GetComponent<MeshRenderer>().enabled = false;
            compatibleComponentInVicinity = false;
        }
    }
}
