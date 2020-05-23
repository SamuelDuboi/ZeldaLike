using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuIntro : MonoBehaviour
{
    public Button[] button;
    private IEnumerator Start()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        foreach (Button but in button)
            but.interactable = false;
        yield return new WaitForSeconds(3f);
        for(float i = 1; i>0.05; i-=0.01f)
        {
            sprite.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.02f);
        }
        foreach (Button but in button)
            but.interactable = true;
        Destroy(gameObject);
    } 


}
