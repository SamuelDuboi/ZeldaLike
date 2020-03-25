using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_WindPower : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"  && Input.GetButtonDown("Interact"))
        {
            SD_PlayerAttack.Instance.hasWind = true;
            GetComponent<Animator>().enabled = true;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void StopANimation()
    {
        GetComponent<Animator>().enabled = false;
    }
}
