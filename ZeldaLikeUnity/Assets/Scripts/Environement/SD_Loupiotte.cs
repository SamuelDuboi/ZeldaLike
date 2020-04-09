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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && !closeDoor)
        {
            Time.timeScale= 0;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerAttack.Instance.cantAim = true;
            camera.SetActive(true);
            playerCam.SetActive(false);
            camera.transform.position = new Vector3( SD_PlayerMovement.Instance.transform.position.x, SD_PlayerMovement.Instance.transform.position.y,-10);
            move = true;
            cpt = 0;
        }
    }

    private void Update()
    {
        if (move)
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                            new Vector3( transform.GetChild(0).position.x,transform.GetChild(0).position.y,-10),
                                                            0.05f);
        if (Mathf.Abs( camera.transform.position.x - transform.GetChild(0).position.x) < 0.1f)
        {
            move = false;
            if(transform.GetChild(0).localScale.x >= 0.1f)
            {
                OpenDoor = true;
               
            }
            else
            {
                closeDoor = true;
            }
            if (transform.childCount > 1)
            {
                if (transform.GetChild(1).localScale.x >= 0.1f)
                {
                    OpenDoor2 = true;

                }
                else
                {
                    closeDoor2 = true;
                }
            }


        }
        if (OpenDoor)
        {
            if (cpt == 0)
                timer = timeToReach;
            timer += 0.01f;
            if (timer > timeToReach)
            {
                if (transform.GetChild(0).localScale.x >= 0.1f)
                {
                    transform.GetChild(0).position -= new Vector3(0.02f, 0, 0);
                    transform.GetChild(0).localScale -= new Vector3(0.01f, 0, 0);

                }
                else
                {
                    if (cpt == 0)
                    {
                        moveBack = true;
                        timer = 0;
                        closeDoor = true;
                        cpt++;
                    }
                    OpenDoor = false;

                }
            }
        }
        if (closeDoor)
        {
            if (cpt == 0)
                timer = timeToReach;
            timer += 0.01f;
            if (timer > timeToReach)
            {
                if (transform.GetChild(0).localScale.x <= 1.1f)
                {
                    transform.GetChild(0).position += new Vector3(0.02f, 0, 0);
                    transform.GetChild(0).localScale += new Vector3(0.01f, 0, 0);

                }
                else
                {
                    if (cpt == 0)
                    {
                        moveBack = true;
                        timer = 0;
                        OpenDoor = true;
                        cpt++;
                    }
                    closeDoor = false;
                    
                }
                    
            }
        }

        if (OpenDoor2)
        {
          
            if (timer > timeToReach)
            {
                if (transform.GetChild(1).localScale.x >= 0.1f)
                {
                    transform.GetChild(1).position -= new Vector3(0.02f, 0, 0);
                    transform.GetChild(1).localScale -= new Vector3(0.01f, 0, 0);

                }
                else
                {
                    if (cpt == 0)
                    {                     
                        closeDoor2 = true;
                    }
                    OpenDoor2 = false;

                }
            }
        }
        if (closeDoor2)
        {
           
            if (timer > timeToReach)
            {
                if (transform.GetChild(1).localScale.x <= 1.1f)
                {
                    transform.GetChild(1).position += new Vector3(0.02f, 0, 0);
                    transform.GetChild(1).localScale += new Vector3(0.01f, 0, 0);

                }
                else
                {
                    if (cpt == 0)
                    {
                        OpenDoor2 = true;
                    }
                    closeDoor2 = false;

                }

            }
        }
        if (moveBack)
        {
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                           playerCam.transform.GetChild(0).position,
                                                            0.05f);
        }
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
        }
    }
}
