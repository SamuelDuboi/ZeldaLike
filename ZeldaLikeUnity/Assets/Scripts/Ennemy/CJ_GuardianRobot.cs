using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemy
{
    public class CJ_GuardianRobot : SD_EnnemyGlobalBehavior
    {
        [Range(0,20)]
        public float attackRange;

        [HideInInspector] public GameObject target;
        [HideInInspector] public GameObject smashImpact;
        public GameObject ennemyBullet;
        bool canAttack = true;
        int limit = 0;
        public override void Start()
        {
            base.Start();
            target = gameObject.transform.GetChild(0).gameObject;
            target.SetActive(false);
            smashImpact = gameObject.transform.GetChild(1).gameObject;
            smashImpact.SetActive(false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Mouvement()
        {
            if(Vector2.Distance(transform.position,player.transform.position) > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
                if(canAttack == true && limit < 2)
                {
                    StartCoroutine(Shoot());
                }
            }
            else if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
            {
                if (canAttack == true)
                {
                    StartCoroutine(Smash());
                }
            }
        }

        public IEnumerator Shoot()
        {
            float timer = 2f;
            canAttack = false;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            limit++;
            while (timer > 0)
            {
                target.SetActive(true);
                timer -= Time.deltaTime;
                target.transform.position = player.transform.position;
                yield return null;
            }
            target.SetActive(false);
            for (int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(ennemyBullet, transform.position, Quaternion.identity);
                bullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
                bullet.GetComponent<CJ_BulletBehaviour>().target = player;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(2f);
            canAttack = true;
        }

        public IEnumerator Smash()
        {
            float time = 0.8f;
            isAggro = false;
            canAttack = false;
            GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            smashImpact.SetActive(true);
            while (time > 0)
            {
                time -= Time.deltaTime;
                smashImpact.transform.localScale += new Vector3(3 * Time.deltaTime, 3 * Time.deltaTime);
                yield return null;
            }
            smashImpact.transform.localScale = new Vector3(1, 1, 1);
            smashImpact.SetActive(false);
            yield return new WaitForSeconds(3f);
            limit = 0;
            isAggro = true;
            canAttack = true;
        }
    }
}

