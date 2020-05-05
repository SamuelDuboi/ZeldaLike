using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;


public class TriggerActivated : MonoBehaviour
{

    GameObject interactButton;
    public bool isActivated;
    public GameObject previousTrigger;
    public Animator animator;
    private void Start()
    {

        interactButton = transform.GetChild(0).gameObject;
        interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
        interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                        interactButton.transform.parent.position.y + 1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActivated == false)
        interactButton.SetActive(true);

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Interact"))
        {
            animator.SetBool("On", true);
            if (isActivated == false && previousTrigger == null)
            {

                isActivated = true;


                interactButton.SetActive(false);
                SD_TriggerRonchonchon.Instance.TriggerUp();
            }
            else if (previousTrigger.GetComponent<TriggerActivated>().isActivated && isActivated == false)
            {
                isActivated = true;
                interactButton.SetActive(false);
                SD_TriggerRonchonchon.Instance.TriggerUp();
            }
            else if(isActivated == false)
            {
                SD_TriggerRonchonchon.Instance.ResetAll();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactButton.SetActive(false);
    }

    public IEnumerator resetTriger()
    {
            yield return new WaitForSeconds(0.1f);
            isActivated = false;
            animator.SetBool("On", false);
    }
}
