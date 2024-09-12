using UnityEngine;
using UnityEngine.Tilemaps;



public class TileManager : MonoBehaviour
{
    Tilemap tilemap;
    [SerializeField] MapManager mapManager;
    [SerializeField] Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
            
        Vector3Int[] positions = new Vector3Int[20 * 20];
        TileBase[] tiles = new TileBase[20 * 20];

        Cell[,] map = mapManager.GetMap();
        int count = 0;
        for(int i = 0; i < 20; i++) {
            for (int j = 0; j < 20; j++)
            {
                positions[count] = new Vector3Int(i, j, 0);
               
                Tile tile = new Tile();

                if (map[i, j].IsBuildable)
                {
                    //Dirt
                    tile.sprite = sprites[0];    
                }
                if (!map[i, j].IsBuildable)
                {
                    //Rock
                    tile.sprite = sprites[1];
                }

                tiles[count] = tile;
                count++;
            }
        }

        tilemap.SetTiles(positions, tiles);
    }
}
