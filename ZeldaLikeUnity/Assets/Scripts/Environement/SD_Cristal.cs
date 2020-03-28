using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_Cristal : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SD_PlayerMovement.Instance.platformNumber++;
            StartCoroutine(DesapearCooldown());
        }
    }

    IEnumerator DesapearCooldown()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(5f);

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
