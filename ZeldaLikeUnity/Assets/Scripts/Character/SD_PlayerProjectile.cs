using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;
public class SD_PlayerProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            collision.GetComponent<SD_EnnemyGlobalBehavior>().StunLunch(3f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 9 && collision.tag != "Hole")
            Destroy(gameObject);
    }
}
