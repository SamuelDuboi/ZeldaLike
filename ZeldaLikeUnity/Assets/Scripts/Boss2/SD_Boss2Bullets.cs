﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_Boss2Bullets : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    float timer;
    float bigtimer;
    [HideInInspector] public Rigidbody2D bulletRGB;
    [Range(0,10)]
    public int bulletSpeed = 5;
    bool isParry;
    public int bulletDamage = 1;
    bool gonnaDie;
    float timerDeath;
    void Awake()
    {
        bulletRGB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isParry)
        {
            if (bigtimer < 1)
            {
                bigtimer += Time.deltaTime;
                timer += Time.deltaTime;
                if (timer > 0.2)
                {
                    bulletRGB.AddForce((target.transform.position - transform.position) * bulletSpeed);
                    timer = 0;
                }
            }
            else
            {
                bulletRGB.velocity = new Vector2(target.transform.position.x - transform.position.x,
                                                     target.transform.position.y - transform.position.y).normalized * bulletSpeed;
                transform.rotation = Quaternion.Euler(0, 0, 180+Vector2.Angle(transform.position, target.transform.position));
            }
              
            
        }
        else
        {
            bulletRGB.velocity = new Vector2(transform.parent.position.x - transform.position.x,
                                             transform.parent.position.y - transform.position.y + 1.5f).normalized * bulletSpeed;
            transform.rotation = Quaternion.Euler(0,0, Vector2.Angle(transform.position, transform.parent.position));
            if(Mathf.Abs( transform.position.x - transform.parent.position.x)<0.1f)
                Destroy(gameObject);
        }
        if (gonnaDie)
        {
            timerDeath += Time.deltaTime;
            if (timer > 3f)
                Destroy(gameObject);
        } 


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==8)
        {
            isParry = true;
        }
        else if( collision.gameObject.layer == 16 && isParry)
        {
            collision.transform.parent.GetComponent<SD_Boss2Body>().Stun();
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 11)
        {
           StartCoroutine( SD_PlayerRessources.Instance.TakingDamage(bulletDamage, gameObject, true, 1, true));
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            gonnaDie = true;
        }

    }
}
