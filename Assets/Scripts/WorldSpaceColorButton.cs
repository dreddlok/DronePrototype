using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceColorButton : MonoBehaviour {

    public GameObject buttonHighlight;
    public bool selected;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public Material highlightMaterial;
    public Color color;
    public MeshRenderer target;
    public SpriteRenderer preview;
    public WorldSpaceColorButton[] toggleGroup;

    private Color ogColour;
    private Material ogMaterial;
    private Renderer renderer;

    // Use this for initialization
    void Start () {
        buttonHighlight.SetActive(false);
        renderer = GetComponent<Renderer>();
        ogColour = renderer.material.color;
        ogMaterial = renderer.material;
        toggleGroup = transform.parent.GetComponentsInChildren<WorldSpaceColorButton>();
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (highlighted)
        {            
            if (Input.GetAxis("FireTrigger") == 1)
            {                
                if (!selected)
                {
                    SelectColor();
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        preview.color = color;
    }

    private void SelectColor()
    {
        for (int i = 0; i < toggleGroup.Length; i++)
        {
            toggleGroup[i].GetComponent<WorldSpaceColorButton>().selected = false;
            toggleGroup[i].GetComponent<WorldSpaceColorButton>().buttonHighlight.SetActive(false);
        }
        selected =  true;
        buttonHighlight.SetActive(true);
        AudioSource.PlayClipAtPoint(selectSFX, transform.position);
        target.material.color = color;
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
