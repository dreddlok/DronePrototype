using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject : MonoBehaviour {

    public Color highlightColour;
    public Material highlightMaterial;
    public AudioClip highlightSFX;
    private Material[] oldMaterials;
    private Color[] oldColours;
    private Renderer renderer;

    private void Start()
    {           
        renderer = GetComponent<Renderer>();
        /*oldMaterials = renderer.materials;        
        oldColours = new Color[renderer.materials.Length];
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            oldColours[i] = renderer.materials[i].color;
            //oldMaterials[i] = renderer.materials[i];
        }*/
    }

    public void Select()
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            //renderer.materials[i].color = highlightColour;
            //renderer.materials[i] = highlightMaterial;
            renderer.materials[i].shader = Shader.Find("Self-Illumin/Outlined Diffuse");
        }
        AudioSource.PlayClipAtPoint(highlightSFX, transform.position);
    }

    public void DeSelect()
    {
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            //renderer.materials[i] = oldMaterials[i];
            //renderer.materials[i].color = oldMaterials[i].color;
            renderer.materials[i].shader = Shader.Find("Diffuse");
        }
        
    }
}
