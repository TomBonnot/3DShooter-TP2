using System;
using UnityEngine;

public abstract class EntityBehavior : MonoBehaviour
{
    public event Action OnDeath;

    protected virtual void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
