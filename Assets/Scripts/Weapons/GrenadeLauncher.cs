using UnityEngine;

public class GrenadeLauncher : Weapon
{
    /** 
      This class contains the data of the grenade launcher.
      **/
    public GrenadeLauncher(GameObject weapon, GameObject gunPoint, int ammo)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
    }
}
