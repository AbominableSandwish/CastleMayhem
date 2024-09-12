using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSystem : MonoBehaviour
{
    [SerializeField] Transform target;
    RectTransform rect;
    [SerializeField] RectTransform CanvasRect;
    float timeToRefresh = 0.5f;
    BuildingSystem buildingSystem;
    Button button;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
        rect.anchoredPosition = screenPoint - CanvasRect.sizeDelta / 2f + new Vector2(0, 50);
        buildingSystem = GameObject.FindAnyObjectByType<BuildingSystem>();

        button = GetComponent<Button>();
        image = GetComponent<Image>();

        if (buildingSystem.GetCanComplete(target.gameObject))
        {
            button.enabled = true;
            image.enabled = true;
        }
        else
        {
            button.enabled = false;
            image.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        timeToRefresh -= Time.deltaTime;
        if(timeToRefresh <= 0.0f)
        {
            if (buildingSystem.GetCanComplete(target.gameObject))
            {
                button.enabled = true;
                image.enabled = true;
            }
            else
            {
                button.enabled = false;
                image.enabled = false;
            }
            timeToRefresh = 1.0f;
        }
    }

}
