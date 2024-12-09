using System;
using Unity.VisualScripting;
using UnityEngine;

public class Flamethrower : Weapon
{
    /** 
    Ici on gère la logique interne d'un Rocket Launcher
    Ex: Cet arme fait tant de recoil quand tu le tire
    **/
    private float _verticalBoostStrength;
    private float _horizontalBoostStrength;
    private float _baseVerticalBoostStrength;
    public Flamethrower(GameObject weapon, GameObject gunPoint, int ammo, float verticalBoostStrength, float horizontalBoostStrength, float baseVerticalBoostStrength = 9.81f)
    {
        this.weaponObject = weapon;
        this.gunPoint = gunPoint;
        this.ammo = ammo;
        this._verticalBoostStrength = verticalBoostStrength;
        this._horizontalBoostStrength = horizontalBoostStrength;
        this._baseVerticalBoostStrength = baseVerticalBoostStrength;
    }

    public float getVerticalBoostStrength(Vector3 playerLinearVel)
    {
        // For hover effect
        // If player is looking straight down - hover
        // If the player wants to move - you're gonna lose some altitude
        if (playerLinearVel.y > 0 || gunPoint.transform.up.y >= 0)
        {
            // player ascending or player facing up - boost normally
            return -_verticalBoostStrength;
        }
        // If the player is looking straight down - hover without gaining height
        // Otherwise the player will lose height proportionally to how horizontally they are looking
        Debug.Log(gunPoint.transform.up.y + " " + playerLinearVel.y + " " + _verticalBoostStrength + " " + _baseVerticalBoostStrength);
        // Debug.Log(gunPoint.transform.up.y * (playerLinearVel.y * _verticalBoostStrength + _baseVerticalBoostStrength));
        return (Math.Abs(gunPoint.transform.up.y) * (Math.Abs(playerLinearVel.y) * _verticalBoostStrength + _baseVerticalBoostStrength));
    }

    public float getHotizontalBoostStrength()
    {
        return _horizontalBoostStrength;
    }
}