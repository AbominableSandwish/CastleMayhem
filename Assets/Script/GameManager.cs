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

    [SerializeField] private Action action;

    public bool IsLoaded()
    {
        bool isLoader = false;
        if(action == Action.LoadPary)
            isLoader = true;
        return isLoader;
    }
    private void Start()
    {
        switch (action)
        {
            case Action.NewParty:
                GetComponent<MapManager>().New();
                break; 
            case Action.LoadPary:
                GetComponent<MapManager>().Load();
                GetComponent<BuildingSystem>().Load();
                break;
        }

        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
    }


}
