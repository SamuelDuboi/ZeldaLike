using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;
public class SD_WindProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire")
            Destroy(collision.gameObject);
        if(collision.gameObject.layer == 12)
        {
            collision.GetComponent<SD_EnnemyGlobalBehavior>().StunLunch(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 9 && collision.gameObject.tag != "Hole" && collision.gameObject.tag != "Fire")
            Destroy(gameObject);
    }
}
