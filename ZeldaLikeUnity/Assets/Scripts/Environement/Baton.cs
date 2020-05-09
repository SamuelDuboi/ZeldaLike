using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class Baton : MonoBehaviour
{
    bool canCircle;
    bool canIntercat;
    public GameObject interactButton;
    public GameObject AttackButton;
    GameObject player;
    public float range;
    public GameObject halo;
    public GameObject Alyah2;
    public GameObject Alyah3;
    private void Update()
    {
        if (canIntercat)
        {
            if(Mathf.Abs(Vector2.Distance(transform.position,player.transform.position))< range)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    interactButton.SetActive(false);
                    AudioManager.Instance.Play("Tresor_Obtenu");
                    AttackButton.SetActive(true);
                    AttackButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
                    AttackButton.transform.position = new Vector2(AttackButton.transform.parent.position.x,
                                                                    AttackButton.transform.parent.position.y + 1);
                    SD_PlayerAttack.Instance.cantAttack = false;
                    if (Alyah2.activeSelf)
                    {
                        Alyah2.SetActive(false);
                        Alyah3.SetActive(true);
                    }
                    canCircle = true;
                    canIntercat = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    halo.SetActive(false);
                }
            }
            else
            {
                interactButton.SetActive(false);
            }
        }
      else if(canCircle)
        {
            if(Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) < 8f && !AttackButton.activeSelf)
            {
                AttackButton.SetActive(true);
            }
            else if (Mathf.Abs(Vector2.Distance(transform.position, player.transform.position)) >= 8f)
            {
                AttackButton.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canCircle && !interactButton.activeInHierarchy)
        {
            player = collision.gameObject;
            interactButton.SetActive(true);
            interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
            interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                            interactButton.transform.parent.position.y + 1);
            canIntercat = true;
        }
    }

}
