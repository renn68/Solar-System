using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceScale : MonoBehaviour {

    public Camera cam;
    private float scaler;
    
	// Update is called once per frame
	void LateUpdate () {
        scaler = Vector3.Distance(transform.position, cam.transform.position) / 50f;
        this.transform.localScale = new Vector3(1f, 1f, 1f) * scaler * Time.timeScale;
	}
}
