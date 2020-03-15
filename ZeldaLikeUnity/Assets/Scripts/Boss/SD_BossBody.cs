using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;

public class SD_BossBody : MonoBehaviour
{
    int weakPointNumber;
    public GameObject bullet;
    Collider2D bossCollider;
    Vector2 firstBullet;
    public float bulletNummber;


    void Start()
    {
        weakPointNumber = transform.childCount;
        bossCollider = GetComponent<Collider2D>();
        firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject + "Touche");
        if (collision.gameObject.layer == 8)
        {
            Collider2D[] currentWeakPoint = GetComponentsInChildren<Collider2D>();
            foreach(Collider2D collider in currentWeakPoint)
            {
                if (collider.IsTouching(collision))
                {
                    weakPointNumber--;
                    Destroy(collider.gameObject);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Fire();
    }
    void Fire()
    {
        Vector2 formerBulletPosition = firstBullet;
        while(formerBulletPosition.x <= firstBullet.x + bossCollider.bounds.size.x)
        {
            Vector2 newBulletPosition = new Vector2(formerBulletPosition.x + bossCollider.bounds.size.x / bulletNummber, formerBulletPosition.y);
           GameObject newBullet = Instantiate(bullet, newBulletPosition, Quaternion.identity);
            newBullet.GetComponent<CJ_BulletBehaviour>().target = new Vector2(newBulletPosition.x, newBulletPosition.y - 10f);
            newBullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
            
            formerBulletPosition = newBulletPosition;
        }
       
    }
}
