using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class DashButton : MonoBehaviour
{
    GameObject interactButton;
    public void Start()
    {
        interactButton = transform.GetChild(0).gameObject;
        
    }
    // Start is called before the first frame update

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            interactButton.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            interactButton.SetActive(true);
            interactButton.transform.SetParent(SD_PlayerMovement.Instance.transform);
            interactButton.transform.position = new Vector2(interactButton.transform.parent.position.x,
                                                            interactButton.transform.parent.position.y + 1);
        }
    }
}
