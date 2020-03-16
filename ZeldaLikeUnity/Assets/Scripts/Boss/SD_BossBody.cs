﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;

public class SD_BossBody : MonoBehaviour
{
    int weakPointNumber;
    public GameObject bullet;
    Collider2D bossCollider;
    Vector2 firstBullet;
    public float bulletNummber;
    [Space]
    [Header("Phase1")]
    public float couldowBulletMin1;
    public float couldowBulletMax1;

    [Space]
    [Header("Phase2")]
    public float couldowBulletMin2;
    public float couldowBulletMax2;
    public GameObject bossPositionPhase2;


    [Space]
    [Header("Phase3")]
    public float couldowBulletMin3;
    public float couldowBulletMax3;
    public GameObject bossPositionPhase3;


    float timer;
    float timerToReach;

    [Space]
    public GameObject[] phaseWeakPoins = new GameObject[3];

    
    void Start()
    {
        phaseWeakPoins[1].SetActive(false);
        phaseWeakPoins[2].SetActive(false);
        weakPointNumber = GetComponentsInChildren<Collider2D>().Length-1;
        Debug.Log(weakPointNumber);
        bossCollider = GetComponent<Collider2D>();
        firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
        timerToReach = Random.Range(couldowBulletMin1, couldowBulletMax1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Collider2D[] currentWeakPoint = GetComponentsInChildren<Collider2D>();
            foreach(Collider2D collider in currentWeakPoint)
            {
                if (collider.IsTouching(collision))
                {
                    weakPointNumber--;
                    Debug.Log(weakPointNumber);
                    Destroy(collider.gameObject);
                    if (weakPointNumber == 0)
                    {
                        SD_BossBehavior.Instance.phaseNumber ++;
                        StartCoroutine(Moving());

                    }
                        
                }
            }
        }
    }

    void Update()
    {
        if (SD_BossBehavior.Instance.canMove && SD_BossBehavior.Instance.phaseNumber == 2)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, bossPositionPhase2.transform.position, 10 * Time.deltaTime);
            firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
        }
        else if (SD_BossBehavior.Instance.canMove && SD_BossBehavior.Instance.phaseNumber == 3)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, bossPositionPhase3.transform.position, 10 * Time.deltaTime);
            firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 1)
                Fire(couldowBulletMin1, couldowBulletMax2);
            else if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 2)
                Fire(couldowBulletMin2, couldowBulletMax2);
            else if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 3)
                Fire(couldowBulletMin3, couldowBulletMax3);
        }



    }
    void Fire(float TimerMin, float timerMax)
    {
        timer = 0;
        timerToReach = Random.Range(TimerMin, timerMax);
        Vector2 formerBulletPosition = firstBullet;
        while (formerBulletPosition.x <= firstBullet.x + bossCollider.bounds.size.x)
        {
            Vector2 newBulletPosition = new Vector2(formerBulletPosition.x + bossCollider.bounds.size.x / bulletNummber, formerBulletPosition.y);
            GameObject newBullet = Instantiate(bullet, newBulletPosition, Quaternion.identity);
            newBullet.GetComponent<CJ_BulletBehaviour>().target = new Vector2(newBulletPosition.x, newBulletPosition.y - 10f);
            newBullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;

            formerBulletPosition = newBulletPosition;
        }

    }

    IEnumerator Moving()
    {
        SD_BossBehavior.Instance.canMove = true;
        yield return new WaitForSeconds(4f);
        phaseWeakPoins[SD_BossBehavior.Instance.phaseNumber - 1].SetActive(true);
        phaseWeakPoins[SD_BossBehavior.Instance.phaseNumber - 2].SetActive(false);
        weakPointNumber = GetComponentsInChildren<Collider2D>().Length - 1;
        Debug.Log(weakPointNumber);
        SD_BossBehavior.Instance.canMove = false;
    }
}
