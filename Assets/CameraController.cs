
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 origin;
    float ZoomMin, ZoomMax;
    float Zoom;

    float delta = 0;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        delta += Input.mouseScrollDelta.y;
        if(delta  < 0)
        {
            delta = 0; 
        }

        if (delta > 35)
        {
            delta = 35;
        }
        transform.position = origin + new Vector3(0,0, delta / 5.0f);
    }

    
}
