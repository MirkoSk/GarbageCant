using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCaster : MonoBehaviour
{
    static float _slowRotationSpeed = 100, _mediumRotationSpeed = 500, _fastRotationSpeed = 1000;
    static float _slowWindForce = 150, _mediumWindForce = 600, _fastWindForce = 1000;

    enum FanSpeed { SLOW, MEDIUM, FAST }

    float _myRotationSpeed, _myWindForce;

    [SerializeField]
    FanSpeed _myFanSpeed = FanSpeed.SLOW;

    [SerializeField]
    Vector3 _windDirection = Vector3.forward;


    private void Awake()
    {
        _windDirection.Normalize();

        switch (_myFanSpeed)
        {
            case (FanSpeed.SLOW):
            {
                    _myRotationSpeed = _slowRotationSpeed;
                    _myWindForce = _slowWindForce;
                    break;
                }
            case (FanSpeed.MEDIUM):
                {
                    _myRotationSpeed = _mediumRotationSpeed;
                    _myWindForce = _mediumWindForce;
                    break;
                }
            case (FanSpeed.FAST):
                {
                    _myRotationSpeed = _fastRotationSpeed;
                    _myWindForce = _fastWindForce;
                    break;
                }
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _myRotationSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        other.GetComponentInParent<Rigidbody>().AddForce(transform.TransformDirection(_windDirection) *_myWindForce, ForceMode.Force);
    }
}
