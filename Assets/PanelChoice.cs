using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelChoice : MonoBehaviour
{
    public enum Type
    {
        None,
        New,
        Remove,
        Upgrade
    }
    Type type;
    public void SetAction(Type type, Vector3 position)
    {
        this.type = type;
        GetComponent<RectTransform>().position = position;
    }
    public void Confirm()
    {
        FindAnyObjectByType<BuildingSystem>().ActionConfirmed(type);    
        GetComponent<RectTransform>().position = new Vector3(0, -350, 0);
        type = Type.None;
    }

    public void Cancel()
    {
        type = Type.None;
    }
}
