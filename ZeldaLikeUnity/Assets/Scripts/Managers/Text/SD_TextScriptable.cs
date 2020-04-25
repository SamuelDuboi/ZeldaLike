using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Text")]
public class SD_TextScriptable : ScriptableObject
{
    public Sprite ImageCharacter;
    [TextArea]
    public string text;
    public enum character { alyah1,alyah2,Henry1,Henry2,WindMother,Pepe,Note }
    public character pnj;
}
