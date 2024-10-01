using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    private List<Building> _buildings;
    RessourceSystem _sourceSystem;
    Sprite[] sprites;
    [SerializeField] Tilemap tilemap;
    MapManager mapManager;

    Building newBuilding;

    const int height = 20;
    bool isPreview = false;
    int idSprite = -1;
    Vector3Int lastpos;

    Vector3 offset = new Vector3(0, 1f);

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
        this._buildings = new List<Building>();
        mapManager = GetComponent<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        if (_buildings !=  null)
        {
            foreach (Building building in _buildings)
            {
                building.Update(dt);
            }
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
                    tile.sprite = sprites[idSprite];
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
        switch ((Building.Type)idSprite)
        {
            case Building.Type.Factory:
                GameObject factory = new GameObject();
                factory.name = "Factory_";
                Tile tile = new Tile();
                tile.sprite = sprites[idSprite];
                GameObject.Find("Building").GetComponent<Tilemap>().SetTile(new Vector3Int(lastpos.x, lastpos.y, 0), tile);
                this.newBuilding = new Factory(new Vector2Int(lastpos.x, lastpos.y));
                this._buildings.Add(this.newBuilding);
                _sourceSystem.ReduceRessource(new RessourceSystem.Package(RessourceSystem.Type.A, Factory.COSTMATERIAL1));
                break;
            case Building.Type.MiningCamp: break;
        }
    }

    public void SetButton(UnityEngine.UI.Button button, Building building)
    {
        button.onClick.AddListener(delegate {this.Complete(building);});
    }


    public void Complete(Building building)
    {
        building.button.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        _sourceSystem.AddRessource(building.Complete());
        GetComponent<ViewSystem>().RemoveBtn(building);
    }
}
