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

        public GameObject target;
        public GameObject ennemyBullet;
        public GameObject shield;
        bool canAttack = false;
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
           
            target.SetActive(false);
            if(!WontRepop)
            GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.gardianRobot, gameObject);
            isShielded = true;
            laser = GetComponent<LineRenderer>();
            StartCoroutine(waitToAttac());
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
            {
                transform.position = Vector2.MoveTowards(transform.position,
                                                            player.transform.position,
                                                            Time.deltaTime * speed);
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);

                    ennemyAnimator.SetTrigger("Moving");


            }
                
           

        }

        public IEnumerator Shoot()
        {
            float timer = 2f;
            canAttack = false;
            canMove = false;
            isAttacking = true;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            limit++;

            ennemyAnimator.SetTrigger("LaserCharge");
            AudioManager.Instance.Play("Gardien_Charge_Tir");
            while (timer > 0)
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);
                isAttacking = true;
                canMove = false;
                target.SetActive(true);
                timer -= Time.deltaTime;
                target.transform.position = player.transform.position;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(0.5f);
            AudioManager.Instance.Stop("Gardien_Charge_Tir");
            AudioManager.Instance.Play("Gardien_Tir");
            ennemyAnimator.SetBool("LaserShoot", true);
            LayerMask playermask = 1 << 11;
            while (timer < laserDuration)
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);
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
            ennemyAnimator.SetBool("LaserShoot", false);
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
            if (player.transform.position.x - transform.position.x > 0)
                ennemyAnimator.SetFloat("Left", 0);
            else
                ennemyAnimator.SetFloat("Left", 1);
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
            AudioManager.Instance.Play("Gardien_Bouclier_Desactiver");
            isShielded = false;
            yield return new WaitForSeconds(3);
            isShielded = true;
            shield.SetActive(true);
        }
      public override void Aggro(Collider2D collision)
        {
            base.Aggro(collision);
            ennemyAnimator.SetTrigger("Aggro");
        }

        IEnumerator waitToAttac()
        {
            yield return new WaitForSeconds(2f);
            canAttack = true;
        }
    }
}

