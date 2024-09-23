using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct Cell
{
    bool isBuildable;
    public bool IsBuildable { get => isBuildable; set => isBuildable = value; }
}
public class MapManager : MonoBehaviour
{
    [DllImport("user32.dll")] public static extern bool SetCursorPos(int X, int Y);

    bool isSetting = false;
    Cell[,] map;
    const int height = 20;

    [SerializeField] GameObject prefabTile;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Sprite[] sprites;

    int widthCell;
    int heightCell;

    struct Cursor
    {
        public int x, y;

    }
    Vector2 offsetMouse;
    Cursor _cursor;
    // Start is called before the first frame update
    void Awake()
    {
        map = new Cell[height, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                float rdm = Random.value;
                if (rdm <= 0.75f)
                {
                    //Dirt
                    map[i,j].IsBuildable = true;
                }
                if (rdm > 0.75f)
                {
                    //Rock
                    map[i,j].IsBuildable = false;
                }
            }
        }
    }
    private void Start()
    {
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSetting)
        {
            Vector2 mousePos = GameObject.Find("Cursor").GetComponent<CursorView>().GetPosition() * 2f;
        
            Point pos = new Point();
            pos.X = ((int)mousePos.x - 1) / 2 + ((int)mousePos.y - 1) * 1 + height / 2;
            pos.Y = ((int)mousePos.y - 1) * 1 + ((int)mousePos.x - 1) / (-2) + height / 2;

            Refresh();

            for (int i = 0; i < _cursor.x; i++)
            {
                for (int j = 0; j < _cursor.y; j++)
                {
                    if (pos.X + i >= 0 && pos.X + i < height
                     && pos.Y + j >= 0 && pos.Y + j < height)
                    {
                        Tile tile = new Tile();
                        if (map[pos.X + i, pos.Y + j].IsBuildable)
                        {
                            //Dirt
                            tile.sprite = sprites[0];
                        }
                        if (!map[pos.X + i, pos.Y + j].IsBuildable)
                        {
                            //Rock
                            tile.sprite = sprites[1];
                        }
                        tilemap.SetTile(new Vector3Int(pos.X + i, pos.Y + j, 0), tile);
                    }
                }
            }

            if (Input.GetButton("Fire1"))
            {
                //Show Panel
                RectTransform rect = GameObject.Find("PanelChoice").GetComponent<RectTransform>();

                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

                rect.anchoredPosition = screenPoint - GameObject.FindFirstObjectByType<Canvas>().GetComponent<RectTransform>().sizeDelta / 2f + new Vector2(0, 24);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                CancelBuilding();
            }

        }
    }

    public void SetTile(int x, int y, Cell cell)
    {
        this.map[x, y] = cell;
    }

    public Cell[,] GetMap()
    {
        return map;
    }


    public void SetX(int x)
    {
        this._cursor.x = x;
    }

    public void SetY(int y)
    {
        this._cursor.y = y;
    }


    public void NewBuilding()
    {

        widthCell = (int)(64.0f / Screen.currentResolution.width * Screen.width);
        heightCell = (int)(32.0f / Screen.currentResolution.height * Screen.height);

        SetCursorPos(Screen.mainWindowPosition.x + Screen.width / 2, Screen.mainWindowPosition.y + Screen.height / 2);
        offsetMouse = (Vector2)Input.mousePosition - Screen.mainWindowPosition;
        //UnityEngine.Cursor.visible = false;
        // Confines the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        Debug.Log(Input.mousePositionDelta);

        isSetting = true;

        for (int i = 0; i < _cursor.x; i++)
        {
            for (int j = 0; j < _cursor.y; j++)
            {
                Tile tile = new Tile();
                tile.sprite = sprites[1];
                tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
    }

    void Refresh()
    {
        Vector3Int[] positions = new Vector3Int[20 * 20];
        TileBase[] tiles = new TileBase[20 * 20];

        int count = 0;
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                positions[count] = new Vector3Int(i, j, 0);
                Tile tile = new Tile();
                tiles[count] = tile;
                count++;
            }
        }
        tilemap.SetTiles(positions, tiles);
    }

    public void CancelBuilding()
    {
        isSetting = false;
        UnityEngine.Cursor.visible = true;
        // Releases the cursor
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Refresh();
        SetX(0);
        SetY(0);
    }
}
