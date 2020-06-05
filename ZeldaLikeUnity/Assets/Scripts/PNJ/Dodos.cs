using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodos : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer SpriteRenderer;

    bool isActive;
    Vector2 startPosition;
    bool cantRun;
    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        
         if (!cantRun && !isActive)
            StartCoroutine(MovingRandom());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.Play("Grododo_Slap");
        anim.SetTrigger("Hit");
        cantRun = true;
    }

    public void RUn()
    {
        StartCoroutine(Death());
    
    }
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    IEnumerator MovingRandom()
    {
        LayerMask wallMask = 1 << 9;
        isActive = true;

        float randomx = Random.Range(-3f, 3f);
        float randomy = Random.Range(-3f, 3f);

        while (randomx > -0.8f && randomx < 0.8f)
            randomx = Random.Range(-3f, 3f);
        while (randomy > -0.8f && randomy < 0.8f)
            randomy = Random.Range(-3f, 3f);
        randomx += transform.position.x;
        randomy += transform.position.y;


        Vector2 randomPosition = new Vector2(randomx, randomy);
        while (Mathf.Abs(Vector2.Distance(transform.position, randomPosition)) > 0.2f && (Mathf.Abs(Vector2.Distance(startPosition, randomPosition)) < 5))
        {
            if (randomPosition.x - transform.position.x > 0)
            {
                SpriteRenderer.flipX = true;
            }
            else
                SpriteRenderer.flipX = false;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                        new Vector2(randomx - transform.position.x, randomy - transform.position.y),
                                                        2,
                                                        wallMask);
            Debug.DrawRay(transform.position, new Vector2(randomx - transform.position.x, randomy - transform.position.y));
            if (raycastHit.collider != null  ||cantRun)
            {
                break;
            }

            transform.position = Vector2.MoveTowards(transform.position, randomPosition, .01f);

            yield return new WaitForSeconds(0.05f);
        }
        isActive = false;
    }
}
