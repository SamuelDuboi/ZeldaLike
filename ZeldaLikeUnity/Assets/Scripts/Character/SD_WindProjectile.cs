using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;

public class SD_WindProjectile : MonoBehaviour
{
    public float timeAlive = 2f;
    float cpt = 0;
    private IEnumerator Start()
    {
        while (cpt < timeAlive)
        {
            cpt += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire")
            Destroy(collision.gameObject);
        if(collision.gameObject.layer == 12)
        {
            collision.GetComponent<SD_EnnemyGlobalBehavior>().StunLunch(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 9 && collision.gameObject.tag != "Hole" && collision.gameObject.tag != "Fire" && collision.gameObject.tag != "DestroyedPlatform")
        {if( collision.GetComponent<SD_LoupiotteActivated>() == null)
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        cpt = 0;
        while (cpt < 0.2f)
        {
            cpt += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        Destroy(gameObject);
    }
}
