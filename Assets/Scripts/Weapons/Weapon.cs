using UnityEngine;

public abstract class Weapon
{
    /** 
    Ici on gère la logique interne d'un Weapon en général
    Ex: Quand on essaie de tirer et on a pas de ammo, fait un son
    Il aura surement très peu de actual code dans ces fichiers
    **/

    public Projectile projectile; // the projectile that will be shot from the weapon
    public GameObject weaponObject;
    public GameObject gunPoint;
    public int ammo;

    public bool expendsAmmo()
    {
        // Returns true if shot ammo, false if there is no ammo left
        Debug.Log("Ammo: " + ammo.ToString());
        if (ammo > 0)
        {
            ammo -= 1;
            return true;
        }
        return false;
    }
}