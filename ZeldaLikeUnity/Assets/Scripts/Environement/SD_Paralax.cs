using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Paralax : MonoBehaviour
{
    public GameObject cameraMain;
    [Range(0,1)]
    public float paralaxSpeed;
    Vector2 lasteCamera;
    // Start is called before the first frame update
    void Start()
    {
        lasteCamera = cameraMain.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = new Vector3(cameraMain.transform.position.x - lasteCamera.x, cameraMain.transform.position.y - lasteCamera.y,0);
        transform.position += Vector3.right* (deltaPosition.x * paralaxSpeed);
        transform.position += Vector3.up* (deltaPosition.y * paralaxSpeed);
        lasteCamera = cameraMain.transform.position;
        
    }
}
