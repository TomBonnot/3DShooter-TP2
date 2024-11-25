using UnityEngine;

public class GravityLauncher : Weapon
{
    /** 
     Ici on gère la logique interne d'un inverseur de gravité
     Ex: Cet arme fait tant de recoil quand tu le tire
     **/
    public GravityLauncher(GameObject weapon, GameObject gunPoint, Projectile projectile, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.projectile = projectile;
        this.ammo = ammo;
    }
}
