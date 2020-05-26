using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_Cristal : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject halo;
    public GameObject particule;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (SD_PlayerMovement.Instance.platformNumber == 0)
                SD_PlayerMovement.Instance.platformNumber = 1;
            StartCoroutine(DesapearCooldown());
        }
    }

    IEnumerator DesapearCooldown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        halo.SetActive(false);
        particule.SetActive(false);
        yield return new WaitForSeconds(5f);
        halo.SetActive(true);
        particule.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
