using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using Cinemachine;
public class BossEnding : Singleton<BossEnding>
{
    public GameObject wall;
    Animator animator;
    public int cpt;
    public GameObject cameraPlayer;
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

        cameraPlayer.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 5.6f;
        Destroy(wall);
    }
}
