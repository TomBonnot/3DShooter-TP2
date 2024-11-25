using UnityEngine;

public class GravityLauncher : Weapon
{
    /** 
     Ici on g�re la logique interne d'un inverseur de gravit�
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
