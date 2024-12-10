using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class GrapplingBehavior : WeaponBehavior
{
    [Header("References")]
    private LineRenderer _lr;
    private GameObject _player;
    //private Controller _playerController;

    [Header("Swinging")]
    private Vector3 _swingPoint;
    private SpringJoint _joint;

    [Header("Grappling")]
    private Vector3 _hookPoint;
    private Grappler _grappler;
    private GameObject _shotGrapple;
    private GrappleBehavior _shotGrappleBehavior;

    [Header("SpringJoint Physics")]
    [Range(0, 1)][SerializeField] private float _jointMaxDistance = 0.8f;
    [Range(0, 1)][SerializeField] private float _jointMinDistance = 0.25f;
    [Range(1, 10)][SerializeField] private float _jointSpring = 4.5f;
    [Range(1, 10)][SerializeField] private float _jointDamper = 7f;
    [Range(1, 10)][SerializeField] private float _jointMassScale = 4.5f;

    void Awake()
    {
        _player = GameObject.FindWithTag(Tags.PLAYER);
        _playerController = _player.GetComponent<Controller>();
        _lr = _player.GetComponent<LineRenderer>();
        this._grappler = new Grappler(_weaponPrefab, _gunPoint, _maxAmmo);
        this.weapon = this._grappler;
    }

    public override void Shoot()
    {
        if (!weapon.expendsAmmo()) return;
        // On instantie en world space pour pas que le grapple bouge avec nous
        _shotGrapple = Instantiate(_projectilePrefab, weapon.gunPoint.transform.position, Quaternion.identity);
        _shotGrapple.GetComponent<Rigidbody>().linearVelocity = _gunPoint.transform.forward * _bulletSpeed;
        _shotGrappleBehavior = _shotGrapple.GetComponent<GrappleBehavior>();
        _shotGrappleBehavior.setGrappleEvent(onHook);
        // On active le line renderer
        _lr.positionCount = 2;
    }

    public override void ShootHeld()
    {
        // On a seulement besoin de gérer le dessinage de la corde ici
        // Tout le reste se fait par l'existence du projectile
        DrawRope();
    }

    public override void ReleaseShoot()
    {
        Destroy(_shotGrapple);
        _lr.positionCount = 0;
        StopSwing();
    }

    public void onHook(Vector3 grapplePoint)
    {
        // Callback quand notre grappin a aggrippé quelque chose de grappleable
        _hookPoint = grapplePoint;
        StartSwing();
    }

    private void StartSwing()
    {
        // return if not hooked
        if (_hookPoint == Vector3.zero) return;

        _joint = _player.gameObject.AddComponent<SpringJoint>();
        _joint.autoConfigureConnectedAnchor = false;
        _joint.connectedAnchor = _hookPoint;

        float distanceFromPoint = Vector3.Distance(_player.transform.position, _hookPoint);

        // the distance grapple will try to keep from grapple point. 
        _joint.maxDistance = distanceFromPoint * _jointMaxDistance;
        _joint.minDistance = distanceFromPoint * _jointMinDistance;

        // Set les values pour la physique du spring
        _joint.spring = _jointSpring;
        _joint.damper = _jointDamper;
        _joint.massScale = _jointMassScale;
    }

    public void StopSwing()
    {
        _lr.positionCount = 0;
        Destroy(_joint);
    }


    private void DrawRope()
    {
        // if not grappling, don't draw rope
        if (!_shotGrapple) return;

        _lr.SetPosition(0, weapon.gunPoint.transform.position);
        _lr.SetPosition(1, _shotGrapple.transform.position);
    }
}
