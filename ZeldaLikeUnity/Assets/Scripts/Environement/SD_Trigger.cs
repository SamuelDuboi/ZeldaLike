using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_Trigger : MonoBehaviour
{
    public GameObject camera;
    public GameObject playerCam;
    bool move;
    bool OpenDoor;
    bool closeDoor;
    float timer;
    [Range(1, 100)]
    public float timeToReach;
    bool moveBack;
    int cpt;
    GameObject interactButton;
    Animator doorAnimator;
    public bool Open;
    private void Start()
    { 

        interactButton = transform.GetChild(1).gameObject;
        interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
        interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                        interactButton.transform.parent.position.y + 1);
        doorAnimator = GetComponent<Animator>();
        doorAnimator.SetBool("Oppen", Open);
        timer = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!closeDoor)
        interactButton.SetActive(true);
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Interact") && !closeDoor)
        {
            Time.timeScale = 0;
            timer =1;
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
            closeDoor = true;
            interactButton.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactButton.SetActive(false);
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
        doorAnimator.SetTrigger("ComeBack");


    }
    private void Update()
    {
        if (move)
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                            new Vector3(transform.GetChild(0).position.x, transform.GetChild(0).position.y, -10),
                                                            0.05f);
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
                                                           playerCam.transform.GetChild(0).position, 0.05f);
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
            closeDoor = false;
    }
    public void ForClosing()
    {
        if (!Open)
            closeDoor = false;
    }
}
