using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {

    public Color highlightColour;
    public Material highlightMaterial;
    public AudioClip highlightSFX;
    private Material[] oldMaterials;
    private Color[] oldColours;
    private Renderer meshRenderer;
    private Shader ogShader;

    private void Start()
    {
        meshRenderer = GetComponent<Renderer>();
        ogShader = meshRenderer.material.shader;
    }

    public void Select()
    {
        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            //renderer.materials[i].color = highlightColour;
            //renderer.materials[i] = highlightMaterial;
            meshRenderer.materials[i].shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        }
        AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
    }

    public void DeSelect()
    {
        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            //renderer.materials[i] = oldMaterials[i];
            //renderer.materials[i].color = oldMaterials[i].color;
            meshRenderer.materials[i].shader = ogShader;
        }
        
    }
}
