using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;
public class SD_DestructiblePlatform : MonoBehaviour
{

    Animator animator;
    public bool onePath;
    bool destruct;
    [Range(0,100)]
    public float respawnCooldown = 5;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D( Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
           
            StartCoroutine(Destruction());
            SD_PlayerMovement.Instance.isOnPlatformDestructible = true;
            SD_PlayerMovement.Instance.ChoosePosition(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            SD_PlayerMovement.Instance.isOnPlatformDestructible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 && destruct == false)
        {
            SD_PlayerMovement.Instance.isOnPlatformDestructible = false;
        }
    }
    bool isActive;
    IEnumerator Destruction()
    {
        if (!isActive)
        {
            AudioManager.Instance.Play("Platform_Destruction");
            isActive = true;
            animator.SetTrigger("Fall");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
            DestructiblePlateformeManager.Instance.platfromDestroyed.Add(gameObject);
            destruct = true;
            tag = "Hole";
            SD_PlayerMovement.Instance.isAbleToRunOnHole = false;
            if (!onePath)
            {
                yield return new WaitForSeconds(respawnCooldown);
                animator.SetTrigger("Respawn");
                tag = "DestroyedPlatform";
                DestructiblePlateformeManager.Instance.platfromDestroyed.Remove(gameObject);
            }
            isActive = false;
            destruct = false;
        }
        
    }
    public void SelfReset()
    {
        StopAllCoroutines();
        animator.SetTrigger("Respawn");
        tag = "DestroyedPlatform";
        isActive = false;
        destruct = false;
    }
}
