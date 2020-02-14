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
        [Range(0, 10)]
        public int speed;
        float XAxis;
        float YAxis;
        Rigidbody2D playerRGB;

        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce;

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
            if (!cantMove)
            {
                XAxis = Input.GetAxisRaw("Horizontal");
                YAxis = Input.GetAxisRaw("Vertical");

                Move();
            }

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
                isActive = true;
                
                StartCoroutine(SD_PlayerAttack.Instance.Cancel(0f));
                
                yield return new WaitForSeconds(0.01f);
                speed = initialSpeed;
                speed *= dashForce;
                Move();

                cantMove = true;
                
                yield return new WaitForSeconds(dashTime);
                speed = initialSpeed;
                cantMove = false;
                isActive = false;
            }

        }
    }
}