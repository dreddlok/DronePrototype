using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCameraRot : MonoBehaviour {

    public bool lockRot;
	
	// Update is called once per frame
	void Update () {
		if (lockRot)
        {
            transform.localEulerAngles = (new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
        }
	}
}
