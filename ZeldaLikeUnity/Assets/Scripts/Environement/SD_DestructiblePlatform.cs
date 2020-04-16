using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;
public class SD_DestructiblePlatform : MonoBehaviour
{

    Animator animator;
    public bool onePath;
    [Range(0,100)]
    public float respawnCooldown = 5;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D( Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12)
            StartCoroutine(Destruction());
    }
    bool isActive;
    IEnumerator Destruction()
    {
        if (!isActive)
        {
            isActive = true;
            animator.SetTrigger("Fall");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
            tag = "DestroyedPlatform";
            SD_PlayerMovement.Instance.isAbleToRunOnHole = false;
            if (!onePath)
            {
                yield return new WaitForSeconds(respawnCooldown);
                animator.SetTrigger("Respawn");
                tag = "Untagged";
            }
            isActive = false;
        }
        
    }
}
