using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidController : MonoBehaviour
{
    PlayerController _playerController;
    Rigidbody _rb;
    HingeJoint _hingeJoint;

    PlayerInputs _playerInputs;

    [SerializeField]
    float _angularVelocityThreshhold = .2f;

    [SerializeField]
    float _hingeJointMinLimit = -120;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _rb = GetComponent<Rigidbody>();
        _hingeJoint = GetComponent<HingeJoint>();
    }

    private void Start()
    {
        _playerInputs = _playerController.PlayerInputs;
    }

    private void Update()
    {
        //limit how far the lid can open according to input
        JointLimits limits = _hingeJoint.limits;
        limits.min = Mathf.Lerp(0, _hingeJointMinLimit, _playerInputs.Standard.Jump.ReadValue<float>());
        _hingeJoint.limits = limits;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_rb.angularVelocity.z > _angularVelocityThreshhold)
            return;

        Vector3 averageCollisionPoint = Vector3.zero;
        foreach (ContactPoint contactPoint in collision.contacts)
            averageCollisionPoint += contactPoint.point;
        averageCollisionPoint /= collision.contactCount;

        _playerController.LidCollision(averageCollisionPoint);
    }
}
