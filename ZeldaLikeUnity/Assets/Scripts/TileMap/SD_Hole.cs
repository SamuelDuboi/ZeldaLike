using System.Collections.Generic;
using UnityEngine;
using Player;
using Ennemy;
using UnityEngine.Tilemaps;


 public class SD_Hole : MonoBehaviour
 {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 14)
        {
           
            GetComponent<CompositeCollider2D>().isTrigger = true;
         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }


}
