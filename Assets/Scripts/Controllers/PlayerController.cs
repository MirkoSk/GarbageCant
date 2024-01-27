using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    Vector2 _moveInput;
    float _timeSinceLastInput;
    float _originalAngularDrag;
    bool _holdingJumpButton;

    HingeJoint _lidHingeJoint;


    [Header("Movement Controls")]
    [SerializeField]
    float _torqueMultiplier = 100f;
    [SerializeField]
    float _forceMultiplier = 100f;
    [SerializeField]
    float _airControlModifier = .1f;

    [Space]
    [SerializeField]
    float _maxAngularVelocity = 2f;
    [SerializeField]
    float _angularDragBumpDuration = 1f;
    [SerializeField]
    AnimationCurve _angularDragBumpIntensity;

    [Header("Lid Controls")]
    [SerializeField]
    float _launchForce = 1;

    [Header("References")]
    [SerializeField]
    GameObject _lid;
    [SerializeField]
    Collider _trashCanCollider;
    [SerializeField]
    LayerMask _airbornCheckMask = -1;



    public bool Airborne {get; private set; }

    public static PlayerInputs PlayerInputs { get; private set; }

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();

        _lidHingeJoint = _lid.GetComponentInChildren<HingeJoint>();
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }
    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalAngularDrag = _rb.angularDrag;
        _rb.maxAngularVelocity = _maxAngularVelocity;
    }

    private void Update()
    {
        // Get Input
        _moveInput = PlayerInputs.Standard.Move.ReadValue<Vector2>();
        _moveInput.Normalize();

        // Bump angularDrag after inputs
        _timeSinceLastInput += Time.deltaTime;
        if (_moveInput == Vector2.zero && _timeSinceLastInput <= _angularDragBumpDuration)
        {
            _rb.angularDrag = _originalAngularDrag * _angularDragBumpIntensity.Evaluate(_timeSinceLastInput / _angularDragBumpDuration);
        }
        else
        {
            _rb.angularDrag = _originalAngularDrag;
        }

        // Check if airborne
        if (Physics.CheckCapsule(
            _trashCanCollider.bounds.center,
            _trashCanCollider.ClosestPoint(_trashCanCollider.bounds.center - Vector3.up) * 1.01f,
            .2f,
            _airbornCheckMask
        ))
            Airborne = false;
        else
            Airborne = true;

        // Lid control
        if (PlayerInputs.Standard.Jump.ReadValue<float>() > 0 && !_holdingJumpButton)
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = true;
            JointMotor motor = _lidHingeJoint.motor;
            motor.force = 100;
            motor.targetVelocity = -1000;
            _lidHingeJoint.motor = motor;

            _holdingJumpButton = true;
        }
        else if (PlayerInputs.Standard.Jump.ReadValue<float>() == 0 && _holdingJumpButton)
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = false;
            JointMotor motor = _lidHingeJoint.motor;
            motor.force = 100;
            motor.targetVelocity = 100;
            _lidHingeJoint.motor = motor;

            _holdingJumpButton = false;
        }
    }

    void FixedUpdate()
    {
        // Project moveInput onto camera view
        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
        Vector3 moveVector = (forward * _moveInput.y + right * _moveInput.x).normalized;

        float currentMoveMultiplier = 1f;
        if (Airborne)
            currentMoveMultiplier *= _airControlModifier;

        if (_moveInput != Vector2.zero)
        {
            _rb.AddTorque(new Vector3(moveVector.z, moveVector.y, -moveVector.x) * _torqueMultiplier * currentMoveMultiplier * Time.deltaTime, ForceMode.Acceleration);
            _rb.AddForce(moveVector * _forceMultiplier * currentMoveMultiplier * Time.deltaTime, ForceMode.Acceleration);
            _timeSinceLastInput = 0f;
        }
    }

    public void LidCollision(Vector3 contactPos)
    {
        _rb.AddForce((transform.position - contactPos).normalized * _launchForce * Time.fixedDeltaTime 
            * PlayerInputs.Standard.Jump.ReadValue<float>(), ForceMode.Impulse);
    }

    public void Respawn(Transform spawnPoint)
    {
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
