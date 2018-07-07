using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceButton : MonoBehaviour {

    public enum Mode { Color, Custom, Pattern, Decal };
    public GameObject buttonHighlight;
    public bool selected;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public Material highlightMaterial;
    public Mode mode;
    public GameObject toggleTarget;

    private Color ogColour;
    private Material ogMaterial;
    private Renderer renderer;
    private WorldSpaceButton[] toggleGroup;

    // Use this for initialization
    void Start () {
        //buttonHighlight.SetActive(false);
        renderer = GetComponent<Renderer>();
        ogColour = renderer.material.color;
        ogMaterial = renderer.material;
        toggleGroup = transform.parent.GetComponentsInChildren<WorldSpaceButton>();
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (highlighted)
        {
            if (Input.GetAxis("FireTrigger") == 1 && selected == false)
            {                
                ButtonEffect();
            }
        }

    }

    private void ButtonEffect()
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
        selected = true; ;
        buttonHighlight.SetActive(true);
        AudioSource.PlayClipAtPoint(selectSFX, transform.position);
        switch (mode)
        {
            case Mode.Color:
                toggleTarget.SetActive(true);
                break;
            case Mode.Pattern:
                toggleTarget.SetActive(true);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TriggerSphere")
        {
            highlighted = true;
            renderer.material = highlightMaterial;
            AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TriggerSphere")
        {
            highlighted = false;
            renderer.material = ogMaterial;
        }
    }
}
