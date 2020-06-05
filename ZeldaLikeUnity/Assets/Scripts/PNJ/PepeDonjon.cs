using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;
public class PepeDonjon : MonoBehaviour
{
    
    public GameObject text;
    int cpt;
    public GameObject camera;
    public GameObject playerCam;
    bool move;
    bool moveBack;
    float distance;
    public float timeCameraMoving = 1;


    private void Update()
    {

        if (move)
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                            new Vector3( transform.position.x, transform.position.y, -10),
                                                            distance / timeCameraMoving);
        if (Mathf.Abs(camera.transform.position.x - transform.position.x) < 0.1f && cpt == 0 && move)
        {
            Time.timeScale = 1f;
            move = false;
            PlayDialogue();

        }
        if (moveBack && camera.activeInHierarchy)
        {
            Time.timeScale = 0;
            camera.transform.position = Vector3.MoveTowards(camera.transform.position,
                                                           playerCam.transform.GetChild(0).position, distance / timeCameraMoving);
            if (Mathf.Abs(camera.transform.position.x - playerCam.transform.GetChild(0).position.x) < 0.1f && moveBack)
            {
                AudioManager.Instance.UnPause();
                moveBack = false;
                Time.timeScale = 1;
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                SD_PlayerAttack.Instance.cantAttack = false;
                SD_PlayerAttack.Instance.cantAim = false;
                camera.SetActive(false);
                playerCam.SetActive(true);
                Time.timeScale = 1;
                Destroy(gameObject);
            }
        }
        if (cpt == 1 && !text.activeInHierarchy)
        {
            moveBack = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            Interact();
        }
    }

    public void PlayDialogue()
    {
        cpt++;
        text.SetActive(true);
        AudioManager.Instance.Play("Voix_Pepe");
        StartCoroutine(text.GetComponentInChildren<SD_TextTest>().Text());
        GameManagerV2.Instance.Saving(false);


    }

    void Interact()
    {
            AudioManager.Instance.Pause();
            moveBack = false;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerAttack.Instance.cantAim = true;
            camera.SetActive(true);
            playerCam.SetActive(false);
            camera.transform.position = new Vector3(SD_PlayerMovement.Instance.transform.position.x, SD_PlayerMovement.Instance.transform.position.y, -10);
            move = true;            
            distance = Mathf.Abs(Vector2.Distance(new Vector3(transform.position.x, transform.position.y, -10), camera.transform.position)) * 0.1f;
        
    }

   
    
}
