using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_LockedDoor : MonoBehaviour
{
    public int keyNumber;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetButtonDown("Interact") && SD_PlayerMovement.Instance.keyNumber>0)
        {
            SD_PlayerMovement.Instance.keyNumber --;
            SD_PlayerMovement.Instance.keyUI[SD_PlayerMovement.Instance.keyNumber].SetActive(false);
            keyNumber--;
            if (keyNumber ==0)
                Destroy(gameObject);
        }
    }
}
