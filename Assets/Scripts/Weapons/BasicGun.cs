using Unity.VisualScripting;
using UnityEngine;

public class BasicGun : Weapon
{
    /** 
    Ici on gère la logique interne d'un BasicGun
    **/
    public BasicGun(GameObject weapon, GameObject gunPoint, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
    }
}