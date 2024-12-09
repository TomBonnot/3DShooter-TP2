using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : Weapon
{
    /** 
    Ici on gère la logique interne d'un Rocket Launcher
    Ex: Cet arme fait tant de recoil quand tu le tire
    **/
    public RocketLauncher(GameObject weapon, GameObject gunPoint, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
    }
}