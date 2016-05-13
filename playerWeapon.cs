using UnityEngine;
//create our own playerWeapon class
[System.Serializable] //tells unity how to save/load this class 
public class playerWeapon
{
    public string name = "Handgun";
    public int damage = 15;
    public float range = 80f;
}
