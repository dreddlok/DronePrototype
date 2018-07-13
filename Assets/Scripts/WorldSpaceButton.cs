using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour {

    public enum Mode { Color, Custom, Pattern, Decal, Toggle, Fuselage, Wing, Engine, Nose };
    public GameObject buttonHighlight;
    public bool selected;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public Material highlightMaterial;
    public Mode mode;
    public GameObject toggleTarget;

    private Material ogMaterial;
    private Renderer meshRenderer;
    private WorldSpaceButton[] toggleGroup;
    private bool triggerPressed;

    // Use this for initialization
    void Start () {
        //buttonHighlight.SetActive(false);
        meshRenderer = GetComponent<Renderer>();
        ogMaterial = meshRenderer.material;
        if (transform.parent)
        {
            toggleGroup = transform.parent.GetComponentsInChildren<WorldSpaceButton>();
        }
        if (mode == Mode.Toggle)
        {
            toggleTarget.SetActive(selected);
        }
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (highlighted && triggerPressed == false)
        {
            if (Input.GetAxis("FireTrigger") == 1)
            {                
               if (mode != Mode.Toggle )
                {
                    if (selected == false)
                    {
                        ButtonEffect();
                        triggerPressed = true;
                    }
                } else
                {
                    ButtonEffect();
                    triggerPressed = true;
                }
            }
        }

        if (Input.GetAxis("FireTrigger") < 1)
        {
            triggerPressed = false;
        }

    }

    private void ButtonEffect()
    {
        if (toggleGroup != null)
        {
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                toggleGroup[i].selected = false;
                if (toggleGroup[i].toggleTarget)
                {
                    toggleGroup[i].toggleTarget.SetActive(false);
                }
                toggleGroup[i].highlighted = false;
                toggleGroup[i].buttonHighlight.SetActive(false);
            }
        }       
        
        AudioSource.PlayClipAtPoint(selectSFX, transform.position);
        switch (mode)
        {
            case Mode.Color:
                toggleTarget.SetActive(true);
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Pattern:
                toggleTarget.SetActive(true);
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Decal:
                toggleTarget.SetActive(true);
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Toggle:
                selected = !selected;
                buttonHighlight.SetActive(selected);
                toggleTarget.SetActive(selected);
                break;
            case Mode.Fuselage:
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Wing:
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Engine:
                buttonHighlight.SetActive(true);
                selected = true;
                break;
            case Mode.Nose:
                buttonHighlight.SetActive(true);
                selected = true;
                break;
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


}
