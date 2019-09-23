using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour

{
    public float ResultMilliCount;
    public int ResultSecondCount;
    public int ResultMinueCount;

    public GameObject ResultMinuteDisplay;
    public GameObject ResultSecondDisplay;
    public GameObject ResultMilliDisplay;

    public int MinuteCount;
    public int SecondCount;
    public float MilliCount;
    public string MilliDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject MilliBox;


    public int NeededCheckPoint = 1;

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public float currentMaxSpeed = 50;
    public float maxSpeed = 50;

    public Rigidbody rb;
    public Vector3 com;

    public string AxisFB;
    public string AxisRL;
    public string AxisBreak;

    public KeyCode Camera;

    public float BreakPower = 10;
    
    public GameObject camera1, camera2, camera3;

    public float AntiRoll = 5000f;

    public int Lap;

    private void Start()
    {
        //Time.timeScale = 1f;
        axleInfos[0].leftWheel.ConfigureVehicleSubsteps(5, 12, 15);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
    

    public void FixedUpdate()
    {

        //Camera
        if (Input.GetKeyUp(Camera))
        {
            if (camera1 != null)
            {
                if (camera1.activeInHierarchy == true)
                {
                    camera1.SetActive(false);
                    camera2.SetActive(true);
                    camera3.SetActive(false);

                    return;
                }

                if (camera2.activeInHierarchy == true)
                {
                    camera1.SetActive(false);
                    camera2.SetActive(false);
                    camera3.SetActive(true);

                    return;
                }

                if (camera3.activeInHierarchy == true)
                {
                    camera1.SetActive(true);
                    camera2.SetActive(false);
                    camera3.SetActive(false);


                    return;
                }
            }
        }

        float motor = maxMotorTorque * Input.GetAxis(AxisFB);
        float steering = maxSteeringAngle * Input.GetAxis(AxisRL);
        float brakeTorque = Mathf.Abs(Input.GetAxis(AxisBreak));

        foreach (AxleInfo axleInfo in axleInfos)
        {
            //Rollbar
            WheelHit hit;
            float travelL = 1.0f;
            float travelR = 1.0f;

            bool groundedL = axleInfo.leftWheel.GetGroundHit(out hit);
            if (groundedL)
                travelL = (-axleInfo.leftWheel.transform.InverseTransformPoint(hit.point).y - axleInfo.leftWheel.radius) / axleInfo.leftWheel.suspensionDistance;

            bool groundedR = axleInfo.rightWheel.GetGroundHit(out hit);
            if (groundedR)
                travelR = (-axleInfo.rightWheel.transform.InverseTransformPoint(hit.point).y - axleInfo.rightWheel.radius) / axleInfo.rightWheel.suspensionDistance;

            float antiRollForce = (travelL - travelR) * AntiRoll;

            if (groundedL)
                rb.AddForceAtPosition(axleInfo.leftWheel.transform.up * -antiRollForce, axleInfo.leftWheel.transform.position);

            if (groundedR)
            {
                rb.AddForceAtPosition(axleInfo.rightWheel.transform.up * -antiRollForce, axleInfo.rightWheel.transform.position);
            }

            //brake
            if (motor == 0)
            {
                brakeTorque = maxMotorTorque;
            }

            if (brakeTorque > 0.001)
            {
                brakeTorque = maxMotorTorque;
                motor = 0;
            }

            else
            {
                brakeTorque = 0;
            }

            axleInfo.leftWheel.brakeTorque = brakeTorque;
            axleInfo.rightWheel.brakeTorque = brakeTorque;

            //Movement

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            if (rb.velocity.magnitude > currentMaxSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }



            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

   
}

