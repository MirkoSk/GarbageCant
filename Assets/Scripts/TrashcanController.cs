using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TrashcanController : MonoBehaviour
{
    [SerializeField] float torqueMultiplier = 100f;
    [SerializeField] float forceMultiplier = 100f;
    [SerializeField] float angularDragBumpDuration = 1f;
    [SerializeField] AnimationCurve angularDragBumpIntensity;
    [SerializeField] float maxAngularVelocity = 2f;

    new Rigidbody rigidbody;
    Vector3 inputVector;
    float timeSinceLastInput;
    float originalAngularDrag;



    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalAngularDrag = rigidbody.angularDrag;
        rigidbody.maxAngularVelocity = maxAngularVelocity;
    }

    private void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        inputVector = inputVector.normalized;

        timeSinceLastInput += Time.deltaTime;
        if (inputVector == Vector3.zero && timeSinceLastInput <= angularDragBumpDuration)
        {
            rigidbody.angularDrag = originalAngularDrag * angularDragBumpIntensity.Evaluate(timeSinceLastInput / angularDragBumpDuration);
        }
        else
        {
            rigidbody.angularDrag = originalAngularDrag;
        }
    }

    private void FixedUpdate()
    {
        if (inputVector != Vector3.zero)
        {
            rigidbody.AddTorque(new Vector3(-inputVector.y, 0f, inputVector.x) * torqueMultiplier * Time.deltaTime, ForceMode.Acceleration);
            rigidbody.AddForce(new Vector3(-inputVector.x, 0f, -inputVector.y) * forceMultiplier * Time.deltaTime, ForceMode.Acceleration);
            timeSinceLastInput = 0f;
        }
    }
}
