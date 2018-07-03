using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDroneCamers : MonoBehaviour {

    public enum DroneMonitorMode { FPV, TP, Offline};
    public DroneMonitorMode currentlySelectedMode = DroneMonitorMode.Offline;
    public GameObject FPVCamera;
    public GameObject TPCamera;
    public GameObject RenderSurface;
    public Material cameraMaterial;
    public Material offlineMaterial;
    public AudioClip changeSFX;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("ChangeView"))
        {
            Debug.Log("Change view button pressed");
            switch (currentlySelectedMode)
            {
                case DroneMonitorMode.FPV:
                    TPCamera.SetActive(true);
                    FPVCamera.SetActive(false);
                    currentlySelectedMode = DroneMonitorMode.TP;
                    RenderSurface.GetComponent<Renderer>().material = cameraMaterial;
                    AudioSource.PlayClipAtPoint(changeSFX, transform.position);
                    break;
                case DroneMonitorMode.TP:
                    TPCamera.SetActive(false);
                    FPVCamera.SetActive(false);
                    RenderSurface.GetComponent<Renderer>().material = offlineMaterial;
                    currentlySelectedMode = DroneMonitorMode.Offline;
                    AudioSource.PlayClipAtPoint(changeSFX, transform.position);
                    break;
                case DroneMonitorMode.Offline:
                    TPCamera.SetActive(false);
                    FPVCamera.SetActive(true);
                    currentlySelectedMode = DroneMonitorMode.FPV;
                    RenderSurface.GetComponent<Renderer>().material = cameraMaterial;
                    AudioSource.PlayClipAtPoint(changeSFX, transform.position);
                    break;
            }
        }
	}
}
