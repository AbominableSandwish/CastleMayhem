using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    bool ActionUsed = false;
    public bool isClicked = false;
    bool TickSecurity = false;
    const int nbrTick = 48;
    int counter = 0;
    public enum Action
    {
        None,
        Confim,
        Cancel
    }

    Action action = Action.None;
    // Update is called once per frame
    void Update()
    {
        isClicked = false;
        if (Input.GetButton("Fire1"))
        {
            action = Action.Confim;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            action = Action.Cancel;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TickSecurity = true;
        }

        if (TickSecurity)
        {
            if (counter >= nbrTick)
            {
                ResetClick();
            }
            counter++;
        }

        if (action != Action.None)
            ActionUsed = true;

     
    }

    public void ResetClick()
    {
        TickSecurity = false;
        isClicked = true;
        counter = 0;
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
