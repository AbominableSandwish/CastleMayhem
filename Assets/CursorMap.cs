using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorMap : MonoBehaviour
{

    BuildingSystem buildingSystem;
    InputSystem inputSystem;
    // Start is called before the first frame update
    void Start()
    {
        cursorView = GameObject.Find("Cursor").GetComponent<CursorView>();
        buildingSystem = FindAnyObjectByType<BuildingSystem>();
        inputSystem = FindAnyObjectByType<InputSystem>();
    }

    CursorView cursorView;
    const int height = 20;

    // Update is called once per frame
    void Update()
    {
        if (inputSystem.isClicked)
        {
            Vector2 mousePos = GameObject.Find("Cursor").GetComponent<CursorView>().GetPosition() * 2f;
            Point pos = new Point();
            pos.X = ((int)mousePos.x - 1) / 2 + ((int)mousePos.y - 1) * 1 + height / 2;
            pos.Y = ((int)mousePos.y - 1) * 1 + ((int)mousePos.x - 1) / (-2) + height / 2;
            Search(pos);
        }
    }

    void Search(Point position)
    {
        bool FocusBuilding = false;
        foreach(Building building in buildingSystem.Buildings)
        {
            if (building.AABB(position))
            {
                FocusBuilding = true;
                FindAnyObjectByType<BuildingPanelView>().ShowPanel(building);
            }
        }

        if (!FocusBuilding)
            FindAnyObjectByType<BuildingPanelView>().HidePanel();
    }
}
