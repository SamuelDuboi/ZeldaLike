using System.Collections.Generic;
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
        public bool WontRepop;
        [HideInInspector] public GameObject player;
        [HideInInspector] public Rigidbody2D ennemyRGB;
        public GameObject aggroZone;
        public GameObject desaggroZone;
        [HideInInspector] public bool activeAggro;
        [Range(0, 10)]
        public float speed;
         public bool canMove;
         public bool isAvoidingObstacles;
        public int damage;
        [HideInInspector] public bool dontAttackPlayerOnCOllision;
        public int life;
        [HideInInspector] public Animator ennemyAnimator;

        [HideInInspector] public bool isAggro;

         public bool canTakeDamage;

        [Range(0, 1)]
        public float freezTime = 1;
        // to avoid wall
        enum raycastDirection { North, North1, East, East1, South, South1, West, West1 }
        RaycastHit2D[] hitPoints = new RaycastHit2D[8];
        int wallTouch = 4;

        public GameObject[] rayStartCorner = new GameObject[4];

        public bool isAttacking;

        [HideInInspector] public Vector2 startPosition;

        public GameObject healDrop;

        public GameObject etincelles;
        LayerMask attackMask;


        bool stunCantStun;
        public virtual void Start()
        {
            attackMask = 1 << 8;
            ennemyRGB = GetComponent<Rigidbody2D>();
            ennemyAnimator = GetComponent<Animator>();
        }

       /* private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                if (canTakeDamage)
                {
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.3f, .2f));
                    StartCoroutine(TakingDamage(SD_PlayerAttack.Instance.Damage, collision.gameObject, false, 10));
                }

            }
        }*/
        public virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8)
            {
                if (canTakeDamage)
                {
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.3f, .2f));
                    StartCoroutine(TakingDamage(SD_PlayerAttack.Instance.Damage, collision.gameObject, false, 10));
                }

            }
            if (collision.gameObject.tag == "Player" && !activeAggro)
            {
                Aggro(collision);
            }//if is attack by player
             //if is attack by projectile
            
            if (collision.gameObject.layer == 14 && collision.gameObject.tag != "Wind")
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
            if (collision.gameObject.tag == "Player" && activeAggro)
            {

                Desaggro(collision);
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
            canTakeDamage = false;
            Time.timeScale = 0.1f;
            isAttacking = false;
            canMove = false;
            isAggro = false;
            life -= damage;

            yield return new WaitForSeconds(0.1f* freezTime);
            RaycastHit2D raycastHit2D;
            if (attack.layer ==8)
            {
                 raycastHit2D = Physics2D.Raycast(transform.position,
                                                           new Vector2(transform.position.x - attack.transform.parent.position.x,
                                                            transform.position.y - attack.transform.parent.position.y),
                                                           2,
                                                           attackMask);
            }
            else
            {
                 raycastHit2D = Physics2D.Raycast(transform.position,
                                                               new Vector2(transform.position.x - attack.transform.position.x,
                                                                transform.position.y - attack.transform.position.y),
                                                               2,
                                                               attackMask);
            }
               
            if (raycastHit2D.collider != null)
            {
                etincelles.transform.position = raycastHit2D.point;
                etincelles.SetActive(true);
            }
            Time.timeScale = 1;
            if (life <= 0)
            {

                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0, .2f));

                float randomHeal = Random.Range(0, 11);
                if (randomHeal <= SD_PlayerRessources.Instance.chanceDropHeal)
                {
                    Instantiate(healDrop, transform.position, Quaternion.identity);
                    SD_PlayerRessources.Instance.chanceDropHeal = 2;
                }
                else
                    SD_PlayerRessources.Instance.chanceDropHeal++;
                CJ_PlayerCameraManager.Instance.ennemyList.Remove(gameObject);
                GetComponent<SpriteRenderer>().color = Color.white;
                ennemyAnimator.SetTrigger("Death");
                ennemyRGB.velocity = Vector2.zero;
                isAggro = false;
                canMove = false;
                yield break;
            }
            
            if (SD_PlayerAttack.Instance.canPushBack)
            {
                ennemyRGB.velocity = new Vector2(transform.position.x - SD_PlayerMovement.Instance.transform.position.x,
                                               transform.position.y - SD_PlayerMovement.Instance.transform.position.y).normalized * projectionForce;
                yield return new WaitForSeconds(0.5f);
                ennemyRGB.velocity = Vector2.zero;
            }
            yield return new WaitForSeconds(0.2f);
            etincelles.SetActive(false);
            if(!notStunable)
            canTakeDamage = true;
            StartCoroutine(Stun(1f));
            if (destroyContact)
                Destroy(attack);
            
            
            canMove = true;
            isAggro = true;




        }
        /// <summary>
        /// test if their is any walls around the game object, disabled movement if yes and focus on avoiding it
        /// </summary>
        void AvoidWalls()
        {// cast 2 ray cast for each poles, the ray are cast at the corner of the hitbox, the ray range is 2 (pretty small)
            LayerMask wallMask = 1<<9;
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
                    if (hitPoints[i].collider != null && hitPoints[i].collider.gameObject.tag == "Hole")
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
            if(canTakeDamage)
           StartCoroutine( Stun(timer));
        }

        [HideInInspector] public bool notStunable;
      public  virtual IEnumerator Stun(float timer)
        {
            if (!notStunable && !stunCantStun)
            {
                stunCantStun = true;
                ennemyAnimator.SetBool("Stun", true);
                ennemyAnimator.SetTrigger("Stunned");
                canMove = false;
                ennemyRGB.velocity = Vector2.zero;
                isAvoidingObstacles = false;
                isAttacking = false;
                isAttacking = true;
                float cpt = 0;
                while (cpt < timer)
                {
                    if (notStunable)
                    {

                        break;
                    }
                    else
                    {
                        cpt += 0.05f;
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                isAttacking = false;
                isAvoidingObstacles = true;
                canMove = true;
                ennemyAnimator.SetBool("Stun", false);
                stunCantStun = false;
            }
            
           
        }

        public void Death()
        {
            Destroy(gameObject);
        }

        public virtual void Aggro(Collider2D collision)
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
            activeAggro = true;
        }

        public virtual void Desaggro(Collider2D collision)
        {

            if (!aggroZone.activeSelf)
            {
                aggroZone.SetActive(true);
                desaggroZone.SetActive(false);
                startPosition = transform.position;

                isAggro = false;
                canMove = false;
                isAttacking = false;
                CJ_PlayerCameraManager.Instance.ennemyList.Remove(gameObject);
                isAvoidingObstacles = false;
            }
            canTakeDamage = false;
            activeAggro = false;
        }

        public void PlaySound(string name)
        {
            AudioManager.Instance.Play(name);
        }

        public void StopSound(string name)
        {
            AudioManager.Instance.Stop(name);
        }
    }
}