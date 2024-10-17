using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelView : MonoBehaviour
{
    Building target;
    bool isOpen = false;
    bool isSet = false;

    Queue<Button> buttonsFree;
    List<Button> buttonsUsed;

    InformationView information;
    BuildingSystem buildingsSystem;

    float velocity = 600.0f;
    float MinY = -150.0f;
    // Start is called before the first frame update
    void Start()
    {
        buttonsFree = new Queue<Button>();
        buttonsUsed = new List<Button>();

        Button[] btns = GetComponentsInChildren<Button>();
        foreach (Button btn in btns)
        {
            buttonsFree.Enqueue(btn);
        }

        information = FindFirstObjectByType<InformationView>();
        buildingsSystem = FindFirstObjectByType<BuildingSystem>();
    }

    enum Action
    {
        Opening,
        Closing,
        Waiting
    }

    Action action = Action.Waiting;

    // Update is called once per frame
    void Update()
    {
        switch (action)
        {
            case Action.Opening:
                if (!isSet)
                {
                    Open();
                    break;
                }
                // View
                bool isReady = true;
                foreach (Button btn in buttonsUsed)
                {
                    if (btn.transform.localPosition.y < 0)
                    {
                        btn.transform.localPosition = btn.transform.localPosition + Vector3.up * velocity * Time.deltaTime;
                        isReady = false;

                        Color color = btn.GetComponent<Image>().color;
                        float norm = (btn.transform.localPosition.y + 100) / 100.0f;
                        btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f * norm);  
                    }
                    else
                    {
                        btn.transform.localPosition = new Vector3(btn.transform.localPosition.x, 0, btn.transform.localPosition.z);
                    }
                }

                if(!isReady)
                    break;

                isOpen = true;
                action = Action.Waiting;

                break;
            case Action.Closing:
           
                bool isFinish = true;
                foreach (Button btn in buttonsUsed)
                {
                   if(btn.transform.localPosition.y > MinY)
                   {
                       btn.transform.localPosition = btn.transform.localPosition + Vector3.down * velocity * Time.deltaTime;
                       isFinish = false;

                       Color color = btn.GetComponent<Image>().color;
                       float norm = (btn.transform.localPosition.y + 100) / 100.0f;
                       btn.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1.0f * norm);
                   }
                   else
                   {
                        btn.transform.localPosition = new Vector3 (btn.transform.localPosition.x, MinY, btn.transform.localPosition.z);
                   }

                   
                }

                if(!isFinish)
                    break;

                Close();
                isOpen = false;
                action = Action.Waiting;
                break;

            case Action.Waiting:
                if(target != null)
                {
                    action = Action.Opening;
                }
                break;
        }
    }

    void Open()
    {
        //Set all Buttons

        //First Button Information
        Button btn = buttonsFree.Dequeue();
        btn.GetComponentInChildren<TextMeshProUGUI>().text = "Info";
        SetButtonInfo(btn, target);
        buttonsUsed.Add(btn);

        if (target.CanUpgrade)
        {
            btn = buttonsFree.Dequeue();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade";
            SetButtonUprade(btn, target);
            buttonsUsed[0].transform.localPosition = new Vector3(-50, buttonsUsed[0].transform.localPosition.y, 0);
            btn.transform.localPosition = new Vector3(50, buttonsUsed[0].transform.localPosition.y, 0);
            buttonsUsed.Add(btn);
            
        }


        isSet = true;
    }

    void Close()
    {
        foreach (Button btn in buttonsUsed)
        {
            btn.transform.localPosition = new Vector3(0, btn.transform.localPosition.y);
            btn.onClick.RemoveAllListeners();
            buttonsFree.Enqueue(btn);
        }
        buttonsUsed.Clear();

        isSet = false;
    }

    public void SetButtonInfo(UnityEngine.UI.Button button, Building building)
    {
        button.onClick.AddListener(delegate { information.Show(building); });
    }

    public void SetButtonUprade(UnityEngine.UI.Button button, Building building)
    {
        button.onClick.AddListener(delegate { buildingsSystem.Upgrade(building); });
    }

    public void ShowPanel(Building building) {
        //If is already Open
        if (building == target)
            return;

        if (isOpen)
        {
            HidePanel();
        }

        this.target = building;
    }

    public void HidePanel() {
        if (isOpen)
        {
            this.target = null;
            action = Action.Closing;
        }
    }
}
