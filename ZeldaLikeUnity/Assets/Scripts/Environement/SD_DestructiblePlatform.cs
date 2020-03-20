using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SD_DestructiblePlatform : MonoBehaviour
{

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D( Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12)
            StartCoroutine(Destruction());
    }

    IEnumerator Destruction()
    {
        animator.SetTrigger("Fall");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        tag = "Hole";
    }
}
