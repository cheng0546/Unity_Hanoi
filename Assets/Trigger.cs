using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    Transform mouseObject;
    Rigidbody rb;
    protected Vector3 positionInit, positionEnd;
    protected bool buttonOnePressed = false ;

    // Start is called before the first frame update
    void Start()
    {
        mouseObject = GameObject.Find("Mouse").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider) {
        GetComponent<MeshRenderer>().materials[0].color = new Color(0, 255, 0);
    }

    private void OnTriggerStay(Collider collider) {
        if (Input.GetMouseButtonDown (0)) {
            buttonOnePressed = true ;
            positionInit = transform.position;
        }

        if (buttonOnePressed) {
            transform.SetParent(mouseObject, true);
            GetComponent<MeshRenderer>().materials[0].color = new Color(0, 0, 255);
            rb.isKinematic = true;
        }

        if (Input.GetMouseButtonUp (0)) {
            buttonOnePressed = false ;
            transform.SetParent(null);
            GetComponent<MeshRenderer>().materials[0].color = new Color(0, 255, 0);
            rb.isKinematic = false;
            positionEnd = transform.position;
            if (positionEnd.x > -5 && positionEnd.x < -3 && positionEnd.z > 4 && positionEnd.z < 6) {
                transform.position = new Vector3(-4, positionEnd.y, 5);
            } else if (positionEnd.x > -1 && positionEnd.x < 1 && positionEnd.z > 4 && positionEnd.z < 6) {
                transform.position = new Vector3(0, positionEnd.y, 5);
            } else if (positionEnd.x > 3 && positionEnd.x < 5 && positionEnd.z > 4 && positionEnd.z < 6) {
                transform.position = new Vector3(4, positionEnd.y, 5);
            } else {
                transform.position = positionInit;
            }
        }
    }

    private void OnTriggerExit(Collider collider) {
        GetComponent<MeshRenderer>().materials[0].color = new Color(255, 0, 0);
    }
}
