using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Boss2Arms : MonoBehaviour
{
    public int life;
    GameObject shield;
    GameObject weakpoint;
    bool canTakeDamage;
    public GameObject Laser;
    public float stunTime;
     float timer;
    bool dead;
    // Start is called before the first frame update
    void Start()
    {
        shield = transform.GetChild(0).gameObject;
        weakpoint = transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if(canTakeDamage && !dead)
        {
            timer += Time.deltaTime;
            if(timer >= stunTime)
            {
                canTakeDamage = false;
                shield.SetActive(true);
                weakpoint.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer ==14 && !canTakeDamage)
        {
            shield.SetActive(false);
            canTakeDamage = true;
            weakpoint.GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (collision.gameObject.layer == 8 && canTakeDamage)
        {
            life--;
            if (life<= 0)
            {
                SD_Boss2Body.Instance.life -= SD_Boss2Body.Instance.maxLife / 8;
                SD_Boss2Body.Instance.lifeBar.fillAmount = SD_Boss2Body.Instance.life/ SD_Boss2Body.Instance.maxLife;
                Destroy(Laser);
                weakpoint.SetActive(false);
                GetComponent<SpriteRenderer>().color = Color.black;
                dead = true;
            }
        }
    }
}
