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
    public MeshRenderer previewTarget;
    public SpriteRenderer preview;
    public WorldSpaceColorButton[] toggleGroup;
    
    private Material ogMaterial;
    private Renderer meshRenderer;

    // Use this for initialization
    void Start () {
        buttonHighlight.SetActive(false);
        meshRenderer = GetComponent<Renderer>();
        ogMaterial = meshRenderer.material;
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
        previewTarget.material.color = color;
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
