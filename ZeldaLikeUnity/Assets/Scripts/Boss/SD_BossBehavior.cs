using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class SD_BossBehavior : Singleton<SD_BossBehavior>
{
    public int phaseNumber = 1;
    public bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
    }


}
