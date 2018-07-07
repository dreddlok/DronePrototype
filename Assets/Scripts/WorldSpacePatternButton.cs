using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpacePatternButton : MonoBehaviour {

    public GameObject buttonHighlight;
    public bool selected;
    public bool highlighted;
    public AudioClip selectSFX;
    public AudioClip highlightSFX;
    public Material highlightMaterial;
    public Color color;
    public MeshRenderer target;
    public SpriteRenderer preview;
    public Texture texture;

    private WorldSpacePatternButton[] toggleGroup;
    private Color ogColour;
    private Material ogMaterial;
    private Renderer renderer;
    private bool triggerDown = false;

    // Use this for initialization
    void Start () {
        buttonHighlight = transform.Find("ButtonSelectedSprite").gameObject;
        preview = transform.Find("Image").GetComponent<SpriteRenderer>();
        buttonHighlight.SetActive(false);
        renderer = GetComponent<Renderer>();
        ogColour = renderer.material.color;
        ogMaterial = renderer.material;
        toggleGroup = transform.parent.GetComponentsInChildren<WorldSpacePatternButton>();
    }
	
	// Update is called once per frame
	void Update () {


            if (highlighted)
            {
            if (Input.GetAxis("FireTrigger") == 1 && triggerDown == false)
            {
                triggerDown = false;
                if (!selected)
                {
                    SelectColor();
                }
            }
        }

    }

     private void SelectColor()
    {
        for (int i = 0; i < toggleGroup.Length; i++)
        {
            toggleGroup[i].GetComponent<WorldSpacePatternButton>().selected = false;
            toggleGroup[i].GetComponent<WorldSpacePatternButton>().buttonHighlight.SetActive(false);
        }
        selected =  true;
        buttonHighlight.SetActive(true);
        AudioSource.PlayClipAtPoint(selectSFX, transform.position);
        target.material.SetTexture("_MainTex", texture);
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
