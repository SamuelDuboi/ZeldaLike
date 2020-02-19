using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Player;


namespace Ennemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class SD_EnnemyGlobalBehavior : MonoBehaviour
    {
      [HideInInspector] public  GameObject player;
      [HideInInspector] public  Rigidbody2D ennemyRGB;
       CircleCollider2D aggroZone;
        [Range(0,10)]
        public float speed;
       [HideInInspector]public  bool canMove;
        public int damage;
        public int life;

        public virtual void Start()
        {
            ennemyRGB = GetComponent<Rigidbody2D>();
            aggroZone = GetComponent<CircleCollider2D>();
        }


        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                aggroZone.enabled = false;
                canMove = true;
                player = collision.gameObject;
            }
            else if(collision.gameObject.layer == 8 )
            {
                StartCoroutine(TakingDamage(SD_PlayerAttack.Instance.currentDamage, collision.gameObject));
            }
        }
        public virtual void FixedUpdate()
        {
            if (canMove)
            {
                Mouvement();
            }
            

        }
        public virtual void Mouvement()
        {

        }

        IEnumerator TakingDamage(int damage, GameObject attack)
        {
            canMove = false;
            life -= damage;
            if (life <= 0)
                Destroy(gameObject);
            ennemyRGB.velocity = new Vector2(  transform.position.x -attack.transform.position.x,
                                              attack.transform.position.y - attack.transform.position.y).normalized *10;
            yield return new WaitForSeconds(0.5f);
            ennemyRGB.velocity = Vector2.zero;
            canMove = true;

        }
    }
}