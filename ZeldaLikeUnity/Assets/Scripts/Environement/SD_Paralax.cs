using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Paralax : MonoBehaviour
{
    GameObject camera;
    [Range(0,1)]
    public float paralaxSpeed;
    Vector2 lasteCamera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.gameObject;
        lasteCamera = camera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = new Vector3(camera.transform.position.x - lasteCamera.x, camera.transform.position.y - lasteCamera.y,0);
        transform.position += Vector3.right* (deltaPosition.x * paralaxSpeed);
        transform.position += Vector3.up* (deltaPosition.y * paralaxSpeed);
        lasteCamera = camera.transform.position;
        
    }
}
