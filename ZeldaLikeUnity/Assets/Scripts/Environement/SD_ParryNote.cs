using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;

public class SD_ParryNote : MonoBehaviour
{
    public GameObject[] scoutRobot = new GameObject[2];
    public GameObject[] scoutRobotPNG = new GameObject[2];

    public List<GameObject> text = new List<GameObject>();
    int cpt;
    int pnj;
    public GameObject interactButton;
    bool canIntercat;
    GameObject player;
    [Range(0, 10)]
    public float range = 2f;


    private void Update()
    {
        if (canIntercat)
        {
            if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) < range)
            {
                if (!interactButton.activeInHierarchy)
                    interactButton.SetActive(true);
                else
                    PlayDialogue();
            }
            else if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= range)
            {
                interactButton.SetActive(false);
            }
        }
        if (cpt == 1 && !text[0].activeInHierarchy)
        {
            interactButton.SetActive(false);
            SD_PlayerAttack.Instance.canParry = true;
            GameManagerV2.Instance.Saving(false);
            foreach (GameObject scout in scoutRobot)
            {
                scout.SetActive(true);
            }
            foreach (GameObject scout in scoutRobotPNG)
            {
                scout.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !canIntercat)
        {
            if (!interactButton.activeInHierarchy && !text[cpt].activeInHierarchy)
            {
                interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
                interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                                interactButton.transform.parent.position.y + 1);
                canIntercat = true;
                player = collision.gameObject;
            }

        }
    }

    public void PlayDialogue()
    {

        if (Input.GetButtonDown("Interact"))
        {
            if (!text[0].activeInHierarchy && cpt == 0)
            {

                cpt++;
                interactButton.SetActive(false);
                text[0].SetActive(true);

                AudioManager.Instance.Play("Note_Obtenu");

                StartCoroutine(text[0].GetComponentInChildren<SD_TextTest>().Text());

                GameManagerV2.Instance.Saving(false);

            }

        }

    }



}
