using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_PNJ : MonoBehaviour
{
   public List<GameObject> text= new List<GameObject>();
    int cpt;
    GameObject interactButton;
    private void Start()
    {
        interactButton = transform.GetChild(0).gameObject;
    }
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
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
                if(!text[cpt].activeInHierarchy)
                {
                    interactButton.SetActive(false);
                    text[cpt].SetActive(true);
                    if (cpt < text.Count-1)
                        cpt++;

                }
                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            interactButton.SetActive(false);

    }
}
