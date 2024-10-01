using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Action
    {
        NewParty,
        LoadPary
    }
    [SerializeField] Action action;
    private void Start()
    {
        switch (action)
        {
            case Action.NewParty:
                GetComponent<MapManager>().New();
                break; 
            case Action.LoadPary:
                GetComponent<MapManager>().Load();
                break;
        }

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }
}
