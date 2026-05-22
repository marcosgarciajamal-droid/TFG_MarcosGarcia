using UnityEngine;

public class PlatformTilt : MonoBehaviour
{

    public bool activate  = false; 
    public float tiltAngle = 30f; // Maximum tilt angle in degrees
    public float tiltSpeed = 2f; // Speed of tilting

    [Header("Sensor Settings")]
    public LEDIndicator led;

    public float sensorTolerance = 2f; // Tolerance for sensor input to activate tilting
    private Quaternion initialRotation; // Initial rotation of the platform
    private Quaternion targetRotation; // Target rotation for tilting

    private bool isNormalPosition = true; // Flag to track if the platform is in its normal position
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(-tiltAngle,0,0)); // Calculate the target rotation based on the tilt angle
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // Smoothly rotate back to the initial rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * tiltSpeed);
        }
        CheckSensor();
    }


    void CheckSensor()
    {
        float angleDifference = Quaternion.Angle(transform.rotation, targetRotation); // Calculate the angle difference between current rotation and target rotation
        if (angleDifference < sensorTolerance)
        {
            if (!isNormalPosition){
                isNormalPosition = true; 
                led.SetActive(true); // Change LED color to indicate normal position
            }
        }
        else
        {
            if (isNormalPosition){
                isNormalPosition = false;
                led.SetActive(false); // Change LED color to indicate tilted position
            }
        }
    }
}

//https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Quaternion.Euler.html
// https://discussions.unity.com/t/quaternion-euler/871856