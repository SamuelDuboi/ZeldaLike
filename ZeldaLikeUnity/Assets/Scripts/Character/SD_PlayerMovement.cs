﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Ennemy;
using UnityEngine.UI;


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
       [HideInInspector] public float XAxis;
        [HideInInspector] public float YAxis;
        [HideInInspector] public float sprint = 1f;
        [Range(1, 2)]
        public float sprintForce;
        [HideInInspector] public Rigidbody2D playerRGB;

        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce;
        [Range(0, 10)]
        public float dashCooldown;
        public int fallDamage;
        //enable movement on false
        [HideInInspector] public bool cantMove;

        [HideInInspector] public bool cantDash;

        [HideInInspector] public bool hasWindShield;

        [HideInInspector] public bool dashIsActive;
        bool wind;

        public GameObject windPlatform;
         public bool isAbleToRunOnHole;
        bool canSpawnPlatform;

        public float platformLifeTime;
        GameObject currentPlatform;
        public int platformNumber = 1;

        [Range(8, 11)]
        public float inertieAfterDash;
        Vector2 playerRespawnAfterFall;
        [Range(0,1)]
        public float timeBeforAbleToMoveAfterFall;

       [HideInInspector]  public int keyNumber;
        public GameObject[] keyUI;
        bool positionForDestroyedPlatformIsAlreadyChose;


        GameObject fade;
        bool canWind;

        [Space]
        [Header("Fire")]
        float timer;
        public int burnStade;
        float damageTimer;
        GameObject fire;
        [Range(0, 5)]
        public float timeBeforBurning = 2f;
        [Range(0, 5)]
        public float timeBetweenBurn = 1f;
        void Awake()
        {
            MakeSingleton(false);
        }
        void Start()
        {
            initialSpeed = speed;
            playerRGB = GetComponent<Rigidbody2D>();
            fade = GameObject.FindGameObjectWithTag("Fade");
            fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        void FixedUpdate()
        {
            //get the input of the player raw (-1,0 or 1) and multply it by speed and then make the velocity equal to it
            if (!cantMove && Input.GetAxisRaw("Horizontal") != 0 && !wind || !cantMove && Input.GetAxisRaw("Vertical") != 0 && !wind)
            {
                if (Input.GetAxis("Horizontal") > 0.6f)
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
            if (burnStade > 0)
            {
                timer += Time.deltaTime;
                if (timer >= timeBeforBurning)
                {
                    burnStade = 2;
                }
            }

            if (burnStade == 2)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= timeBetweenBurn)
                {
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(1, fire, false, 1));
                    damageTimer = 0;
                    timer = 0;
                }
            }
        }


        public void Move()
        {
            SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", !cantMove);
            playerRGB.velocity = new Vector2(XAxis, YAxis) * speed * sprint;
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
      public  IEnumerator Dash()
        {
            if (!cantDash)
            {
                if (!dashIsActive)
                {
                    Debug.Log( new Vector2( XAxis, YAxis));
                    // the player can't dash during a dash
                    dashIsActive = true;
                    //cancel of the current attack if they was an attack
                    StartCoroutine(SD_PlayerAttack.Instance.Cancel(0f));
                    yield return new WaitForSeconds(0.01f);
                    // reset of the speed in case the player was attacking and so, speed was reduce
                    speed = initialSpeed;
                    // add a force for the dash
                    speed *= dashForce;
                    sprint = 1;
                  /*  if (Mathf.Abs(XAxis) < 0.6f && Mathf.Abs(YAxis) < 0.6f)
                    {

                        if (Mathf.Abs(XAxis) > Mathf.Abs(YAxis))
                        {
                            if (XAxis < 0)
                                XAxis = -1;
                            else
                                XAxis = 1;
                        }
                        else
                        {
                            if (YAxis < 0)
                                YAxis = -1f;
                            else
                                YAxis = 1;
                        }
                    }*/
                    playerRGB.velocity = new Vector2(XAxis, YAxis).normalized * speed * sprint;
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetTrigger("Dash");
                    cantMove = true;
                    cantDash = true;

                    timer = 0;
                    burnStade--;
                    if (burnStade <= 0)
                        burnStade = 0;

                    yield return new WaitForSeconds(dashTime);
                    // end of the dash, reset of the speed, the player can move and the player can dash again


                    if (canSpawnPlatform && platformNumber > 0)
                    {
                        currentPlatform = Instantiate(windPlatform, new Vector2(transform.position.x + playerRGB.velocity.normalized.x * 0.2f, transform.position.y + playerRGB.velocity.normalized.y * 0.2f), Quaternion.identity);
                        canSpawnPlatform = false;

                        platformNumber--;
                    }

                    yield return new WaitForSeconds(0.1f);
                    speed = initialSpeed;
                    cantMove = false;
                    dashIsActive = false;
                    sprint = sprintForce;
                    playerRGB.drag = 10 / inertieAfterDash;
                    yield return new WaitForSeconds(dashCooldown);
                    cantDash = false;
                }

            }

        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                if (wind)
                {
                    cantMove = false;
                    cantDash = false;
                    wind = false;
                    Debug.LogError("ca touche le mur");
                    SD_PlayerAttack.Instance.cantAttack = false;

                }

            }
            if (collision.gameObject.tag == "Wind" && collision.gameObject.layer == 9)
            {
                wind = true;
                cantMove = true;
                cantDash = true;

                SD_PlayerAttack.Instance.cantAttack = true;
                if (SD_PlayerAttack.Instance.hasWind)
                    canWind = true;
                SD_PlayerAttack.Instance.hasWind = false; 
            }
            
            else if (collision.gameObject.layer == 18)
            {
                
                 if (collision.gameObject.tag == "Key")
                {
                    keyNumber ++;
                    keyUI[keyNumber -1].SetActive(true);
                    Destroy(collision.gameObject);
                }
            }
            if (collision.gameObject.tag == "Hole")
            {
                playerRespawnAfterFall = new Vector2(transform.position.x - XAxis * 0.5f, transform.position.y - YAxis * 0.5f);
                isAbleToRunOnHole = false;
            }
                
            if (collision.gameObject.tag == "DestroyedPlatform" && !positionForDestroyedPlatformIsAlreadyChose)
            {
                playerRespawnAfterFall = new Vector2(transform.position.x - XAxis * 0.5f, transform.position.y - YAxis * 0.5f);
                positionForDestroyedPlatformIsAlreadyChose = true;
            }

        }
        private void OnTriggerStay2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "WindPlatform")
                isAbleToRunOnHole = true;
            if (collision.gameObject.tag == "Hole" || collision.gameObject.tag == "DestroyedPlatform")
                if (!isAbleToRunOnHole && !dashIsActive)
                {
                    StartCoroutine(Fall(collision));
                }
                else if (SD_PlayerAttack.Instance.hasWind)
                    canSpawnPlatform = true;

            if (collision.tag == "Fire")
            {
                if (burnStade == 0)
                {
                    burnStade++;
                    timer = 0;
                    fire = collision.gameObject;
                }
                if(!collision.GetComponent<Animator>().GetBool("Burning"))
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(1, fire, false, 5));
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wind" && wind)
            {
                wind = false;
                cantMove = false;
                cantDash = false;
                SD_PlayerAttack.Instance.cantAttack = false; 
                if (canWind)
                SD_PlayerAttack.Instance.hasWind = true;
            }
            if (isAbleToRunOnHole && collision.tag == "WindPlatform" || collision.tag == "Hole")
                isAbleToRunOnHole = false;
            if (collision.tag == "Hole" && currentPlatform != null)
            {
                StartCoroutine(PlatfromCantSwpanAfterTrigger());
                Destroy(currentPlatform);
            }

            if (collision.tag == "Fire")
            {
                if (burnStade != 2)
                {
                    burnStade = 0;
                    timer = 0;
                }

            }
        }


        IEnumerator PlatfromCantSwpanAfterTrigger()
        {
            if(dashIsActive)
            {
                yield return new WaitForSeconds(0.2f);
            }
           
            platformNumber = 1;
        }


        IEnumerator Fall(Collider2D collisionPoint)
        {


            if (!dashIsActive)
            {


                cantDash = true;
                cantMove = true;
                SD_PlayerAttack.Instance.cantAttack = true;
                playerRGB.simulated = false;
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fall", true);
                for (float i = 0; i < 50; i++)
                {
                    fade.GetComponent<Image>().color = new Color(0, 0, 0, i/50);
                    Debug.Log(fade.GetComponent<Image>().color.a);
                    Vector2 reduction = Vector2Extensions.addVector(SD_PlayerAnimation.Instance.gameObject.transform.localScale, -new Vector2(0.02f, 0.02f));
                    SD_PlayerAnimation.Instance.gameObject.transform.localScale = reduction;
                    yield return new WaitForSeconds(0.01f);
                }
                fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fall", false);
                SD_PlayerAnimation.Instance.gameObject.transform.localScale = Vector2.one;
                speed = 0;
                if (!isAbleToRunOnHole)
                    transform.position = new Vector2(playerRespawnAfterFall.x, playerRespawnAfterFall.y);
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(fallDamage, collisionPoint.gameObject, false, 1));
                speed = initialSpeed;
                playerRGB.simulated = true;
                yield return new WaitForSeconds(0.2f);
                playerRGB.velocity = Vector2.zero;
                cantDash = true;
                cantMove = true;
                SD_PlayerAttack.Instance.cantAttack = true;
                SD_PlayerRessources.Instance.cantTakeDamage = true;
                yield return new WaitForSeconds(timeBeforAbleToMoveAfterFall);
                cantDash = false;
                cantMove = false;
                SD_PlayerAttack.Instance.cantAttack = false;
                SD_PlayerRessources.Instance.cantTakeDamage = false;
            }

        }

    }
}