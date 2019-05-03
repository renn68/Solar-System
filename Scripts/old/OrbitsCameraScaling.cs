using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitsCameraScaling : MonoBehaviour {

    public Camera ScallingCamera;
    private LineRenderer lr;
	// Use this for initialization
	void Start () {
        ScallingCamera = Camera.main;
        lr = GetComponent<LineRenderer>();
        lr.widthMultiplier = ScallingCamera.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
        if (ScallingCamera.orthographicSize > 0.03f)
        {
            lr.widthMultiplier = ScallingCamera.orthographicSize;
        }
        else
        {
            lr.widthMultiplier = 0.03f;
        }
    }
}
