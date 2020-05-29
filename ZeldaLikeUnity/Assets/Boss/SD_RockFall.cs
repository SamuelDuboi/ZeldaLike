using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_RockFall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 5);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, transform.parent.position, 5 * Time.deltaTime);
        if (transform.position.y <= transform.parent.position.y + 0.5f)
        {

            GetComponent<BoxCollider2D>().enabled = true;
            Destroy(this);
        }
    }
}
