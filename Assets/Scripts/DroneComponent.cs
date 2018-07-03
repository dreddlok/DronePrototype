using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneComponent : MonoBehaviour {

    public bool onStand = true;
    public Vector3 rotSpeed;
    public Vector3 defaultPos;
    public Quaternion defaultRot;
    [Header("Part Stats")]
    public string partName;
    public float speed;
    public float armor;
    public float acceleration;
    public float handling;


    private void Start()
    {
        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        if (onStand)
        {
            transform.parent.transform.Rotate(rotSpeed);
        }
	}
}
