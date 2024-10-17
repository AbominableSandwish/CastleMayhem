using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings;
    RessourceSystem _sourceSystem;
    Sprite[] sprites;

    [SerializeField] GameObject particle;

    [SerializeField] Sprite spriteConstruct;
    [SerializeField] Tilemap tilemap;
    MapManager mapManager;

    Building newBuilding;

    const int height = 20;
    bool isPreview = false;
    int idSprite = -1;
    Vector3Int lastpos;

    Vector3 offset = new Vector3(0, 1f);

    [SerializeField] RectTransform CanvasRect;

    public List<Building> Buildings { get => _buildings; set => _buildings = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _sourceSystem = GetComponent<RessourceSystem>();
        _buildings = new List<Building>();
        sprites = Resources.LoadAll<Sprite>("Sprites/BuildingSprite");      
    }

    private void Start()
    {
        if(GameObject.FindFirstObjectByType<GameManager>().IsLoaded() != true)
            this._buildings = new List<Building>();
        mapManager = GetComponent<MapManager>();
    }

    // Update is called once per frame

    Building buildInUpgrade;
    void Update()
    {
        float dt = Time.deltaTime;
        if (_buildings !=  null)
        {
            foreach (Building building in _buildings)
            {
                building.Update(dt);
                if (building.IsUpgrading)
                {
                    buildInUpgrade = building;
                }

                if(building.button != null)
                {
                    Vector2Int posCell = building.Position;
                    Point pos = new Point();

                    pos.X = 2 * posCell.x + posCell.y * 2;
                    pos.Y = posCell.x - posCell.y;

                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(pos.Y / 2.0f, pos.X / 8.0f - 5));
                    building.button.GetComponent<RectTransform>().anchoredPosition = screenPoint - CanvasRect.sizeDelta / 2f + new Vector2(0, 50);
                }
            }
        }

        if(buildInUpgrade != null)
        {
            _buildings.Remove(buildInUpgrade);
            FinishUpgrade(buildInUpgrade);
            buildInUpgrade = null;
        }

        if (isPreview)
        {
            if (!mapManager.isConfirm)
            {
                Refresh();
                Vector2 mousePos = GameObject.Find("Cursor").GetComponent<CursorView>().GetPosition() * 2f;

                Point pos = new Point();
                pos.X = ((int)mousePos.x - 1) / 2 + ((int)mousePos.y - 1) * 1 + height / 2;
                pos.Y = ((int)mousePos.y - 1) * 1 + ((int)mousePos.x - 1) / (-2) + height / 2;

                if (pos.X >= 0 && pos.X < height - 1 && pos.Y >= 0 && pos.Y < height - 1)
                {
                    Tile tile = new Tile();
                    if (idSprite != -1)
                    {
                        tile.sprite = sprites[idSprite];
                    }
                    else
                    {
                        tile.sprite = spriteConstruct;
                    }
                    tilemap.SetTile(new Vector3Int(pos.X, pos.Y, 0), tile);
                }
                //  preview.GetComponent<Tilemap>().SetTile() = new Vector3(pos.X, pos.Y);
                lastpos = new Vector3Int(pos.X, pos.Y);
            }
        }
    }

    void Refresh()
    {
        Tile tile = new Tile();
        tilemap.SetTile(lastpos, tile);
    }

    public void Complete(GameObject obj)
    {
        foreach (Building building in _buildings)
        {
            _sourceSystem.AddRessource(building.Complete());
        }
    }

    public bool GetCanComplete(GameObject obj)
    {
        bool canComplete = false;
        foreach (Building building in _buildings)
        {
            canComplete = building.CanComplete;
        }
        return canComplete;
    }

    public void Preview(int idSprite)
    {
        tilemap.color = new UnityEngine.Color(1, 1, 1, 0.5f);
        isPreview = true;

        this.idSprite = idSprite;
    }

    public void CancelPreview()
    {
        isPreview = false;
        Refresh();
        this.idSprite = -1;
    }

    public void ActionConfirmed(PanelChoice.Type action)
    {
       
        switch (action)
        {
            case PanelChoice.Type.New:
                NewBuilding();
                CancelPreview();
                break;
        }
        mapManager.CancelBuilding();
    }

    public void NewBuilding()
    {
        GameObject building;
        building = new GameObject();
        Tile tile;
        tile = new Tile();
        tile.sprite = spriteConstruct;
        ConstructionView view;
        switch ((Building.Type)idSprite)
        {
            case Building.Type.Factory:              
                building.name = "Factory_";       
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(lastpos.x, lastpos.y, 0), tile);
                this.newBuilding = new BuildingZone(new Vector2Int(lastpos.x, lastpos.y), Building.Type.Factory, Factory.timeToBuild);
                this.newBuilding.Object = building;
                this._buildings.Add(this.newBuilding);
                view = building.AddComponent<ConstructionView>();
                view.SetConstructionView(spriteConstruct, (BuildingZone)this.newBuilding);
                _sourceSystem.ReduceRessource(new RessourceSystem.Package(RessourceSystem.Type.A, Factory.COSTMATERIAL1));
                break;
            case Building.Type.MiningCamp:
                building.name = "MiningCamp_";
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(lastpos.x, lastpos.y, 0), tile);
                this.newBuilding = new BuildingZone(new Vector2Int(lastpos.x, lastpos.y), Building.Type.MiningCamp, MiningCamp.timeToBuild);
                this.newBuilding.Object = building;
                this._buildings.Add(this.newBuilding);
                view = building.AddComponent<ConstructionView>();
                view.SetConstructionView(spriteConstruct, (BuildingZone)this.newBuilding);
                _sourceSystem.ReduceRessource(new RessourceSystem.Package(RessourceSystem.Type.A, MiningCamp.COSTMATERIAL1));
                break;
        }
        this.newBuilding.sprite = spriteConstruct;
        string json = ExportData();
        StreamManager.WriteToFile("build.data", json);
    }

      public void Upgrade(Building building)
    {
        Building.Type next;
        if (building.type == Building.Type.InContruct)
        {
            next = ((BuildingZone)building).NextType;
        }
        else
        {
            next = building.type;
            this.Buildings.Remove(building);
        }
        
        GameObject obj;
        obj = new GameObject();
        Tile tile;
        tile = new Tile();
        tile.sprite = sprites[(int)next];
        BuildingZone buildingUpgrade = null;
        ConstructionView view;
        switch (next)
        {
            case Building.Type.Factory:
                obj.name = "Factory_";
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(building.position.x, building.position.y, 0), tile);
                buildingUpgrade = new BuildingZone(new Vector2Int(building.position.x, building.position.y), building.type, building.GetTimeToConstruct(), building.level+1);
                buildingUpgrade.Object = obj;
                this._buildings.Add(buildingUpgrade);
                view = obj.AddComponent<ConstructionView>();
                view.SetConstructionView(spriteConstruct, buildingUpgrade);
                break;
            case Building.Type.MiningCamp:
                obj.name = "MiningCamp_";
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(building.position.x, building.position.y, 0), tile);
                buildingUpgrade = new BuildingZone(new Vector2Int(building.position.x, building.position.y), building.type, building.GetTimeToConstruct(), building.level+1);
                buildingUpgrade.Object = obj;
                this._buildings.Add(buildingUpgrade);
                view = obj.AddComponent<ConstructionView>();
                view.SetConstructionView(spriteConstruct, buildingUpgrade);
                break;
        }
        buildingUpgrade.sprite = sprites[(int)next];
        string json = ExportData();
        StreamManager.WriteToFile("build.data", json);
    }

    public void FinishUpgrade(Building building)
    {
        Building.Type next;
        if (building.type == Building.Type.InContruct)
        {
            next = ((BuildingZone)building).NextType;
        }
        else
        {
            next = building.type;
            this.Buildings.Remove(building);
        }
        
        GameObject obj;
        obj = new GameObject();
        Tile tile;
        tile = new Tile();
        tile.sprite = sprites[(int)next];
        Building buildingUpgrade = null;
        switch (next)
        {
            case Building.Type.Factory:
                obj.name = "Factory_";
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(building.position.x, building.position.y, 0), tile);
                buildingUpgrade = new Factory(new Vector2Int(building.position.x, building.position.y), building.level+1);
                buildingUpgrade.Object = obj;
                this._buildings.Add(buildingUpgrade);
                break;
            case Building.Type.MiningCamp:
                obj.name = "MiningCamp_";
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(building.position.x, building.position.y, 0), tile);
                buildingUpgrade = new MiningCamp(new Vector2Int(building.position.x, building.position.y), building.level+1);
                buildingUpgrade.Object = obj;
                this._buildings.Add(buildingUpgrade);
                break;
        }
        buildingUpgrade.sprite = sprites[(int)next];
        string json = ExportData();
        StreamManager.WriteToFile("build.data", json);
    }

    public void SetButton(UnityEngine.UI.Button button, Building building)
    {
        button.onClick.AddListener(delegate {this.Complete(building);});
    }


    public void Complete(Building building)
    {
        building.button.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        RessourceSystem.Package pack = building.Complete();
        if (pack != null)
        {
            Point worldPos = new Point();

            worldPos.X = 2 * building.position.x + building.position.y * 2;
            worldPos.Y = building.position.x - building.position.y;
            switch (pack.Type)
            {
                case RessourceSystem.Type.A:
                    Instantiate(particle, new Vector3(worldPos.Y / 2.0f, worldPos.X / 8.0f - 4, -0.25f), Quaternion.identity);
                    break;
                case RessourceSystem.Type.B:
                    Instantiate(particle, new Vector3(worldPos.Y / 2.0f, worldPos.X / 8.0f - 4, -0.25f), Quaternion.identity);
                    break;
            }
            _sourceSystem.AddRessource(pack);
        }
        
        GetComponent<ViewSystem>().RemoveBtn(building);
        if (building.Object != null)
        {
          
            Destroy(building.Object);
        }

    }

    public string ExportData()
    {
        string buildingsToJson = "";
        foreach (Building building in _buildings)
        {
            buildingsToJson += JsonUtility.ToJson(building) + "\n";
        }
        return buildingsToJson;
    }

    public List<Building> Import(string json)
    {
        List<Building> buildings = new List<Building>();
        string row = "";
        foreach (char c in json)
        {
            if (c != '\n')
            {
                row += c;
            }
            else
            {
                if (row == "")
                    break;
                Building building = JsonUtility.FromJson<Building>(row);
                Tile tile = new Tile();
                tile.sprite = sprites[building.idSprite];
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(building.Position.x, building.Position.y, 0), tile);

                switch (building.type)
                {
                    case Building.Type.Factory:
                        buildings.Add(JsonUtility.FromJson<Factory>(row));
                        break;
                    case Building.Type.MiningCamp:
                        buildings.Add(JsonUtility.FromJson<MiningCamp>(row));
                        break;
                }

                row = "";
            }
        }
        return buildings;
    }
    public void Load()
    {
        string json = StreamManager.readTextFile("build.data");
        this._buildings = new List<Building>();
        _buildings = Import(json);
    }
}
