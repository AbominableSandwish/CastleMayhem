using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {

        }
    }

    public void OpenCloseMenu(bool isOpen)
    {
        this.isOpen = isOpen;
        panel.SetActive(isOpen);
    }
}
