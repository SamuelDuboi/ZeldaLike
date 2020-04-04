using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Boss2Body : MonoBehaviour
{
    bool isStun;
    public int life;
    public GameObject weakPoint;
    public float stunTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Stun());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            life--;
        }
    }

    public IEnumerator Stun()
    {
        isStun = true;
        weakPoint.SetActive(true);
        yield return new WaitForSeconds(stunTime);
        isStun = false;
        weakPoint.SetActive(false);

    }
}
