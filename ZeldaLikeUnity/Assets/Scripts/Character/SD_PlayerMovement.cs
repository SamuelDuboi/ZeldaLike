using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Ennemy;



namespace Player
{
    /// <summary>
    /// The Movement of the player's avatar, can be change du to the public fonction speed
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SD_PlayerMovement : Singleton<SD_PlayerMovement>
    {
        // speed of the player
        [Range(0, 10)]
        public float speed;
        [HideInInspector] public float initialSpeed;
        //inputs on x and y axis
        float XAxis;
        float YAxis;
        float sprint = 1f;
        [Range(1,2)]
        public float sprintForce;
        [HideInInspector] public Rigidbody2D playerRGB;

        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce ;
        [Range(0, 10)]
        public float dashCooldown;
        public int fallDamage;
        //enable movement on false
       public bool cantMove;

         public bool cantDash;

        [HideInInspector] public bool hasWindShield;

        bool dashIsActive;
        bool wind;

        public GameObject windPlatform;
        [HideInInspector] public bool isAbleToRunOnHole;
        bool canSpawnPlatform;
        [HideInInspector] public bool platformIsSpawned;
        public float platformLifeTime;
        GameObject currentPlatform;

        [Range(8,11)]
        public float inertieAfterDash;
        Vector2 playerRespawnAfterFall;
        void Awake()
        {
            MakeSingleton(false);
        }
        void Start()
        {
            initialSpeed = speed;
            playerRGB = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            //get the input of the player raw (-1,0 or 1) and multply it by speed and then make the velocity equal to it
            if (!cantMove && Input.GetAxisRaw("Horizontal") != 0  && !wind || !cantMove && Input.GetAxisRaw("Vertical") != 0 && !wind )
            {
                if (Input.GetAxis("Horizontal") >0.6f)
                XAxis = Input.GetAxisRaw("Horizontal");
                else
                    XAxis = Input.GetAxis("Horizontal");
                if (Input.GetAxis("Vertical") > 0.6f)
                    YAxis = Input.GetAxisRaw("Vertical");
                else
                    YAxis = Input.GetAxis("Vertical");
                Move();
            }
            else
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", false);
                sprint = 1;
                playerRGB.drag = 10;
            }


        }
        private void Update()
        {
            // to dash
            if (Input.GetButtonDown("Dash"))
            {
                StartCoroutine(Dash());
            }
        }


        public void Move()
        {
            SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", !cantMove);
            playerRGB.velocity = new Vector2(XAxis, YAxis) * speed*sprint;
            if (XAxis < 0.1 && XAxis > -0.1 && YAxis > 0.1)                
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0f);
            }
            else if (XAxis < 0.1 && XAxis > -0.1 && YAxis < -0.1)
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", -1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0f);

            }               
            else if (XAxis >= 0.1f)
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0f);
            }                
            else if (XAxis <= 0.1)
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", -1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0f);

            }
                


        }
        /// <summary>
        /// Make the player dash forward
        /// </summary>
        /// <returns></returns>
        IEnumerator Dash()
        {
            if (!cantDash)
            {
                if (!dashIsActive)
                {
                    // the player can't dash during a dash
                    dashIsActive = true;
                    //cancel of the current attack if they was an attack
                    StartCoroutine(SD_PlayerAttack.Instance.Cancel(0f));
                    yield return new WaitForSeconds(0.01f);
                    // reset of the speed in case the player was attacking and so, speed was reduce
                    speed = initialSpeed;
                    // add a force for the dash
                    speed *= dashForce;
                    Move();
                    cantMove = true;
                    cantDash = true;
                    yield return new WaitForSeconds(dashTime);
                    // end of the dash, reset of the speed, the player can move and the player can dash again
                    
                  
                    if (canSpawnPlatform && !platformIsSpawned)
                    {
                       currentPlatform= Instantiate(windPlatform, new Vector2( transform.position.x + playerRGB.velocity.normalized.x*0.2f, transform.position.y +playerRGB.velocity.normalized.y*0.2f), Quaternion.identity);
                        canSpawnPlatform = false;
                        platformIsSpawned = true;
                    }
                    
                    yield return new WaitForSeconds(0.1f);
                    speed = initialSpeed;
                    cantMove = false;
                    dashIsActive = false;
                    sprint = sprintForce;
                    playerRGB.drag = 10/ inertieAfterDash;
                    yield return new WaitForSeconds(dashCooldown);
                    cantDash = false;
                }

            }

        }

       private void OnTriggerEnter2D(Collider2D collision)
        { 
                if (collision.gameObject.tag == "Wind" && collision.gameObject.layer == 9)
            {
                wind = true;
                cantMove = true;
                cantDash = true;
            }
            else if (collision.gameObject.tag == "Wall" && wind)
            {
                wind = false;
                cantMove = false;
                cantDash = false;
                Debug.Log("Wall");
            }
            if (collision.gameObject.tag == "Hole")
                playerRespawnAfterFall = new Vector2( transform.position.x -XAxis*0.5f, transform.position.y - YAxis*0.5f);

        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "WindPlatform")
                isAbleToRunOnHole = true;
            if (collision.gameObject.tag == "Hole")
                if (!isAbleToRunOnHole && !dashIsActive)
                    StartCoroutine(Fall(collision));
                else if (SD_PlayerAttack.Instance.hasWind)
                    canSpawnPlatform = true;

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wind" && wind)
            {
                wind = false;
                cantMove = false;
                cantDash = false;
            }
            else if (collision.gameObject.tag == "Wall" && !wind)
            {
                wind = true;
                cantMove = true;
                cantDash = true;
            }
            if (isAbleToRunOnHole && collision.tag == "WindPlatform" || collision.tag == "Hole")
                isAbleToRunOnHole = false;
            if (collision.tag == "Hole" && currentPlatform != null)
            {
                platformIsSpawned = false;
                Destroy(currentPlatform);
            }
        }

     



        IEnumerator Fall(Collider2D collisionPoint)
        {
              

           if (!dashIsActive)
            {
                

                cantDash = true;
                cantMove = true;
                SD_PlayerRessources.Instance.cantTakeDamage = true;
                SD_PlayerAttack.Instance.cantAttack = true;
                playerRGB.simulated = false;

                for (int i = 0; i < 50; i++)
                {
                    Vector2 reduction = Vector2Extensions.addVector(SD_PlayerAnimation.Instance.gameObject.transform.localScale, -new Vector2(0.02f, 0.02f));
                    SD_PlayerAnimation.Instance.gameObject.transform.localScale = reduction;
                    yield return new WaitForSeconds(0.001f);
                }
                SD_PlayerAnimation.Instance.gameObject.transform.localScale = Vector2.one;
                speed = 0;
                if (!isAbleToRunOnHole)
                    transform.position = new Vector2(playerRespawnAfterFall.x , playerRespawnAfterFall.y );                
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(fallDamage, collisionPoint.gameObject, false,1));
                speed = initialSpeed;
                playerRGB.simulated = true;
                cantDash = false;
                cantMove = false;
                SD_PlayerRessources.Instance.cantTakeDamage = false;
                SD_PlayerAttack.Instance.cantAttack = false;
            }
            
        }
    }
}