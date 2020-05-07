﻿using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrosDodo : MonoBehaviour
{
    public GameObject direction;
    bool run;
    float timer;
    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, direction.transform.position, 2 * Time.deltaTime);
                SD_PlayerMovement.Instance.cantDash = true;
                SD_PlayerMovement.Instance.cantMove = true;
                SD_PlayerAttack.Instance.cantAttack = true;
;            }
            if (timer > 5f)
            {
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                SD_PlayerAttack.Instance.cantAttack = false;
                Destroy(gameObject);

            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            GetComponent<Animator>().SetTrigger("WakeUp");
            run = true;
        }
    }
}