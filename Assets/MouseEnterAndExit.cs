using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseEnterAndExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter () {
        GetComponent<MeshRenderer>().materials[0].color = new Color(0, 255, 0);
    }

    public void OnMouseExit () {
        GetComponent<MeshRenderer>().materials[0].color = new Color(255, 0, 0);
    }

    public void OnMouseDown () {
        GetComponent<MeshRenderer>().materials[0].color = new Color(0, 0, 225);
    }

    public void OnMouseUp () {
        GetComponent<MeshRenderer>().materials[0].color = new Color(0, 255, 0);
    }

}
