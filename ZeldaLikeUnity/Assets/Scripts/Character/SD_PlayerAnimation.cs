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
        public GameObject halo;
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