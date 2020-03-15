using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;


namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class SD_PlayerAnimation : Singleton<SD_PlayerAnimation>
    {
        public SpriteRenderer sprite;
       [HideInInspector] public Animator PlayerAnimator;
        public AnimationClip[] attackAnimation = new AnimationClip[3];
        void Awake()
        {
            MakeSingleton(false);
        }
        void Start()
        {
            PlayerAnimator = GetComponent<Animator>();
            sprite = GetComponent<SpriteRenderer>();           
        }

    }
}