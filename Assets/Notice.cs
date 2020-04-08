using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WasaaMP
{
    public class Notice : MonoBehaviour
    {
        public int notice = 0;
        Text text;
        Transform gameObjectM, gameObjectR;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<Text>();
            gameObjectM = GameObject.Find("GameObjectM").transform;
            gameObjectR = GameObject.Find("GameObjectR").transform;

        }

        // Update is called once per frame
        void Update()
        {
            notice = CursorTool.notice;
            switch (notice)
            {
                case 0:
                    text.text = "Welcome to the Tower of Hanoi!";
                    break;
                case 1:
                    text.text = "You can only take the upper disk from one of the stacks.";
                    break;
                case 2:
                    text.text = "You cannot place a larger disk on top of a smaller one.";
                    break;
                case 3:
                    text.text = "Add a new disk successfully.";
                    break;
                case 4:
                    text.text = "The number of the disks is maximal.";
                    break;
            }
            if (gameObjectM.GetComponentsInChildren<Transform>(true).Length == 5 || gameObjectR.GetComponentsInChildren<Transform>(true).Length == 5)
            {
                text.text = "Congratulations!";
            }
        }
    }
}
