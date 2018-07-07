using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceSlider : MonoBehaviour {

    public enum Mode { Metallic, Gloss};
    public bool grabbed;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public float ymin;
    public float ymax;
    public MeshRenderer target;
    public Mode mode;
    public Material highlightMaterial;
        
    private Transform triggerTransform;
    private Renderer Meshrenderer;
    private bool triggerDown = false;
    private Material ogMaterial;

    private void Start()
    {
        Meshrenderer = GetComponent<Renderer>();
        ogMaterial = Meshrenderer.material;
    }

    // Update is called once per frame
    void Update() {
        if (grabbed)
        {
            OutputValue();
            if (triggerTransform)
            {
                transform.position = new Vector3(triggerTransform.position.x, triggerTransform.position.y, triggerTransform.position.z); //Sansform.InverseTransformPoint(triggerTransform.position).z
            }
            transform.localPosition = new Vector3(0, Mathf.Clamp(transform.localPosition.y, ymin, ymax), 0);
            if (GetComponent<AudioSource>().isPlaying == false)
            {
                GetComponent<AudioSource>().Play();
            }
        }

        if (Input.GetAxis("FireTrigger") == 1 && highlighted)
        {
            if (grabbed == false)
            {
                grabbed = true;
                AudioSource.PlayClipAtPoint(selectSFX, transform.position);
            }
        }

        if (Input.GetAxis("FireTrigger") < 1)
        {
            grabbed = false;
            triggerDown = true;
            GetComponent<AudioSource>().Stop();
        }
        
    }

    private void OutputValue()
    {
        float sliderValue = (transform.localPosition.y - ymin) / (ymax-ymin);
        switch (mode)
        {
            case Mode.Metallic:
                target.material.SetFloat("_Metallic", sliderValue);
                break;
            case Mode.Gloss:
                target.material.SetFloat("_Glossiness", sliderValue);
                break;
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TriggerSphere")
        {
            highlighted = true;
            Meshrenderer.material = highlightMaterial;
            triggerTransform = other.transform;
            AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TriggerSphere")
        {            
            highlighted = false;
            Meshrenderer.material = ogMaterial; 
        }
    }
}
