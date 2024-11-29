using Unity.VisualScripting;
using UnityEngine;

public class Grappler : Weapon
{
    /** 
    Ici on gère la logique interne d'un Grapple
    **/
    public Grappler(GameObject weapon, GameObject gunPoint, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
    }
}