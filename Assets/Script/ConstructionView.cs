using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;

public class ConstructionView : MonoBehaviour
{
    Sprite ConstructSprite;
    BuildingZone building;

    float timer;

    int nbrConstruct = 16 * 8;
    float timeToConstruct;

    Vector2 size;

    public void SetConstructionView(Sprite sprite, BuildingZone building)
    {
        this.ConstructSprite = sprite;
        this.building = building;

        size = building.GetSize();

        timeToConstruct = (this.building.TimeToBuild - 5) / (float)(nbrConstruct * size.x * size.y);

        Point worldPos = new Point();

        worldPos.X = 2 * building.position.x + building.position.y * 2;
        worldPos.Y = building.position.x - building.position.y;

     
        this.worldpos = new Vector3(worldPos.Y / 2.0f, worldPos.X / 8.0f - 5);
        this.currentPos = this.worldpos;

     
    }

    Vector3 worldpos;
    Vector2 posConstruct;
    Vector3 currentPos;

    enum Dir
    {
        right,
        top,
        left,
        bottom
    }
    Dir dir = Dir.right;

    int level = 0;

    int counter = 1;

    float dist = 0.2325f / 4.0f;

    bool isConstruc = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isConstruc)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                GameObject Construct = new GameObject();
                Construct.transform.parent = this.transform;
              
                Construct.transform.localPosition = this.currentPos;
                SpriteRenderer render = Construct.AddComponent<SpriteRenderer>();
                render.sprite = this.ConstructSprite;

                switch (dir)
                {
                    case Dir.right:
                        posConstruct.x++;
                        currentPos.x = worldpos.x + 2.0f * posConstruct.x * dist + dist * posConstruct.y * 2.0f;
                        currentPos.y = worldpos.y + posConstruct.x * dist - posConstruct.y * dist + level * 0.2325f / 3;
                        render.sortingOrder = 400 + level - counter;
                        if (counter >= 0 + 4 * size.x)
                        {
                            dir = Dir.top;
                           
                        }
                        break;
                    case Dir.top:
                        posConstruct.y--;
                        currentPos.x = worldpos.x + 2.0f * posConstruct.x * dist + dist * posConstruct.y * 2.0f;
                        currentPos.y = worldpos.y + posConstruct.x * dist - posConstruct.y * dist + level * 0.2325f / 3;
                        render.sortingOrder = 200 + level - counter;
                        if (counter >= 4 * size.x + 4 * size.y)
                        {
                            dir = Dir.left;
                        }
                        break;
                    case Dir.left:
                        posConstruct.x--;
                        currentPos.x = worldpos.x + 2.0f * posConstruct.x * dist + dist * posConstruct.y * 2.0f;
                        currentPos.y = worldpos.y + posConstruct.x * dist - posConstruct.y * dist + level * 0.2325f / 3;
                        render.sortingOrder = 100 + level + counter;
                        if (counter >= 4 * size.x + 4 * size.y + 4 * size.x)
                        {
                            dir = Dir.bottom;
                        }
                        break;
                    case Dir.bottom:
                        if (counter >= 4 * size.x + 4 * size.y + 4 * size.x + 4 * size.y)
                        {
                            level++;
                        }
                        posConstruct.y++;
                        currentPos.x = worldpos.x + 2.0f * posConstruct.x * dist + dist * posConstruct.y * 2.0f;
                        currentPos.y = worldpos.y + posConstruct.x * dist - posConstruct.y * dist + level * 0.2325f / 3;
                        render.sortingOrder = 300 + counter * 2 + level;
                        if (counter >= 4 * size.x + 4 * size.y + 4 * size.x + 4 * size.y)
                        {
                            dir = Dir.right;
                            counter = 0;

                            if (building.CanComplete)
                            {
                                isConstruc = false;
                            }

                        }
                        break;
                }
                counter++;
                timer = timeToConstruct;
            }
        }
    }
}
