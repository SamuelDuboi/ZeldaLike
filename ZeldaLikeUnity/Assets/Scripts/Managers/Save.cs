using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
 public class Save 
 {
    public float playerPositionX;
    public float playerPositionY;
    public float pvMax;
    public float currentPv;
    public float power;
    public int scenceIndex;
    public bool hasWind;
    public bool canParry;

    [HideInInspector] public int Alyah1;
    [HideInInspector] public int Alyah2;
    [HideInInspector] public int Henry1;
    [HideInInspector] public int Henry2;
    [HideInInspector] public int WindMother;
    [HideInInspector] public int Pepe;

}
