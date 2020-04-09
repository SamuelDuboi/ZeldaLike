using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_WindProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Fire")
            Destroy(collision.gameObject);

        if (collision.gameObject.layer == 9 && collision.gameObject.tag != "Hole")
            Destroy(gameObject);
    }
}
