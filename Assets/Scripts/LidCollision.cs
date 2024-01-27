using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidCollision : MonoBehaviour
{
    PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colliding w/ {collision.gameObject}");

        Vector3 averageCollisionPoint = Vector3.zero;
        foreach (ContactPoint contactPoint in collision.contacts)
            averageCollisionPoint += contactPoint.point;
        averageCollisionPoint /= collision.contactCount;

        _playerController.LidCollision(averageCollisionPoint);
    }
}
