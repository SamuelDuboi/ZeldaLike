using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCOmb : MonoBehaviour
{
    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            gameObject.SetActive(false);
            timer = 0;
        }
    }


}
