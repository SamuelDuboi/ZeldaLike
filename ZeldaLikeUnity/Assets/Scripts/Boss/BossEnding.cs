using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
public class BossEnding : Singleton<BossEnding>
{
    public GameObject wall;
    Animator animator;
    public int cpt;
    private void Awake()
    {
        MakeSingleton(false);
        animator = GetComponent<Animator>();
    }

   public void AnimatorLunch()
    {
        cpt++;
        if(cpt>= 4)
        {
            animator.SetTrigger("On");
        }
    }

    public void Shaking()
    {
        StartCoroutine(GameManagerV2.Instance.GamePadeShake(1, 5f));
    }
    public void DestroyWall()
    {
        Destroy(wall);
    }
}
