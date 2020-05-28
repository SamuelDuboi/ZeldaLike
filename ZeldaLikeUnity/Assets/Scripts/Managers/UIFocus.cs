
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocus : MonoBehaviour
{
    GameObject lastselect;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        lastselect = new GameObject();
    }
    void Update()
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastselect);
            }
            else if (lastselect != EventSystem.current.currentSelectedGameObject)
            {
                lastselect = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
