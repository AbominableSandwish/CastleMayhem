using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

public class BuildingSystem : MonoBehaviour
{
    private List<Building> _buildings;
    RessourceSystem _sourceSystem;
    Sprite[] sprites;
    [SerializeField] Tilemap tilemap;

    const int height = 20;
    bool isPreview = false;
    int idSprite = -1;
    Vector3Int lastpos;

    Vector3 offset = new Vector3(0, 1f);

    // Start is called before the first frame update
    void Awake()
    {
        _sourceSystem = GetComponent<RessourceSystem>();

        _buildings = new List<Building>();

        sprites = Resources.LoadAll<Sprite>("Sprites/BuildingSprite");
        
    }

    private void Start()
    {
        
        foreach (Building building in _buildings)
        {
            SpriteRenderer renderer = building.obj.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = sprites[building.idSprite];
        }
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

    void Refresh()
    {
        Tile tile = new Tile();
        tilemap.SetTile(lastpos, tile);
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

    public void Preview(int idSprite)
    {
        tilemap.color = new UnityEngine.Color(1, 1, 1, 0.5f);
        isPreview = true;

        this.idSprite = idSprite;
    }

    public void CancelPreview()
    {
        isPreview = true;
        this.idSprite = -1;
    }
}
