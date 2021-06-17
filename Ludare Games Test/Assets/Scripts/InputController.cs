using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public UnityAction moveLeftAction;
    public UnityAction moveRightAction;
    public UnityAction jumpAction;


    // Update is called once per frame
    void Update()
    {
        bool leftHeld = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool rightHeld = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        bool jump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        if (leftHeld && !rightHeld)
        {
            moveLeftAction.Invoke();
        }
        else if (rightHeld && !leftHeld)
        {
            moveRightAction.Invoke();
        }

        if (jump)
        {
            jumpAction.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Level Select");
        }
    }
}
