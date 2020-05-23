using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    float speed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            speed = SD_PlayerMovement.Instance.speed;
            SD_PlayerMovement.Instance.speed = speed * 0.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
           
            SD_PlayerMovement.Instance.speed =speed ;
        }
    }
}
