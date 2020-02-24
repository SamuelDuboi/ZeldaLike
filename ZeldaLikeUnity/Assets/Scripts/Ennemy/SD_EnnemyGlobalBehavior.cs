using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Player;
using UnityEngine.Tilemaps;


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
       [HideInInspector] public bool isAvoidingObstacles;
        public int damage;
        public int life;

        [HideInInspector] public bool isAggro;

        bool canTakeDamage;

        // to avoid wall
        enum raycastDirection { North, North1,East, East1, South, South1, West, West1 }
        RaycastHit2D[] hitPoints = new RaycastHit2D[8];
        int wallTouch = 4;

        public GameObject[] rayStartCorner = new GameObject[4];
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
                isAvoidingObstacles = true;
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
            if(isAvoidingObstacles)
                AvoidWalls();


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
                ennemyRGB.velocity = Vector2.zero;

                for (int i = 0; i < hitPoints.Length; i++)
                {
                    // true if the racast has touch something, stock the number of the raycast that hitted in the array into wall touch
                    if (hitPoints[i].collider != null)
                    {
                        // if the collision point is too close, the object go backward to avoid to fall in the holes
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
                        // if the raycast didn't touche anything, the object can move normally
                        canMove = true;
                        wallTouch = 8;
                    }
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
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Hole")
                StartCoroutine(Fall(collision.GetContact(0).point));

        }

        IEnumerator Fall(Vector2 collision)
        {            
            canMove = false;
            isAggro = false;          
            
            ennemyRGB.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().isTrigger = true;
            transform.position = Vector3.MoveTowards(transform.position, collision, 200);
            
            for (int i = 0; i < 100; i++)
            {
                Vector2 reduction = Vector2Extensions.addVector(transform.localScale, -new Vector2(0.01f, 0.01f));
                transform.localScale = reduction;
                yield return new WaitForSeconds(0.001f);
            }
            Destroy(gameObject);
        }
    }
}