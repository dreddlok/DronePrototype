﻿using System.Collections;
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
    public MeshRenderer previewTarget;
    public SpriteRenderer preview;
    public Texture texture;

    private WorldSpacePatternButton[] toggleGroup;
    private Material ogMaterial;
    private Renderer meshRenderer;
    private bool triggerDown = false;

    // Use this for initialization
    void Start () {
        buttonHighlight = transform.Find("ButtonSelectedSprite").gameObject;
        preview = transform.Find("Image").GetComponent<SpriteRenderer>();
        buttonHighlight.SetActive(false);
        meshRenderer = GetComponent<Renderer>();
        ogMaterial = meshRenderer.material;
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
        previewTarget.material.SetTexture("_MainTex", texture);
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
