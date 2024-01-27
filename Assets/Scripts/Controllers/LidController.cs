using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidController : MonoBehaviour
{
    PlayerController _playerController;
    HingeJoint _hingeJoint;

    PlayerInputs _playerInputs;
    float _lastFrameJumpInput;

    float _lastOpeningTimestamp;

    [SerializeField]
    float _hingeJointMinLimit = -120;

    [SerializeField]
    float _timeToleranceLidOpenend = .1f;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _hingeJoint = GetComponent<HingeJoint>();
    }

    private void Start()
    {
        _playerInputs = PlayerController.PlayerInputs;
    }

    private void Update()
    {
        if (_playerInputs.Standard.Jump.ReadValue<float>() - _lastFrameJumpInput > 0)
            _lastOpeningTimestamp = Time.timeSinceLevelLoad;

        //limit how far the lid can open according to input
        JointLimits limits = _hingeJoint.limits;
        limits.min = Mathf.Lerp(0, _hingeJointMinLimit, _playerInputs.Standard.Jump.ReadValue<float>());
        _hingeJoint.limits = limits;

        _lastFrameJumpInput = _playerInputs.Standard.Jump.ReadValue<float>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.timeSinceLevelLoad - _lastOpeningTimestamp >= _timeToleranceLidOpenend)
            return;

        Vector3 averageCollisionPoint = Vector3.zero;
        foreach (ContactPoint contactPoint in collision.contacts)
            averageCollisionPoint += contactPoint.point;
        averageCollisionPoint /= collision.contactCount;

        _playerController.LidCollision(averageCollisionPoint);
    }
}
