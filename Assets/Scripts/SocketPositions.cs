using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketPositions : MonoBehaviour
{

    public Vector3 enginePosition;
    public Vector3 wingPosition;
    public Vector3 NosePosition;

    public void SetSocketPositions(GameObject fuselage)
    {
        SocketPositions fuselageSocket = fuselage.GetComponent<SocketPositions>();
        enginePosition = fuselageSocket.enginePosition;
        wingPosition = fuselageSocket.wingPosition;
        NosePosition = fuselageSocket.NosePosition;
        foreach (Transform child in transform)
        {
            switch (child.gameObject.tag)
            {
                case "Engine":
                    child.localPosition = enginePosition;
                    break;
                case "Wing":
                    child.localPosition = wingPosition;
                    break;
                case "Nose":
                    child.localPosition = NosePosition;
                    break;
            }
        }
    }

}
