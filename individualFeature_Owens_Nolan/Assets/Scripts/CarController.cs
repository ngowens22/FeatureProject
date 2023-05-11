using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteeringAngle;
    private float currentBrakeForce;
    
    public bool isBraking;
    public bool isDrifting;

    [Header("Forces")]
    [SerializeField] public float motorForce;
    [SerializeField] public float brakeForce;
    [SerializeField] private float maxSteeringAngle;


    [Header("Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [Header("Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        //drift
        if(Input.GetKey(KeyCode.LeftShift))
        {
            //forward
            WheelFrictionCurve ForFrictionCurve = frontLeftWheelCollider.forwardFriction;
            ForFrictionCurve.stiffness = .5f;
            backLeftWheelCollider.forwardFriction = ForFrictionCurve;
            backRightWheelCollider.forwardFriction = ForFrictionCurve;

            //sideways
            WheelFrictionCurve SidFrictionCurve = frontLeftWheelCollider.sidewaysFriction;
            SidFrictionCurve.stiffness = .5f;
            backLeftWheelCollider.sidewaysFriction = SidFrictionCurve;
            backRightWheelCollider.sidewaysFriction = SidFrictionCurve;
        }
        else
        {
            //forward
            WheelFrictionCurve ForFrictionCurve = frontLeftWheelCollider.forwardFriction;
            ForFrictionCurve.stiffness = 3f;
            backLeftWheelCollider.forwardFriction = ForFrictionCurve;
            backRightWheelCollider.forwardFriction = ForFrictionCurve;

            //sideways
            WheelFrictionCurve SidFrictionCurve = frontLeftWheelCollider.sidewaysFriction;
            SidFrictionCurve.stiffness = 3f;
            backLeftWheelCollider.sidewaysFriction = SidFrictionCurve;
            backRightWheelCollider.sidewaysFriction = SidFrictionCurve;
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBrakeForce = isBraking ? brakeForce : 0f;
        Brake();
    }

    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void Brake()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        backLeftWheelCollider.brakeTorque = currentBrakeForce;
        backRightWheelCollider.brakeTorque = currentBrakeForce;
    }


}
