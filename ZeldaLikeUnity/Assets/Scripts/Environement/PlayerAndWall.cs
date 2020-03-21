using System.Collections.Generic;
using UnityEngine;
using Player;



 public class PlayerAndWall : MonoBehaviour
 {
    int layer;
    CompositeCollider2D WallCollider;

    private void Start()
    {
        WallCollider = GetComponent<CompositeCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ContactPoint2D contact = collision.contacts[0];
            if (Vector3.Dot(contact.normal, Vector3.down) > 0.5)
            {
                SD_PlayerMovement.Instance.cantDash = true;
                SD_PlayerMovement.Instance.cantMove = true;
                WallCollider.isTrigger = true;
                collision.gameObject.GetComponentInParent<Rigidbody2D>().gravityScale = 10;
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            layer++;
            Debug.Log(layer);
            if (layer == 2)
            {
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                WallCollider.isTrigger = false;
                collision.gameObject.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
                layer = 0;
            }
        }
    }
}
