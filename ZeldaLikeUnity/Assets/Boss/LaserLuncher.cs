using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLuncher : MonoBehaviour
{
    public GameObject laser;
    public void Shoot()
    {
        Instantiate(laser, transform.position, Quaternion.identity);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
