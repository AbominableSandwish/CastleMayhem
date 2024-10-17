using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject prefabInfo;
    [SerializeField] private Transform CharPanel;

    [SerializeField] private Sprite health;
    [SerializeField] private Sprite Coin;
    [SerializeField] private Sprite Gem;
    [SerializeField] private Sprite Capacity;

    List<GameObject> listInfo;

    Building target;

    public void Update()
    {
        if (listInfo != null)
        {
            foreach (GameObject info in listInfo)
            {
                TextMeshProUGUI[] texts = info.GetComponentsInChildren<TextMeshProUGUI>();
                if (texts[0].text == "Capacity:")
                {
                    texts[1].text = (target.GetCapacity().ToString() +  '/' + target.GetMaximumStorageQuantity()).ToString();
                }

            }
        }
    }

    public void Show(Building building)
    {

        FindFirstObjectByType<InputSystem>().ResetClick();
        target = building;
        panel.SetActive(true);

        GetComponentsInChildren<TextMeshProUGUI>()[0].text = building.type.ToString();
        GetComponentsInChildren<TextMeshProUGUI>()[1].text = "level " + building.level.ToString();
        GetComponentsInChildren<Image>()[4].sprite = building.sprite;

        listInfo = new List<GameObject>();
        //health
        GameObject Info = Instantiate(prefabInfo, CharPanel);
        Info.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        Info.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
     
        int positionX = 75;
        if(listInfo.Count + 1 % 2 == 1)
        {
            positionX = -75;
        }
        Info.transform.localPosition = new Vector3(positionX, ((listInfo.Count - 1) / 2) * -50 + 100);
        listInfo.Add(Info);
        Image[] images = Info.GetComponentsInChildren<Image>();
        images[1].sprite = health;
        images[2].enabled = false;
        TextMeshProUGUI[] texts = Info.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = "Health :";
        texts[1].text = building.Health.ToString();
       

        if(building.type == Building.Type.Factory || building.type == Building.Type.MiningCamp)
        {
            Info = Instantiate(prefabInfo, CharPanel, false);
            listInfo.Add(Info);
            positionX = 75;
            if (listInfo.Count + 1 % 2 == 1)
            {
                positionX = -75;
            }
            Info.transform.localPosition = new Vector3(positionX, ((listInfo.Count - 1) / 2) * -50 + 100);
            images = Info.GetComponentsInChildren<Image>();

            images[2].enabled = false;
            texts = Info.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "Productivity:";
            texts[1].text = (target.GetproductionQuantityPerHour()).ToString();

            switch (building.type)
            {
                case Building.Type.Factory:
                    images[1].sprite = Coin;
                    break;

                case Building.Type.MiningCamp:
                    images[1].sprite = Gem;                 
                    break;
            }

            Info = Instantiate(prefabInfo, CharPanel, false);
            listInfo.Add(Info);
            positionX = 75;
            if (listInfo.Count + 1 % 2 == 1)
            {
                positionX = -75;
            }
            Info.transform.localPosition = new Vector3(positionX, ((listInfo.Count - 1) / 2) * -50 + 100);
            images = Info.GetComponentsInChildren<Image>();
            images[1].sprite = Capacity;


            switch (building.type)
            {
                case Building.Type.Factory:
                    images[2].sprite = Coin;
                    break;

                case Building.Type.MiningCamp:
                    images[2].sprite = Gem;
                    break;
            }

            texts = Info.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "Capacity:";
            texts[1].text = (target.GetMaximumStorageQuantity()).ToString();
        }

      
    }

    public void Hide()
    {
        FindFirstObjectByType<InputSystem>().ResetClick();
        if(listInfo!= null)
            if (listInfo.Count != 0) {

                foreach (GameObject obj in listInfo)
                {
                    Destroy(obj);
                }
                listInfo.Clear();
            }

        target = null;
        panel.SetActive(false);
    }
}
