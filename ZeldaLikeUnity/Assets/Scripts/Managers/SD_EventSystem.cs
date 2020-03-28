using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SD_EventSystem : MonoBehaviour
{
    // Start is called before the first frame update
  public  GameObject death;
   public GameObject pause;


    // Update is called once per frame
    void Update()
    {
       
    }
    public void ChangePanel()
    {
        if (pause.activeInHierarchy)
        {
            GetComponent<EventSystem>().SetSelectedGameObject(pause);
        }
         if (death.activeInHierarchy)
            GetComponent<EventSystem>().SetSelectedGameObject(death);
    }
}
