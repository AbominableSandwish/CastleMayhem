using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MenuBuilding : MonoBehaviour
{
    Animator _anim;
    bool _isOpen = false;
    InputSystem input;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        input =  GameObject.FindFirstObjectByType<InputSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isOpen)
        {
            if (input.ActionFree(InputSystem.Action.Cancel))
            {
                CloseMenu();
            }
        }
    }

    public void OpenMenu()
    {
        _anim.SetTrigger("isOpen");
        _isOpen = true;
    }

    public void CloseMenu()
    {
        _anim.SetTrigger("isClose");
        _isOpen = false;
    }

}
