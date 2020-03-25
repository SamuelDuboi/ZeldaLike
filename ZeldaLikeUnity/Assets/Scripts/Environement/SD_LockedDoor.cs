using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_LockedDoor : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetButtonDown("Interact") && SD_PlayerMovement.Instance.hasKey)
        {
            SD_PlayerMovement.Instance.hasKey = false;
            SD_PlayerMovement.Instance.keyUI.SetActive(false);
            Destroy(gameObject);
        }
    }
}
