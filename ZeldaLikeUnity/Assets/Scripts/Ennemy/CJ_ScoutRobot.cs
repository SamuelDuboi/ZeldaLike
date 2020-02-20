using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemy
{
  public class CJ_ScoutRobot : SD_EnnemyGlobalBehavior

    {
        [Range(0,20)]
        public float stopDistance;
        [Range(0,20)]
        public float retreatDistance;
        [Range(0, 20)]
        public float retreatSpeed;
        [Range(0,10)]
        public float recoverytime;
        [Range(0,50)]
        public float bulletSpeed;
        bool canShoot = true;
        [HideInInspector]public GameObject target;
        public GameObject ennemyBullet;
      public override void Start()
    {
            base.Start();
            target = gameObject.transform.GetChild(0).gameObject;
            target.SetActive(false);
    }
    
     public override void FixedUpdate()
    {
            base.FixedUpdate();
            if (canMove)
                ennemyRGB.velocity = Vector2.zero;
    }

        public override void Mouvement()
        {
            if(Vector2.Distance(transform.position,player.transform.position) > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
            }
            else if(Vector2.Distance(transform.position,player.transform.position) < stopDistance && Vector2.Distance(transform.position, player.transform.position) > retreatDistance && canShoot == true)
            {
                canMove = false;
                StartCoroutine(SniperShot());
            }
            else if (Vector2.Distance(transform.position, player.transform.position) < stopDistance && Vector2.Distance(transform.position, player.transform.position) < retreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * -retreatSpeed);
            }
        }

        IEnumerator SniperShot()
        {
            float timer = 2f;
            canShoot = false;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            while (timer > 0)
            {
                target.SetActive(true);
                timer -= Time.deltaTime;
                target.transform.position = player.transform.position;
                yield return null;
            }
            target.GetComponent<SpriteRenderer>().color = Color.black;
            yield return new WaitForSeconds(0.5f);
            target.SetActive(false);
            GameObject bullet = Instantiate(ennemyBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = (target.transform.position - gameObject.transform.position).normalized *  bulletSpeed;
            yield return new WaitForSeconds(recoverytime);
            canMove = true;
            //yield return new WaitForSeconds(recoverytime);
            canShoot = true;
        }
    }
}