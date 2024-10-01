using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ViewSystem : MonoBehaviour
{
    BuildingSystem buildingSystem;
    [SerializeField] GameObject prefab;
    int nbrButton = 32;

    [SerializeField] private Queue<Button> freeButtons;

    [SerializeField] RectTransform CanvasRect;
    float timeToRefresh = 0.5f;

    [SerializeField] List<Sprite> sprites; 
    // Start is called before the first frame update
    void Start()
    {
        freeButtons = new Queue<Button>();

        for (int i = 0; i < nbrButton; i++) {
            prefab.name = "Button_" + i.ToString();
            GameObject gameObject = GameObject.Instantiate(prefab, CanvasRect);
            gameObject.transform.position = new Vector3(0, -100, 0);
            freeButtons.Enqueue(gameObject.GetComponent<Button>());
        }

        buildingSystem = GetComponent<BuildingSystem>();
    }

    private void FixedUpdate()
    {
        timeToRefresh -= Time.deltaTime;
        if(timeToRefresh <= 0.0f)
        {
            foreach(Building building in buildingSystem.Buildings)
            {
                if (building.button == null)
                {
                    if (building.CanComplete)
                    {
                        Button btn = freeButtons.Dequeue();
                        if(building.type == Building.Type.Factory)
                        {
                            btn.GetComponent<Image>().sprite = sprites[0];
                        }
                        if (building.type == Building.Type.MiningCamp)
                        {
                            btn.GetComponent<Image>().sprite = sprites[1];
                        }
                        btn.enabled = true;
                        btn.image.enabled = true;
                        building.button = btn.gameObject;
                        buildingSystem.SetButton(btn, building);

                        Vector2Int posCell = building.Position;
                        Point pos = new Point();
                        //pos.X = (int)Mathf.Sqrt((2 * posCell.x + 2 * posCell.y) * (posCell.x + 3 * posCell.y));
                        //pos.Y = (int)Mathf.Sqrt((posCell.x - posCell.y) * (3 * posCell.x - posCell.y));

                        pos.X = 2 * posCell.x + posCell.y * 2;
                        pos.Y = posCell.x - posCell.y;

                        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(pos.Y / 2.0f, pos.X / 8.0f - 5));
                        btn.GetComponent<RectTransform>().anchoredPosition = screenPoint - CanvasRect.sizeDelta / 2f + new Vector2(0, 50);
                    }
                }
            }
            timeToRefresh = 1.0f;
        }
    }

    public void RemoveBtn(Building building)
    {
        Button btn = building.button.GetComponent<Button>();
        btn.enabled = false;
        btn.image.enabled = false;
        building.button = null;
        freeButtons.Enqueue(btn);
    }

}
