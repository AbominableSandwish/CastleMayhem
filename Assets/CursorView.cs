using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CursorView : MonoBehaviour
{
    [DllImport("user32.dll")] public static extern bool SetCursorPos(int X, int Y);
    [SerializeField] private Vector3 offset = new Vector3(0.16f, 0.16f);
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Vector3Int newPos = new Vector3Int(Screen.mainWindowPosition.x + Screen.width / 2, Screen.mainWindowPosition.y +Screen.height / 2);
        SetCursorPos(newPos.x, newPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -cam.transform.position.z) - offset);
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
