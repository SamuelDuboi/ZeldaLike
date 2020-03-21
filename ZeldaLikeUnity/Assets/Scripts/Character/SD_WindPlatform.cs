using System.Collections;
using UnityEngine;
using Player;


public class SD_WindPlatform : MonoBehaviour
{
    BoxCollider2D collider;
    SpriteRenderer spriteRenderer;
    float opacity = 1;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Destruction(SD_PlayerMovement.Instance.platformLifeTime));
    }


    IEnumerator Destruction(float timer)
    {
        yield return new WaitForSeconds(2f);
        while (collider.size.x >= 0.05f || collider.size.y >= 0.05f)
        {
            collider.size -= new Vector2(0.1f, 0.1f);
            opacity -= 0.1f;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);

            yield return new WaitForSeconds(timer / 10);
        }
        
        //SD_PlayerMovement.Instance.isAbleToRunOnHole = false;
        Destroy(gameObject);

    }
}
