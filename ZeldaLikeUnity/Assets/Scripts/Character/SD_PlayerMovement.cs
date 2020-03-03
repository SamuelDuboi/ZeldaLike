﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;



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
        float initialSpeed;
        //inputs on x and y axis
        float XAxis;
        float YAxis;

        [HideInInspector] public Rigidbody2D playerRGB;

        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce ;

        public int fallDamage;
        //enable movement on false
        [HideInInspector] public bool cantMove;

        [HideInInspector] public bool cantDash;


        bool isActive;
        bool wind;
        void Start()
        {
            MakeSingleton(true);
            initialSpeed = speed;
            playerRGB = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            //get the input of the player raw (-1,0 or 1) and multply it by speed and then make the velocity equal to it
            if (!cantMove && Input.GetAxisRaw("Horizontal") != 0 && !wind || !cantMove && Input.GetAxisRaw("Vertical") != 0 && !wind)
            {
                XAxis = Input.GetAxisRaw("Horizontal");
                YAxis = Input.GetAxisRaw("Vertical");

                Move();
            }
            else
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", false);
           

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
            playerRGB.velocity = new Vector2(XAxis, YAxis) * speed;
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
                if (!isActive)
                {
                    // the player can't dash during a dash
                    isActive = true;
                    //cancel of the current attack if they was an attack
                    StartCoroutine(SD_PlayerAttack.Instance.Cancel(0f));
                    yield return new WaitForSeconds(0.01f);
                    // reset of the speed in case the player was attacking and so, speed was reduce
                    speed = initialSpeed;
                    // add a force for the dash
                    speed *= dashForce;
                    Move();
                    cantMove = true;
                    yield return new WaitForSeconds(dashTime);
                    // end of the dash, reset of the speed, the player can move and the player can dash again
                    speed = initialSpeed;
                    cantMove = false;
                    isActive = false;
                }

            }

        }

       private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wind" && collision.gameObject.layer == 9 )
            {
                wind = true;       
                cantMove = true;
                cantDash = true;

                
            }
            else if(collision.gameObject.tag == "Wall" && wind)
            {
                wind = false;
                cantMove = false;
                cantDash = false;
                Debug.Log("Wall");
            }
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
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Hole")
                StartCoroutine(Fall(collision));
            
        }



        IEnumerator Fall(Collision2D collisionPoint)
        {
            cantDash = true;
            cantMove = true;
            SD_PlayerLife.Instance.cantTakeDamage = true;
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
            Vector2 bouncePoint =  new Vector2(collisionPoint.GetContact(0).point.x - transform.position.x, collisionPoint.GetContact(0).point.y - transform.position.y+0.5f);
            transform.position = new Vector2(transform.position.x - bouncePoint.x, transform.position.y - bouncePoint.y);
            StartCoroutine(SD_PlayerLife.Instance.TakingDamage(fallDamage, collisionPoint.gameObject, false));
            speed = initialSpeed;
            playerRGB.simulated = true;
            cantDash = false;
            cantMove = false;
            SD_PlayerLife.Instance.cantTakeDamage = false;
            SD_PlayerAttack.Instance.cantAttack = false;
        }
    }
}