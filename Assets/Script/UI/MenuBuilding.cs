using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MenuBuilding : MonoBehaviour
{
    Animator _anim;
    bool _isOpen = false;
    InputSystem input;

    [SerializeField] private GameObject prefab;
    RessourceSystem ressourceSystem;
    BuildingSystem buildingSystem;
    MapManager mapManager;
    List<GameObject> buttons;
    // Start is called before the first frame update
    void Start()
    {
        ressourceSystem = GameObject.FindAnyObjectByType<RessourceSystem>();
        buildingSystem = GameObject.FindAnyObjectByType<BuildingSystem>();
        mapManager = GameObject.FindAnyObjectByType<MapManager>();
        _anim = GetComponent<Animator>();
        input =  GameObject.FindFirstObjectByType<InputSystem>();

        buttons = new List<GameObject>();
        int i = 0;
        
        foreach (Building.Type building in Enum.GetValues(typeof(Building.Type)))
        {
            if (building != Building.Type.InContruct)
            {
                GameObject btn = Instantiate(prefab, this.transform);
                btn.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, i * (-75) - 100, 0);

                string name = "";
                int costResourceA = 0;
                int costResourceB = 0;

                int xSize = 0;
                int ySize = 0;
                switch (building)
                {
                    case Building.Type.Factory:
                        name = building.ToString();
                        costResourceA = Factory.COSTMATERIAL1;
                        costResourceB = Factory.COSTMATERIAL2;
                        break;

                    case Building.Type.MiningCamp:
                        name = building.ToString();
                        costResourceA = MiningCamp.COSTMATERIAL1;
                        costResourceB = MiningCamp.COSTMATERIAL2;
                        break;
                }
                btn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = name;
                btn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = costResourceA.ToString();
                btn.GetComponent<Button>().onClick.AddListener(delegate { mapManager.SetX(xSize); });
                btn.GetComponent<Button>().onClick.AddListener(delegate { mapManager.SetY(ySize); });
                btn.GetComponent<Button>().onClick.AddListener(delegate { mapManager.NewBuilding((int)building); });
                btn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = costResourceB.ToString();
                buttons.Add(btn);
                i++;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isOpen)
        {
            if (input.ActionFree(InputSystem.Action.Cancel))
            {
                CloseMenu();
            }
        }

        foreach (GameObject btn in buttons)
        {
            bool CanConstruct = true;
            if (ressourceSystem.RessourceA < int.Parse(btn.GetComponentsInChildren<TextMeshProUGUI>()[1].text))
            {
                btn.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.red;
                CanConstruct = false;
            }
            else
            {
                btn.GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.white;
            }

            if (ressourceSystem.RessourceB < int.Parse(btn.GetComponentsInChildren<TextMeshProUGUI>()[2].text))
            {
                btn.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.red;
                CanConstruct = false;
            }
            else
            {
                btn.GetComponentsInChildren<TextMeshProUGUI>()[2].color = Color.white;
            }


            btn.GetComponent<UnityEngine.UI.Button>().enabled = CanConstruct;


        }

    }

    public void OpenMenu()
    {
        _anim.SetTrigger("isOpen");
        _isOpen = true;

        
    }

    public void CloseMenu()
    {
        _anim.SetTrigger("isClose");
        _isOpen = false;
    }

}
