using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField]
    float _movementForce = 1, _launchForce = 1, _airControlModifier = .1f;

    [SerializeField]
    Collider _trashCanCollider;

    [SerializeField]
    LayerMask _airbornCheckMask = -1;

    [SerializeField]
    GameObject _lid;
    Quaternion _lidStartRot, _lidGoalRot;

    public bool Airborne {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _lidStartRot = _lid.transform.localRotation;
        _lidGoalRot = _lidStartRot * Quaternion.Euler(Vector3.right * 120);
    }

    private void Update()
    {
        if (Physics.CheckCapsule(
            _trashCanCollider.bounds.center,
            _trashCanCollider.ClosestPoint(_trashCanCollider.bounds.center - Vector3.up) * 1.01f,
            .2f,
            _airbornCheckMask
        ))
            Airborne = false;
        else
            Airborne = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = true;
            StopAllCoroutines();
            StartCoroutine(AnimateLid(false));
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            foreach (Collider col in _lid.GetComponentsInChildren<Collider>())
                col.enabled = false;
            StopAllCoroutines();
            StartCoroutine(AnimateLid(true));
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


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movementDirection += transform.right;
        if (Input.GetKey(KeyCode.S))
            movementDirection -= transform.right;
        if (Input.GetKey(KeyCode.A))
            movementDirection += Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * -Vector3.forward;
        if (Input.GetKey(KeyCode.D))
            movementDirection += Quaternion.AngleAxis(transform.localEulerAngles.y, Vector3.up) * Vector3.forward;

        movementDirection.Normalize();

        if (Airborne)
            movementDirection *= _airControlModifier;
        //_rb.AddForce(movementDirection * _movementForce * Time.fixedDeltaTime, ForceMode.Force);
        _rb.AddTorque(movementDirection * _movementForce * Time.fixedDeltaTime, ForceMode.Force);
    }

    public void LidCollision(Vector3 contactPos)
    {
        _rb.AddForce((transform.position - contactPos).normalized * _launchForce * Time.fixedDeltaTime, ForceMode.Impulse);
    }
}
