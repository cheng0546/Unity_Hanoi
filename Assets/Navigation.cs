using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{

	private Vector3 cameraPositionOffset = Vector3.zero;	
	private Quaternion cameraOrientationOffset = Quaternion.identity;
	
        void Start () {

     // attach the camera to the navigation rig

        Camera theCamera = (Camera)GameObject.FindObjectOfType (typeof(Camera)) ;
        Transform cameraTransform = theCamera.transform ;
        cameraTransform.SetParent (transform) ;
        cameraTransform.localPosition = cameraPositionOffset ;
        cameraTransform.localRotation = cameraOrientationOffset ;

    }

    void Update () {

        var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f ;

        var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f ;

        transform.Rotate (0, x, 0) ;

        transform.Translate (0, 0, z) ;      

    }
}
