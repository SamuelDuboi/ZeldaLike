using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;



namespace Player
{
    /// <summary>
    /// The Movement of the player's avatar, can be change du to the public fonction speed
    /// </summary>
    [RequireComponent (typeof(Rigidbody2D))]
    public class SD_PlayerMovement : Singleton<SD_PlayerMovement>
    {
        // speed of the player
        [Range(0, 10)]
        public int speed;
        //inputs on x and y axis
        float XAxis;
        float YAxis;
        
        Rigidbody2D playerRGB;

        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce;

        //enable movement on false
        [HideInInspector] public bool cantMove;

        int initialSpeed;

        bool isActive;
        void Start()
        {
            initialSpeed = speed;
            MakeSingleton(true);
            playerRGB = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            //get the input of the player raw (-1,0 or 1) and multply it by speed and then make the velocity equal to it
            if (!cantMove)
            {
                XAxis = Input.GetAxisRaw("Horizontal");
                YAxis = Input.GetAxisRaw("Vertical");

                Move();
            }
            // to dash
            if (Input.GetKeyDown(KeyCode.V))
            {
               StartCoroutine( Dash());
            }
        }


        public void Move()
        {
            playerRGB.velocity = new Vector2(XAxis, YAxis) * speed;

        }

        IEnumerator  Dash()
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
}