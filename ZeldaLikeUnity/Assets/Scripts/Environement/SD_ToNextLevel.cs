using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Management;
public class SD_ToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManagerV2.Instance.Saving(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
            

    }
}
