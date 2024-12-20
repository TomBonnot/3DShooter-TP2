using UnityEngine;

public class RangeEnemyBehavior : EnemyBehavior
{
    // References for the range attack system
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _gun;
    [SerializeField] private GameObject _gunPoint;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _topBody;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _reloadTime = 2f;
    // Delay before the first shot after spotting the player
    private float _delayOnSight;
    // Tracks the next allowable attack time
    private float _timeStamp;

    AntiGravityEnemy _antiGravityEnemy;
    
    protected override void Start()
    {
        base.Start();
        _timeStamp = 0f;
        _antiGravityEnemy = GetComponent<AntiGravityEnemy>();
        _delayOnSight = 1f;
    }
    void Update()
    {
        // Check if the player is within the enemy's sight
        if (IsPlayerInSight())
        {
            // Handle the initial delay when the player is first spotted
            ShootingDelay();
            // Rotate the enemy to face the player
            LookAtPlayer();

            // If the cooldown has passed, attack the player
            if (_timeStamp <= Time.time)
            {
                AttackPlayer();
            }
           
        }
        else
        {
            // Reset detection state if the player is no longer in sight
            _playerJustSpotted = false;
        }
    }

    private void ShootingDelay()
    {
        // Trigger a delay only when the player is first spotted
        if (!_playerJustSpotted)
        {
            // Mark the player as recently spotted
            _playerJustSpotted = true;
            // Set the initial attack delay
            _timeStamp = Time.time + _delayOnSight;
        }
    }

    private void LookAtPlayer()
    {
        // Determine the appropriate up direction based on gravity state
        Vector3 _upDirection = _antiGravityEnemy.IsGravityInverted ? Vector3.down : Vector3.up;

        // Rotate the enemy to face the player, considering the gravity direction
        _topBody.transform.LookAt(_player.transform, _upDirection);
    }

    protected override void AttackPlayer()
    {
        // Reset the cooldown for the next attack
        _timeStamp = Time.time + _reloadTime;
        // Shoot the projectile in the player direction
        GameObject _shotProjectile = Instantiate(_projectile, _gunPoint.transform.position, _gun.transform.rotation);
        Vector3 _projectileDirection = (_target.transform.position - _gunPoint.transform.position).normalized;
        _shotProjectile.GetComponent<Rigidbody>().linearVelocity = _projectileDirection * _bulletSpeed;
        Destroy(_shotProjectile, 3f);
    }
}
