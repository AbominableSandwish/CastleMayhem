using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    private List<Building> _buildings;
    RessourceSystem _sourceSystem;

    // Start is called before the first frame update
    void Awake()
    {
        _sourceSystem = GetComponent<RessourceSystem>();

        _buildings = new List<Building>();
        _buildings.Add(new Factory(GameObject.Find("Building_0")));
        _buildings.Add(new MiningCamp(GameObject.Find("Building_1")));
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        foreach (Building building in _buildings) {
            building.Update(dt);
        }
    }

    public void Complete(GameObject obj)
    {
        foreach (Building building in _buildings)
        {
            if(building.obj == obj)
            {
                _sourceSystem.AddRessource(building.Complete());
            }
        }
    }

    public bool GetCanComplete(GameObject obj)
    {
        bool canComplete = false;
        foreach (Building building in _buildings)
        {
            if (building.obj == obj)
            {
                canComplete = building.CanComplete;
            }
        }
        return canComplete;
    }
}
