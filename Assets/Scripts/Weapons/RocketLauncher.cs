using Unity.VisualScripting;
using UnityEngine;

public class RocketLauncher : Weapon
{
    /** 
    Ici on gère la logique interne d'un Rocket Launcher
    Ex: Cet arme fait tant de recoil quand tu le tire
    **/
    public RocketLauncher(GameObject weapon, GameObject gunPoint, Projectile projectile, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.projectile = projectile;
        this.ammo = ammo;
    }
}