using UnityEngine;

public class GravityLauncher : Weapon
{
    /** 
     This class contains the data of the gravity gun.
     **/
    public GravityLauncher(GameObject weapon, GameObject gunPoint, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
    }
}
