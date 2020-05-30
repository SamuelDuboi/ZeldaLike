using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBoss1 : MonoBehaviour
{
    bool cant;

    // Update is called once per frame
    void Update()
    {
        if (cant)
        {
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAim = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerRessources.Instance.cantTakeDamage = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("On");
            cant = true;
        }
    }


    public void PlayerOff()
    {
        SD_PlayerMovement.Instance.transform.position = new Vector3(transform.position.x, transform.position.y, - 300);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(5);
    }
}
