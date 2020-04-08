using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject btnObj = GameObject.Find("Canvas/Button");
        Button btn = (Button)btnObj.GetComponent<Button>();
        btn.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Quit() {
#if     UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log ("QUIT");
#else
            Application.Quit();
            Debug.Log("QUIT");
#endif
    }
}

