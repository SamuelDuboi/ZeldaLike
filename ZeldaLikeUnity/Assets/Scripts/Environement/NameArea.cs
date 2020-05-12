using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameArea : MonoBehaviour
{
  [HideInInspector]  public bool activated;
    [TextArea]
    public string nameArea;
    void Start()
    {
        GameManagerV2.Instance.nameAreas.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.layer == 10 && !activated)
        {
            activated = true;
            StartCoroutine(GameManagerV2.Instance.NameAppearD(nameArea, this));
        }
    }
}
