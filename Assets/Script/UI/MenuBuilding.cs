using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBuilding : MonoBehaviour
{
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu()
    {
        _anim.SetTrigger("isOpen"); 
    }

    public void CloseMenu()
    {
        _anim.SetTrigger("isClose");
        GameObject.FindAnyObjectByType<MapManager>().CancelBuilding();
    }
}
