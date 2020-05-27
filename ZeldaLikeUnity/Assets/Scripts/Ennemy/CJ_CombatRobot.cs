using System.Collections;
using UnityEngine;
using Management;
using System;
using Player;

namespace Ennemy
{
    public class CJ_CombatRobot : SD_EnnemyGlobalBehavior
    {
        [Range(0, 20)]
        public float slashRange;
        [Range(0, 5)]
        public float slashColdown;
        [Range(0, 20)]
        public float followSpeed;
        bool moveFirst;
        bool isMoving;
        public Material normalMat;
        public Material hitMat;
        SpriteRenderer cmbatSprite;
        public override void Start()
        {
            cmbatSprite = GetComponent<SpriteRenderer>();
            base.Start();
            if (!WontRepop)
                GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.combatRobot, gameObject);
        }

        public override void FixedUpdate()
        {
            if (!moveFirst)
                canMove = false;
            base.FixedUpdate();

            isAvoidingObstacles = false;

            if (isMoving)
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);

                ennemyRGB.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * followSpeed;
               
            }
        }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8  && canTakeDamage)
            {
                StopSlashSounds();StartCoroutine(hit());
            }
            base.OnTriggerEnter2D(collision);         
           
        }       

        public override void Mouvement()
        {
            if (!isAttacking  )
            {                
                if (Mathf.Abs(Vector2.Distance(player.transform.position, transform.position)) < slashRange)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    ennemyRGB.velocity = Vector2.zero;
                    isMoving = false;
                   StartCoroutine(SlashAttack());
                   
                }
                else
                {
                    ennemyAnimator.SetBool("Walk", true);
                    isMoving = true;
                }               
            }
        }


        public IEnumerator SlashAttack()
        {          
            Vector2 Aim;
            isAttacking = true;
            canMove = false;
            isAvoidingObstacles = false;
            Aim = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            if (Aim.y > 0.1 && Mathf.Abs(Aim.y) > Mathf.Abs(Aim.x))
            {
                ennemyAnimator.SetInteger("attackNumber", 2);
            }
            else if (Aim.y < -0.1 && Mathf.Abs(Aim.y) > Mathf.Abs(Aim.x))
            {
                ennemyAnimator.SetInteger("attackNumber", 4);
            }
            else if (Aim.x > 0.1 && Mathf.Abs(Aim.x) > Mathf.Abs(Aim.y))
            {
                ennemyAnimator.SetInteger("attackNumber", 3);
            }
            else if (Aim.x < -0.1 && Mathf.Abs(Aim.x) > Mathf.Abs(Aim.y))
            {
                ennemyAnimator.SetInteger("attackNumber", 1);
            }
            float cpt = 0;

            AudioManager.Instance.Play("Combat_Slash_Preparation");
            while (cpt < 1f)
            {
                cpt += 0.1f;
               
                yield return new WaitForSeconds(0.1f);
            }

            ennemyAnimator.SetTrigger("Attack");
            AudioManager.Instance.Stop("Combat_Slash_Preparation");
            AudioManager.Instance.Play("Combat_Slash");

            cpt = 0;
            yield return new WaitForSeconds(slashColdown);
  
            canMove = true;
            isAvoidingObstacles = true;
            isAttacking = false;          
            
        }
        public void ResetAttack()
        {
            ennemyAnimator.SetInteger("attackNumber", 0);
        }
        public void CanMove()
        {
            moveFirst = true;
            canMove = true;

            ennemyAnimator.SetBool("Walk", true);
        }
        public override void Aggro(Collider2D collision)
        {
            base.Aggro(collision);
            ennemyAnimator.SetTrigger("Aggro");

        }
        public override void Desaggro(Collider2D collision)
        {
            StopAllCoroutines();
            isAttacking = false;
            canTakeDamage = true;
            isMoving = false;
            ennemyAnimator.SetBool("Walk", false);
            ennemyAnimator.SetInteger("attackNumber", 0);
            ennemyAnimator.ResetTrigger("Aggro");
            ennemyAnimator.ResetTrigger("Attack");
            ennemyAnimator.ResetTrigger("Stunned");
            ennemyAnimator.SetTrigger("Sleep");
            ennemyAnimator.SetBool("Stun", false);
            StopSlashSounds();
            base.Desaggro(collision);

        }
        public void StopSlashSounds()
        {
            AudioManager.Instance.Stop("Combat_Slash");
            AudioManager.Instance.Stop("Combat_Slash_Preparation");
        }

        IEnumerator hit()
        {

            cmbatSprite.material = hitMat;
            yield return new WaitForSeconds(0.2f);
            cmbatSprite.material = normalMat;
        }

    }
}