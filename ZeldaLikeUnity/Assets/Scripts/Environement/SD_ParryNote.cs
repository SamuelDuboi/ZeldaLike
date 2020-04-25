using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;

public class SD_ParryNote : MonoBehaviour
{
    public GameObject[] scoutRobot = new GameObject[2];
    public GameObject[] scoutRobotPNG = new GameObject[2];
    void OnCollisionStay2D (Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetButtonDown("Interact"))
        {
            SD_PlayerAttack.Instance.canParry = true;
            GameManagerV2.Instance.Saving(false);
            foreach(GameObject scout in scoutRobot)
            {
                scout.SetActive(true);
            }
            foreach (GameObject scout in scoutRobotPNG)
            {
                scout.SetActive(false);
            }
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
