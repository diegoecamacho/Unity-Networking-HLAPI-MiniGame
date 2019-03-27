using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderLookAt : MonoBehaviour {

    Transform cameraTransform;

	// Use this for initialization
	void Start () {
        cameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(cameraTransform);
	}
}
