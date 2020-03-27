using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_ParryNote : MonoBehaviour
{
    public GameObject[] scoutRobot = new GameObject[2];
    void OnCollisionStay2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            SD_PlayerAttack.Instance.canParry = true;
            scoutRobot[1].SetActive(true);
            scoutRobot[0].SetActive(true);
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
