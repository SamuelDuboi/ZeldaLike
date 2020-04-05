using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;

public class SD_Boss2Body : MonoBehaviour
{
     bool isStun;
    public float life;
    float maxLife;
    public GameObject weakPoint;
    public GameObject shield;
    public float stunTime = 8;
    public GameObject bulletPrefab;
    GameObject target;
    public GameObject[] RocketLunchPoint = new GameObject[2];
    float timer;
    float currentLife;
    
    public Image lifeBar;
    // Start is called before the first frame update
    void Start()
    {
        target = SD_PlayerMovement.Instance.gameObject;
        maxLife = life;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stun();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            LunchBullet();
        }
        if (isStun)
        {           
            timer += Time.deltaTime;
            if (timer>= stunTime || life <= currentLife- life/4 )
            {
                isStun = false;
                weakPoint.SetActive(false);
                shield.SetActive(true);
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            life--;
            lifeBar.fillAmount = life / maxLife;
        }
    }


    void LunchBullet()
    {
        GameObject currentBullet = Instantiate(bulletPrefab,
                                                RocketLunchPoint[0].transform.position,
                                                Quaternion.identity);
        currentBullet.GetComponent<SD_Boss2Bullets>().target = target;
        currentBullet.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2( -5,10);
        currentBullet.transform.SetParent(transform);
        GameObject currentBullet2 = Instantiate(bulletPrefab, 
                                                RocketLunchPoint[1].transform.position,
                                                 Quaternion.identity);
        currentBullet2.GetComponent<SD_Boss2Bullets>().target = target;
        currentBullet2.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2(5, 10);
        currentBullet2.transform.SetParent(transform);
    }
    public void Stun()
    {
        isStun = true;
        currentLife = life;
        weakPoint.SetActive(true);
        shield.SetActive(false);
    }
}
