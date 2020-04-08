using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;
public  class SD_PNJ : MonoBehaviour
{
   public List<GameObject> text= new List<GameObject>();
    int cpt;
    int pnj;
    GameObject interactButton;
    public  void Start()
    {
        interactButton = transform.GetChild(0).gameObject;
        switch (text[0].GetClassInChildren<SD_TextTest>().pnj)
        {
            case 0:
                  cpt = SD_PlayerRessources.Instance.Alyah1;
                break;
            case 1:
                 cpt= SD_PlayerRessources.Instance.Alyah2 ;
                break;
            case 2:
                 cpt= SD_PlayerRessources.Instance.Henry1 ;
                break;
            case 3:
                cpt=SD_PlayerRessources.Instance.Henry2 ;
                break;
            case 4:
                 cpt= SD_PlayerRessources.Instance.WindMother ;
                break;
            case 5:
                 cpt= SD_PlayerRessources.Instance.Pepe ;
                break;
        }
    }
    // Start is called before the first frame update

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

        if (Input.GetButtonDown("Interact"))
        {
            if (!text[cpt].activeInHierarchy)
            {
                interactButton.SetActive(false);
                text[cpt].SetActive(true);
                if (cpt < text.Count - 1)
                {
                    cpt++;
                    switch (text[0].GetClassInChildren<SD_TextTest>().pnj)
                    {
                        case 0:
                            SD_PlayerRessources.Instance.Alyah1 = cpt;
                            break;
                        case 1:
                            SD_PlayerRessources.Instance.Alyah2 = cpt;
                            break;
                        case 2:
                            SD_PlayerRessources.Instance.Henry1 = cpt;
                            break;
                        case 3:
                            SD_PlayerRessources.Instance.Henry2 = cpt;
                            break;
                        case 4:
                            SD_PlayerRessources.Instance.WindMother = cpt;
                            break;
                        case 5:
                            SD_PlayerRessources.Instance.Pepe = cpt;
                            break;
                    }
                    GameManagerV2.Instance.Saving(false);
                }

            }

        }
    }
}
