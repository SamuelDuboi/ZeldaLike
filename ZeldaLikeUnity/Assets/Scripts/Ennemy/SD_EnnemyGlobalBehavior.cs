﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Player;
using UnityEngine.Tilemaps;
using Management;


namespace Ennemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class SD_EnnemyGlobalBehavior : MonoBehaviour
    {
        [HideInInspector] public GameObject player;
        [HideInInspector] public Rigidbody2D ennemyRGB;
        public GameObject aggroZone;
        public GameObject desaggroZone;
        [Range(0, 10)]
        public float speed;
         public bool canMove;
         public bool isAvoidingObstacles;
        public int damage;
        public int life;

        [HideInInspector] public bool isAggro;

        bool canTakeDamage;

        [Range(0, 1)]
        public float freezTime = 1;
        // to avoid wall
        enum raycastDirection { North, North1, East, East1, South, South1, West, West1 }
        RaycastHit2D[] hitPoints = new RaycastHit2D[8];
        int wallTouch = 4;

        public bool IsInMainScene;
        public GameObject[] rayStartCorner = new GameObject[4];

        public bool isAttacking;

        [HideInInspector] public Vector2 startPosition;

        public GameObject healDrop;
        public virtual void Start()
        {
            ennemyRGB = GetComponent<Rigidbody2D>();
        }


        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                CJ_PlayerCameraManager.Instance.ennemyList.Add(gameObject);
                if (aggroZone.activeSelf)
                {
                    aggroZone.SetActive(false);
                    desaggroZone.SetActive(true);

                }
                isAggro = true;
                canMove = true;
                isAvoidingObstacles = true;
                canTakeDamage = true;
                player = collision.transform.parent.gameObject;
            }//if is attack by player
            else if (collision.gameObject.layer == 8)
            {
                if (canTakeDamage)
                {
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.3f, .2f));
                    StartCoroutine(TakingDamage(SD_PlayerAttack.Instance.currentDamage, collision.gameObject, false, 10));
                }
                else
                    StartCoroutine(TakingDamage(0, collision.gameObject, false, 20));

            } //if is attack by projectile
            else if (collision.gameObject.layer == 14 && collision.gameObject.tag != "Wind")
            {
                if (collision.gameObject.GetComponent<CJ_BulletBehaviour>()!= null &&collision.gameObject.GetComponent<CJ_BulletBehaviour>().isParry == true)
                {
                    collision.gameObject.GetComponent<CJ_BulletBehaviour>().isParry = false;
                    StartCoroutine(TakingDamage(3, collision.gameObject, true, 10));
                }
            }

            if (collision.gameObject.tag == "Hole")
                StartCoroutine(Fall(new Vector2(transform.position.x + ennemyRGB.velocity.x, transform.position.y + ennemyRGB.velocity.y)));

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (!aggroZone.activeSelf)
                { 
                    aggroZone.SetActive(true);
                    desaggroZone.SetActive(false);
                    startPosition = transform.position;
                }
                isAggro = false;
                canMove = false;
                isAttacking = false;
                CJ_PlayerCameraManager.Instance.ennemyList.Remove(gameObject);
                isAvoidingObstacles = false;
                canTakeDamage = false;

            }

        }
        public virtual void FixedUpdate()
        {
            if (canMove)
            {
                Mouvement();
            }
            if (isAvoidingObstacles)
                AvoidWalls();


        }
        public virtual void Mouvement()
        {

        }

        public IEnumerator TakingDamage(int damage, GameObject attack, bool destroyContact, int projectionForce)
        {
            Time.timeScale = 0.1f;
            attack.GetComponent<ParticleSystem>().Play();
            isAttacking = false;
            canTakeDamage = false;
            canMove = false;
            isAggro = false;
            life -= damage;
            Debug.Log("damaeg" + damage);
            Debug.Log("life" + life);
            yield return new WaitForSeconds(0.1f* freezTime);
            Time.timeScale = 1;
            if (life <= 0)
            {
                if (IsInMainScene)
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0, .2f));
                else
                    StartCoroutine(GameManager.Instance.GamePadeShake(0, .2f));
                float randomHeal = Random.Range(0, 11);
                if (randomHeal <= SD_PlayerRessources.Instance.chanceDropHeal)
                {
                    Instantiate(healDrop, transform.position, Quaternion.identity);
                    SD_PlayerRessources.Instance.chanceDropHeal = 2;
                }
                else
                    SD_PlayerRessources.Instance.chanceDropHeal++;
                CJ_PlayerCameraManager.Instance.ennemyList.Remove(gameObject);
                Destroy(gameObject);
            }
            if(SD_PlayerAttack.Instance.canPushBack)
                ennemyRGB.velocity = new Vector2(transform.position.x - attack.transform.position.x,
                                              attack.transform.position.y - attack.transform.position.y).normalized * projectionForce;

            if (destroyContact)
                Destroy(attack);
            yield return new WaitForSeconds(0.2f * damage);

            StartCoroutine(Stun(1f));
            ennemyRGB.velocity = Vector2.zero;
            canMove = true;
            canTakeDamage = true;
            isAggro = true;




        }
        /// <summary>
        /// test if their is any walls around the game object, disabled movement if yes and focus on avoiding it
        /// </summary>
        void AvoidWalls()
        {// cast 2 ray cast for each poles, the ray are cast at the corner of the hitbox, the ray range is 2 (pretty small)
            LayerMask wallMask = LayerMask.GetMask("Wall");
            RaycastHit2D northRay = Physics2D.Raycast(rayStartCorner[0].transform.position, transform.TransformDirection(Vector3.up), 2, wallMask);
            RaycastHit2D northRay1 = Physics2D.Raycast(rayStartCorner[1].transform.position, transform.TransformDirection(Vector3.up), 2, wallMask);
            RaycastHit2D eastRay = Physics2D.Raycast(rayStartCorner[1].transform.position, transform.TransformDirection(Vector3.right), 2, wallMask);
            RaycastHit2D eastRay1 = Physics2D.Raycast(rayStartCorner[2].transform.position, transform.TransformDirection(Vector3.right), 2, wallMask);
            RaycastHit2D southRay = Physics2D.Raycast(rayStartCorner[2].transform.position, transform.TransformDirection(Vector3.down), 2, wallMask);
            RaycastHit2D southRay1 = Physics2D.Raycast(rayStartCorner[3].transform.position, transform.TransformDirection(Vector3.down), 2, wallMask);
            RaycastHit2D westRay = Physics2D.Raycast(rayStartCorner[3].transform.position, transform.TransformDirection(Vector3.left), 2, wallMask);
            RaycastHit2D westRay1 = Physics2D.Raycast(rayStartCorner[0].transform.position, transform.TransformDirection(Vector3.left), 2, wallMask);

            // stock all the  raycast in an array
            hitPoints[(int)raycastDirection.North] = northRay;
            hitPoints[(int)raycastDirection.North1] = northRay;
            hitPoints[(int)raycastDirection.East] = eastRay;
            hitPoints[(int)raycastDirection.East1] = eastRay;
            hitPoints[(int)raycastDirection.South] = southRay;
            hitPoints[(int)raycastDirection.South1] = southRay;
            hitPoints[(int)raycastDirection.West] = westRay;
            hitPoints[(int)raycastDirection.West1] = westRay;




            if (isAggro)
            {


                for (int i = 0; i < hitPoints.Length; i++)
                {
                    // true if the racast has touch something, stock the number of the raycast that hitted in the array into wall touch
                    if (hitPoints[i].collider != null)
                    {
                        ennemyRGB.velocity = Vector2.zero;
                        // if the collision point is too close, the object go backward to avoid to fall in the holes
                        if (Vector2.Distance(transform.position, hitPoints[i].point) <= 1.5)
                            transform.position = Vector2.MoveTowards(transform.position,
                                                                     new Vector2(transform.position.x - (hitPoints[i].point.x - transform.position.x),
                                                                                     transform.position.y - (hitPoints[i].point.y - transform.position.y)),
                                                                     Time.deltaTime * speed);

                        wallTouch = i;
                        break;
                    }
                    else
                    {
                        // if the raycast didn't touche anything, the object can move normally
                        canMove = true;
                        wallTouch = 8;
                    }

                    Debug.DrawLine(transform.position, hitPoints[i].point);
                }

                if (wallTouch < 8)
                {
                    canMove = false;
                    // will get the position of the player and move accordingly : if the wall is on the right and the player y is above the object, it will go upward
                    if (wallTouch == (int)raycastDirection.East || wallTouch == (int)raycastDirection.East1 || wallTouch == (int)raycastDirection.West || wallTouch == (int)raycastDirection.West1)
                    {

                        if (player.transform.position.y >= transform.position.y)
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.up * 100, Time.deltaTime * speed);
                        else
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.down * 100, Time.deltaTime * speed);
                    }
                    else if (wallTouch == (int)raycastDirection.North || wallTouch == (int)raycastDirection.North1 || wallTouch == (int)raycastDirection.South || wallTouch == (int)raycastDirection.South1)
                    {
                        if (player.transform.position.x >= transform.position.x)
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.right * 100, Time.deltaTime * speed);
                        else
                            transform.position = Vector2.MoveTowards(transform.position, Vector2.left * 100, Time.deltaTime * speed);
                    }
                }

            }

        }


        IEnumerator Fall(Vector2 collision)
        {
            canMove = false;
            isAggro = false;

            ennemyRGB.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().color = Color.red;
            transform.position = Vector3.MoveTowards(transform.position, collision, 200);
            Vector3 currentScale = transform.localScale;
            for (int i = 0; i < 20; i++)
            {
                Vector2 reduction = Vector2Extensions.addVector(transform.localScale, -new Vector2(0.01f, 0.01f));
                transform.localScale = reduction;
                yield return new WaitForSeconds(0.001f);
            }
            GetComponent<SpriteRenderer>().color = Color.white;
            transform.localScale = currentScale;

                Destroy(gameObject);
            
        }
        public void StunLunch(float timer)
        {
           StartCoroutine( Stun(timer));
        }

      public  virtual IEnumerator Stun(float timer)
        {
            canMove = false;
            ennemyRGB.velocity = Vector2.zero;
            isAvoidingObstacles = false;
            isAttacking = true;
            yield return new WaitForSeconds(timer);
            isAttacking = false;
            isAvoidingObstacles = true;
            canMove = true;
        }
    }
}