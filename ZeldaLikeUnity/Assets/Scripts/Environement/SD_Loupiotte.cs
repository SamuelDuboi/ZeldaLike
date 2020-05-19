using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_Loupiotte : MonoBehaviour
{
    public GameObject camera;
    public GameObject playerCam;
    bool move;
    bool OpenDoor;
    bool OpenDoor2;
    bool closeDoor;
    bool closeDoor2;
    float timer;
    [Range(1,20)]
    public float timeToReach;
    bool moveBack;
    int cpt;

    public List<GameObject> loupiottes = new List<GameObject>();
 [HideInInspector]  public int loupiottesCount;


    Animator doorAnimator;
    public bool Open;
    public bool stayOpen;
    float distance;
    public float timeCameraMoving;
    private void Start()
    {
        loupiottesCount = 0;
       
        doorAnimator = GetComponent<Animator>();
        doorAnimator.SetBool("Oppen", Open);
        timer = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && !closeDoor  && loupiottesCount == loupiottes.Count - 1)
        {
            if (SD_PlayerMovement.Instance.grosPoussière.activeSelf)
                SD_PlayerMovement.Instance.grosPoussière.SetActive(false);
            Time.timeScale = 0;
            moveBack = false;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerAttack.Instance.cantAim = true;
            camera.SetActive(true);
            playerCam.SetActive(false);
            camera.transform.position = new Vector3(SD_PlayerMovement.Instance.transform.position.x, SD_PlayerMovement.Instance.transform.position.y, -10);
            move = true;
            cpt = 0;
            timer = 1;
            closeDoor = true;
            distance = Mathf.Abs( Vector2.Distance(new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, -10), camera.transform.position));

        }

    }
      

    void Activation()
    {
        doorAnimator.SetTrigger("Activated");
        doorAnimator.speed = 10f;
    }
    IEnumerator WaitToComeBack()
    {
        timer = 1;
        yield return new WaitForSeconds(timeToReach);
        if (!stayOpen)
            doorAnimator.SetTrigger("ComeBack");
        else
            closeDoor = true;


    }
    private void Update()
    {
        if (move)
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                            new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, -10),
                                                            distance/timeCameraMoving);
        if (timer == 0)
        {
            StartCoroutine(WaitToComeBack());

        }
        if (Mathf.Abs(camera.transform.position.x - transform.GetChild(0).position.x) < 0.1f && cpt == 0)
        {
            Time.timeScale = 0.1f;
            move = false;
            Activation();
            cpt++;
        }

        if (moveBack && camera.activeInHierarchy)
        {
            Time.timeScale = 0;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                           playerCam.transform.GetChild(0).position, distance / timeCameraMoving);
            if (Mathf.Abs(camera.transform.position.x - playerCam.transform.GetChild(0).position.x) < 0.1f && moveBack)
            {
                moveBack = false;
                Time.timeScale = 1;
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                SD_PlayerAttack.Instance.cantAttack = false;
                SD_PlayerAttack.Instance.cantAim = false;
                camera.SetActive(false);
                playerCam.SetActive(true);
                Time.timeScale = 1;
                timer = 0;
            }

        }

    }
    public void MoveBack()
    {
        doorAnimator.speed = 1f;
        moveBack = true;
    }
    public void ForOppening()
    {
        if (Open)
        {
            if (!stayOpen)
            {
                loupiottes[loupiottesCount].GetComponent<SD_LoupiotteActivated>().activated = false;
                loupiottes[loupiottesCount].GetComponent<Animator>().SetTrigger("Off");
                loupiottes[loupiottesCount].GetComponent<SD_LoupiotteActivated>().shield.SetActive(false);
                loupiottes[0].GetComponent<SD_LoupiotteActivated>().activated = true;
                loupiottes[0].GetComponent<Animator>().SetTrigger("On");
                loupiottes[0].GetComponent<SD_LoupiotteActivated>().shield.SetActive(true);
            }
            closeDoor = false;
        }
    }
    public void ForClosing()
    {
        if (!Open)
        {
            if (!stayOpen)
            {
                loupiottes[loupiottesCount].GetComponent<SD_LoupiotteActivated>().activated = false;
                loupiottes[loupiottesCount].GetComponent<Animator>().SetTrigger("Off");
                loupiottes[0].GetComponent<SD_LoupiotteActivated>().activated = true;
                loupiottes[0].GetComponent<Animator>().SetTrigger("Activated");
            }
            closeDoor = false;
        }
    }

    public void Triggered(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && !closeDoor  && loupiottesCount == loupiottes.Count - 1)
        {
            if (SD_PlayerMovement.Instance.grosPoussière.activeSelf)
                SD_PlayerMovement.Instance.grosPoussière.SetActive(false);
            Time.timeScale = 0;
            moveBack = false;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerAttack.Instance.cantAim = true;
            camera.SetActive(true);
            playerCam.SetActive(false);
            camera.transform.position = new Vector3(SD_PlayerMovement.Instance.transform.position.x, SD_PlayerMovement.Instance.transform.position.y, -10);
            move = true;
            cpt = 0;
            loupiottesCount = 0;
            timer = 1;
            closeDoor = true;
            foreach (GameObject loupiots in loupiottes)
            {
                loupiots.GetComponent<Animator>().SetTrigger("On");
                loupiots.GetComponent<Animator>().SetTrigger("Activated");

            }
            distance = Mathf.Abs(Vector2.Distance(new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, -10), camera.transform.position))*0.1f;
        }
       else if (collision.gameObject.layer == 14 && loupiottes.Count > 1)
        {
            float i = -1;
            foreach (GameObject loupiots in loupiottes)
            {
                i++;
                if (loupiots.GetComponent<SD_LoupiotteActivated>().isTriggered &&
                    loupiots.GetComponent<SD_LoupiotteActivated>().activated)
                {
                    loupiottesCount ++;
                    break;
                }
            }
            if (i > -1)
            {
                loupiottes[(int)i].GetComponent<SD_LoupiotteActivated>().activated = false;
                loupiottes[(int)i].GetComponent<Animator>().SetTrigger("On");
                loupiottes[(int)i + 1].GetComponent<SD_LoupiotteActivated>().activated = true;
                loupiottes[(int)i + 1].GetComponent<Animator>().SetTrigger("Activated");
            }
        }
    }
}
