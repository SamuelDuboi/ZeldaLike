using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_WindPower : MonoBehaviour
{
    public GameObject interactButton;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactButton.SetActive(true);

        interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                        interactButton.transform.parent.position.y + 1);
        if (collision.gameObject.tag == "Player"  && Input.GetButtonDown("Interact"))
        {
            SD_PlayerAttack.Instance.hasWind = true;
            GetComponent<Animator>().enabled = true;

            interactButton.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        interactButton.SetActive(false);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void StopANimation()
    {
        GetComponent<Animator>().enabled = false;
    }
}
