using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBehavior : MonoBehaviour
{
    // Initial values when scene is loaded
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Rigidbody _initialRigidbody;

    // Dictionary to store initial local positions and rotations of all children
    private Dictionary<Transform, (Vector3 localPosition, Quaternion localRotation)> _childrenInitialStates;

    private void Start()
    {
        GameManager.Instance.OnReloadLevel += ResetState;

        // Save initial position
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _initialRigidbody = GetComponent<Rigidbody>();

        // Save initial local positions and rotations of all children
        _childrenInitialStates = new Dictionary<Transform, (Vector3, Quaternion)>();
        SaveChildrenStates(transform);
    }

    /*
    *   Recursively save states of all children
    */
    private void SaveChildrenStates(Transform parent)
    {
        foreach (Transform child in parent)
        {
            // Save the current child's state
            _childrenInitialStates[child] = (child.localPosition, child.localRotation);
            // Recursively save states of this child's children
            SaveChildrenStates(child);
        }
    }

    // Reset the entity to it's initial position and rotation
    private void ResetState()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation; 
        _initialRigidbody.linearVelocity = Vector3.zero;
        _initialRigidbody.angularVelocity = Vector3.zero;

        // Reset all children to their initial local positions and rotations
        foreach (var kvp in _childrenInitialStates)
        {
            Transform child = kvp.Key;
            (Vector3 localPosition, Quaternion localRotation) = kvp.Value;
            child.localPosition = localPosition;
            child.localRotation = localRotation;
        }
        Controller _controller = GetComponent<Controller>();
        _controller.ResetLastMoveDirection();
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnReloadLevel -= ResetState;
    }
}
