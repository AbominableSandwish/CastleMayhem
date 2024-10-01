using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    bool ActionUsed = false;
    public enum Action
    {
        None,
        Confim,
        Cancel
    }

    Action action = Action.None;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            action = Action.Confim;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            action = Action.Cancel;
        }

        if(action != Action.None)
            ActionUsed = true;
    }

    public bool ActionFree(Action action)
    {
        bool actionFree = false;
        if (ActionUsed)
        {
            if(this.action == action)
            {
                actionFree = true;
                ActionUsed = false;
                this.action = Action.None;
            }
        }
        return actionFree;
    }
}
