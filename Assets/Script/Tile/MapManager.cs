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
        map = new Cell[20,20];

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
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
            Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.mainWindowPosition.x, Input.mousePosition.y - Screen.mainWindowPosition.y) - offsetMouse;
            Vector3Int cellPos = new Vector3Int((int)(mousePos.y / heightCell) / 2 + (int)(mousePos.x / widthCell) / 2 + 9, (int)(mousePos.y / heightCell) / 2 - (int)(mousePos.x / widthCell) / 2 + 9);
            Debug.Log(cellPos);

            Refresh();

            for (int i = 0; i < _cursor.x; i++)
            {
                for (int j = 0; j < _cursor.y; j++)
                {
                    if (cellPos.x + i >= 0 && cellPos.x + i < 20
                     && cellPos.y + j >= 0 && cellPos.y + j < 20)
                    {
                        Tile tile = new Tile();
                        if (map[cellPos.x + i, cellPos.y + j].IsBuildable)
                        {
                            //Dirt
                            tile.sprite = sprites[0];
                        }
                        if (!map[cellPos.x + i, cellPos.y + j].IsBuildable)
                        {
                            //Rock
                            tile.sprite = sprites[1];
                        }
                        tilemap.SetTile(new Vector3Int(cellPos.x + i, cellPos.y + j, 0), tile);
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
