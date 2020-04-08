using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interaction : MonoBehaviour
{
    private int flag = 0;
    protected bool buttonOnePressed = false ;
    protected bool targetAcquired = false ;
    protected bool buttonTwoPressed = false ;
    protected bool buttonThreePressed = false ;
    protected GameObject target ;
    protected GameObject mouseObject;
    protected Vector3 worldPoint;
    protected Vector3 positionInit, positionEnd;

    void Start() {
        for (int i = 0; i < 4; i++) {
            GameObject cube = GameObject.Find("Cube" + i);
            cube.GetComponent<MeshRenderer>().materials[0].color = new Color(255, 0, 0);
        }

        mouseObject = GameObject.Find("Mouse");
    }

    void Update () {

        // if (Input.GetMouseButtonDown (0)) { 
        //     buttonOnePressed = true ;
        //     FindTarget ();
        // }

        if (Input.GetMouseButtonDown (1)) {
            if (flag == 0) {
                    flag = 1;
                } else {
                    flag = 0;
                }
        }
        if (flag == 1) {
            //mouseObject = hit.collider.gameObject;
            Vector3 targer = Camera.main.WorldToScreenPoint(mouseObject.transform.position);
            Vector3 mouse = Input.mousePosition;
            mouse.z = targer.z;
            Vector3 mouseScreenPos = Camera.main.ScreenToWorldPoint(mouse);
            mouseObject.transform.position = mouseScreenPos;
            positionEnd = mouseScreenPos;
        }

        // if (buttonOnePressed) {

        //     TranslateTarget () ;

        // }

        // if (buttonThreePressed) {
        // }

        // if (Input.GetMouseButtonUp (0)) {

        //     buttonOnePressed = false ;

        //     // LooseTarget () ;
        // }

        // if (Input.GetMouseButtonUp (1)) {

        //     buttonThreePressed = false ;

        //     // LooseTarget () ;
        // }

    }

    
    public void FindTarget () {
        
        Ray rayMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (rayMouse, out hit)) {

            targetAcquired = true;

            //print ("Found an object - distance: " + hit.distance);

            try {
                target = hit.collider.gameObject;
                positionInit = target.transform.position;
            } catch (NullReferenceException e) { 
                target = null ;         
                targetAcquired = false;
            }

        }

    }
}
