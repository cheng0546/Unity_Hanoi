using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    Transform gameObjectM, gameObjectR;
    Transform[] fireworks;
    GameObject fireworkss;
    // Start is called before the first frame update
    void Start()
    {
        gameObjectM = GameObject.Find("GameObjectM").transform;
        gameObjectR = GameObject.Find("GameObjectR").transform;
        fireworks = GetComponentsInChildren<Transform>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObjectM.GetComponentsInChildren<Transform>(true).Length == 5 || gameObjectR.GetComponentsInChildren<Transform>(true).Length == 5) {            
            foreach (Transform firework in fireworks) {
                if (!firework.name.ToString().Equals("Firework")) {
                    firework.gameObject.SetActive(true);
                } 
            }
        } else {
            foreach (Transform firework in fireworks) {
                if (!firework.name.ToString().Equals("Firework")) {
                    firework.gameObject.SetActive(false);
                }
            }
        }
    }
}
