using Management;
using UnityEngine;
using Player;
using UnityEngine.SocialPlatforms;

public class DezoomCam : MonoBehaviour
{
    public GameObject cam;
    public GameObject camPlayer;
    float currentPosition;
    bool up;
    bool down;
    Camera cameraCam;
    [Range(0,10)]
    public float cameraSpeed;
    [Range(5.7f, 10)]
    public float FOVMax;
    bool moveUp;
    bool moveDown;
    public bool goRight;
    public Collider2D left;
    public Collider2D right;
    private void Start()
    {
        cameraCam = cam.GetComponent<Camera>();
    }
    void Update()
    {
        if (up )
        {
            SD_PlayerMovement.Instance.cantSprint = true;
            if (goRight)
            {
                if (cam.transform.position.x > currentPosition && moveUp)
                {
                    cameraCam.orthographicSize += 0.013f * cameraSpeed;
                    currentPosition = cam.transform.position.x;

                }
                else if (cam.transform.position.x < currentPosition && moveDown)
                {
                    cameraCam.orthographicSize -= 0.013f * cameraSpeed;
                    currentPosition = cam.transform.position.x;
                }
            }
            else
            {
                if (cam.transform.position.x < currentPosition && moveUp)
                {
                    cameraCam.orthographicSize += 0.013f * cameraSpeed;
                    currentPosition = cam.transform.position.x;

                }
                else if (cam.transform.position.x > currentPosition && moveDown)
                {
                    cameraCam.orthographicSize -= 0.013f * cameraSpeed;
                    currentPosition = cam.transform.position.x;
                }

            }
            if (cameraCam.orthographicSize < 5.6f)
            {

                SD_PlayerMovement.Instance.cantSprint = false;
                up = false;
                moveDown = false;
                camPlayer.transform.position = new Vector3(SD_PlayerMovement.Instance.transform.position.x,
                                                           SD_PlayerMovement.Instance.transform.position.y,
                                                           -10);
                camPlayer.SetActive(true);
                
            GameManagerV2.Instance.cantPause = false;
                cam.SetActive(false);
                left.enabled = true;
                right.enabled = true;
            }
            else if  (cameraCam.orthographicSize >= FOVMax)
            {
                moveUp = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10 || collision.gameObject.layer == 11)
        {
            if (!up)
            {
                if (goRight)
                {
                    left.enabled = false;
                    right.enabled = true;
                }
                else
                {
                    right.enabled = false;
                    left.enabled = true;
                }

                moveUp = true;
                moveDown = true;
                up = true;
                if (SD_PlayerMovement.Instance.grosPoussière.activeSelf)
                    SD_PlayerMovement.Instance.grosPoussière.SetActive(false);                
                cam.transform.SetParent(collision.transform);
                cam.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -10);
                if (goRight)
                    currentPosition = cam.transform.position.x - 0.01f;
                else
                    currentPosition = cam.transform.position.x + 0.01f;

                cameraCam.orthographicSize = 5.6f;
                cam.SetActive(true);
                GameManagerV2.Instance.cantPause = true;
                camPlayer.SetActive(false);
            }
            else
            {
                if (cam.transform.position.x > currentPosition && goRight || cam.transform.position.x < currentPosition && !goRight)
                {
                    if (!moveUp)
                        moveUp = true;
                    else if (!moveDown)
                        moveDown = false;
                }
                if (goRight)
                {
                    left.enabled = true;
                    right.enabled = false;
                    goRight = false;
                }
                else
                {
                    right.enabled = true;
                    left.enabled = false;
                    goRight = true;
                }

            }
        }

    }
}
