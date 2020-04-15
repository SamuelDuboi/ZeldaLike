using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Player;

namespace Ennemy
{
   [RequireComponent (typeof (LineRenderer))]
    public class CJ_GuardianRobot : SD_EnnemyGlobalBehavior
    {
        [Range(0,20)]
        public float attackRange;

        [HideInInspector] public GameObject target;
        [HideInInspector] public GameObject smashImpact;
        public GameObject ennemyBullet;
        public GameObject shield;
        bool canAttack = true;
        int limit = 0;
        bool isShielded;

        [Header("Laser")]
        public float laserDuration = 1;
        public float cooldownAfterLaser = 2;
        public int laserDamage=2;
        LineRenderer laser;

        [Header("Smash")]
        public int smashDamage;
        public float cooldownAfterSmash;
        public override void Start()
        {
            base.Start();
            target = gameObject.transform.GetChild(0).gameObject;
            target.SetActive(false);
            smashImpact = gameObject.transform.GetChild(1).gameObject;
            smashImpact.SetActive(false);
            if( IsInMainScene)
            GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.gardianRobot, gameObject);
            else
                GameManager.Instance.AddEnnemieToList(GameManager.ennemies.gardianRobot, gameObject);

            isShielded = true;
            laser = GetComponent<LineRenderer>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isShielded)
                canTakeDamage = false;
            else
                canTakeDamage = true;
            if (canAttack && isAggro)
            {
                int random = Random.Range(1, 3);
                if (random == 1)
                    StartCoroutine(Shoot());
                else
                    StartCoroutine(Smash());
            }
               
        }

        public override void Mouvement()
        {
            if (!isAttacking) 
                transform.position = Vector2.MoveTowards(transform.position, 
                                                         player.transform.position, 
                                                         Time.deltaTime * speed);
           

        }

        public IEnumerator Shoot()
        {
            float timer = 2f;
            canAttack = false;
            canMove = false;
            isAttacking = true;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            limit++;
            while (timer > 0)
            {
                isAttacking = true;
                canMove = false;
                target.SetActive(true);
                timer -= Time.deltaTime;
                target.transform.position = player.transform.position;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(0.5f);
            LayerMask playermask = 1 << 11;
            while (timer < laserDuration)
            {
                isAttacking = true;
                canMove = false;
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, target.transform.position);
                RaycastHit2D hitpoint = Physics2D.Raycast(transform.position,
                                                     new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y),
                                                     2000,
                                                    playermask);
                timer += 0.01f;
                if (hitpoint.collider != null)
                {
                    if (hitpoint.collider.gameObject.tag == "Player")
                        StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 0.1f));
                }
                yield return new WaitForSeconds(0.01f);
            }
            timer = 0;
            target.SetActive(false);
            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, transform.position);
            canMove = true;
            isAttacking = false;
            yield return new WaitForSeconds(cooldownAfterLaser);
            canAttack = true;

        }

        public IEnumerator Smash()
        {
            canMove = false;
            canAttack = false;
            isAttacking = true;
            GetComponent<Animator>().SetTrigger("Smash");
            float timer = 0;
            int formerDamage = damage;
            damage = smashDamage;
                
            while (timer < 2)
            {
                isAttacking = true;
                canMove = false;
                timer += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            damage = formerDamage;
            isAttacking = false;
            canMove = true;
            yield return new WaitForSeconds(cooldownAfterSmash);
            canAttack = true;
        }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if(collision.gameObject.layer == 14)
            {
                if (shield.activeInHierarchy)
                    StartCoroutine(ShieldPopOut());
;
            }
        }

        IEnumerator ShieldPopOut()
        {
            shield.SetActive(false);
            isShielded = false;
            yield return new WaitForSeconds(3);
            isShielded = true;
            shield.SetActive(true);
        }
    }
}

