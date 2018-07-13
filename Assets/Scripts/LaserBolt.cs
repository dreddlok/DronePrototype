using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour {

    public float Speed = 50;
    public AudioClip collisonSFX;
    public GameObject impactEffect;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(collisonSFX, transform.position);
        var impact = (GameObject)Instantiate(impactEffect, transform.position, Quaternion.Inverse(transform.rotation));
        Destroy(impact, 2.0f);
        Destroy(gameObject);
    }
}
