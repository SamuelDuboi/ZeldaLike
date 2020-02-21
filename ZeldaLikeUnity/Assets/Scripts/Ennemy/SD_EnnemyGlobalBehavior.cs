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

        [HideInInspector] public bool isAggro;

        bool canTakeDamage;

        // to avoid wall
        enum raycastDirection { North, East, South, West }
        RaycastHit2D[] hitPoints = new RaycastHit2D[4];
        int wallTouch = 4;
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
                isAggro = true;
                canMove = true;
                canTakeDamage = true;
                player = collision.gameObject;
            }
            else if(collision.gameObject.layer == 8 )
            {
                if(canTakeDamage)
                StartCoroutine(TakingDamage(SD_PlayerAttack.Instance.currentDamage, collision.gameObject));
            }
        }
        public virtual void FixedUpdate()
        {
            if (canMove)
            {
                Mouvement();
            }
            LayerMask wallMask = LayerMask.GetMask("Wall");
            RaycastHit2D northRay = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), 2, wallMask);
            RaycastHit2D eastRay = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 2, wallMask);
            RaycastHit2D southRay = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.down), 2, wallMask);
            RaycastHit2D westRay = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.left), 2, wallMask);

            hitPoints[(int)raycastDirection.North] = northRay;
            hitPoints[(int)raycastDirection.East] = eastRay;
            hitPoints[(int)raycastDirection.South] = southRay;
            hitPoints[(int)raycastDirection.West] = westRay;



            Debug.DrawLine(transform.position, Vector2Extensions.addVector(transform.position, Vector2.right * 2));

            if (isAggro)
            {
                ennemyRGB.velocity = Vector2.zero;
                for (int i = 0; i < hitPoints.Length; i++)
                {

                    if (hitPoints[i].collider != null)
                    {
                        if (Vector2.Distance(transform.position, hitPoints[i].point) <= 1.9f)
                            transform.position = Vector2.MoveTowards(transform.position,
                                                                        new Vector2(transform.position.x - (hitPoints[i].point.x - transform.position.x),
                                                                                     transform.position.y - (hitPoints[i].point.y - transform.position.y)),
                                                                      Time.deltaTime * speed);


                        wallTouch = i;
                        break;
                    }
                    else
                    {
                        canMove = true;
                        wallTouch = 4;
                    }
                }

                if (wallTouch < 4)
                {
                    canMove = false;
                    if (wallTouch == (int)raycastDirection.East || wallTouch == (int)raycastDirection.West)
                    {

                        if (player.transform.position.y >= transform.position.y)
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.up * 100, Time.deltaTime * speed);
                        else
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.down * 100, Time.deltaTime * speed);
                    }
                    else if (wallTouch == (int)raycastDirection.North || wallTouch == (int)raycastDirection.South)
                    {
                        if (player.transform.position.x >= transform.position.x)
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.right * 100, Time.deltaTime * speed);
                        else
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.left * 100, Time.deltaTime * speed);
                    }
                }

            }


        }
        public  virtual  void Mouvement()
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