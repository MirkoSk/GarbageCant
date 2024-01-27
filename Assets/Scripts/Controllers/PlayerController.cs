using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    Vector2 _moveInput;
    float _timeSinceLastInput;
    float _originalAngularDrag;
    Quaternion _lidStartRot, _lidGoalRot;



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



    public bool Airborne {get; private set;}



    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalAngularDrag = _rb.angularDrag;
        _rb.maxAngularVelocity = _maxAngularVelocity;
        _lidStartRot = _lid.transform.localRotation;
        _lidGoalRot = _lidStartRot * Quaternion.Euler(Vector3.right * -140);
    }

    private void Update()
    {
        // Get Input
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = true;
            StopAllCoroutines();
            StartCoroutine(AnimateLid(false));
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = false;
            StopAllCoroutines();
            StartCoroutine(AnimateLid(true));
        }
    }

    void FixedUpdate()
    {
        // Project moveInput onto camera view
        Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
        Vector3 moveVector = (forward * _moveInput.y + right * _moveInput.x).normalized;

        Debug.DrawLine(transform.position, transform.position + moveVector, Color.red, 2f);

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



    IEnumerator AnimateLid(bool closeLid)
    {
        Quaternion startRot = _lid.transform.localRotation;
        Quaternion goalRot = _lidGoalRot;
        if (closeLid)
            goalRot = _lidStartRot;

        float timeElapsed = 0;
        float lerpDuration = .1f;
        while (timeElapsed < lerpDuration)
        {
            _lid.transform.localRotation = Quaternion.Lerp(startRot, goalRot, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _lid.transform.localRotation = goalRot;
    }

    public void LidCollision(Vector3 contactPos)
    {
        _rb.AddForce((transform.position - contactPos).normalized * _launchForce * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
