using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    ParticleSystem _particleSystem;
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_particleSystem.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
