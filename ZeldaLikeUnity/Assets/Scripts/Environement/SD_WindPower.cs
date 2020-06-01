using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;

public class SD_WindPower : MonoBehaviour
{
    public GameObject end;
    public GameObject interactButton;
    public GameObject text;
    bool canText;
    private void Update()
    {
        if (!text.activeSelf && canText)
        {
            canText = false;
            SD_PlayerAttack.Instance.hasWind = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerAttack.Instance.cantAim = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerMovement.Instance.cantDash = true;
            GetComponent<Animator>().enabled = true;
            end.transform.position = SD_PlayerMovement.Instance.transform.position;
            StartCoroutine( GameManagerV2.Instance.GamePadeShake(0.2f, 4.75f));
            StartCoroutine(glow());
        }
    }
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
            canText = true;
            interactButton.SetActive(false);
            text.SetActive(true);
            StartCoroutine( text.GetComponentInChildren<SD_TextTest>().Text());
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
        SD_PlayerAttack.Instance.cantAim = false;
        SD_PlayerAttack.Instance.cantAttack = false;
        SD_PlayerMovement.Instance.cantMove = false;
        SD_PlayerMovement.Instance.cantDash = false;
        GameManagerV2.Instance.Saving(false);
        GetComponent<Animator>().enabled = false;
        Destroy(this);
    }

    IEnumerator glow()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.8f, 1f));
        GameObject halo = SD_PlayerAnimation.Instance.halo;
        halo.SetActive(true);
        halo.transform.localScale = new Vector2(0.1f, 0.1f);
        for (int i =0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            halo.transform.localScale += new Vector3(0.1f, 0.1f);
        }
    }


}
