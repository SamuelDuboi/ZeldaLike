using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;

public class AlyahBehavior : MonoBehaviour
{
    public List<GameObject> text = new List<GameObject>();
    int cpt;
    int pnj;
    GameObject interactButton;
    bool runAway;
   public  GameObject alya2;
    public void Start()
    {
        interactButton = transform.GetChild(0).gameObject;
      
    }
    private void Update()
    {
        if (runAway)
        {
            transform.position = Vector2.MoveTowards(transform.position, alya2.transform.position, 20 * Time.deltaTime);
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            if(Vector2.Distance(transform.position, alya2.transform.position) < 10)
            {
                alya2.SetActive(true);
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            interactButton.SetActive(false);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayDialogue();
        }
    }

    public void PlayDialogue()
    {
        if (!interactButton.activeInHierarchy && !text[cpt].activeInHierarchy)
        {
            interactButton.SetActive(true);
            interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
            interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                            interactButton.transform.parent.position.y + 1);
        }


        if (!text[0].activeInHierarchy)
        {
            interactButton.SetActive(false);
            text[0].SetActive(true);
            StartCoroutine( text[0].GetComponent<SD_TextTest>().Text());

             if (!runAway && cpt > 0)
            {
                runAway = true;
                GetComponent<Animator>().SetBool("Run", true);
            }
               
            cpt++;
        }

    }
}
