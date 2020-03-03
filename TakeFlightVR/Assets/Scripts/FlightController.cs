using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class FlightController : MonoBehaviour
{
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public GameObject headsetGO;
    public GameObject leftControllerGO;
    public GameObject rightControllerGO;
    public Rigidbody rb;
    [Space]
    public bool allowSpin = true;
    public float spinSpeed = 1;
    public float strafeSpeed = 1;
    [Space]
    public float moveSpeed = 5;
    public float minSpeed = 3;
    public float maxSpeed = 50;
    public float desiredSpeed = 20;
    [Space]

    public OVRInput.Button calibrateButton = OVRInput.Button.One | OVRInput.Button.Two;
    public OVRInput.Button toggleButton = OVRInput.Button.Three | OVRInput.Button.Four;
    public OVRInput.Button moveButton = OVRInput.Button.PrimaryHandTrigger;
    [Space]
    public float handZeroHeight;
    public Vector3 flightVector;
    public float steeringAngle;
    public Vector3 bodyForwardVector;
    public Vector3 avgControllerPosition;
    public Vector3 avgControllerLocalPosition;
    [Space]
    public OVRPlayerController ovrPlayerController;
    public CharacterController characterController;
    [Space]
    public bool flightEnabled = true;
    public bool hasToggledFlightThisFrame = false;
    void EnableFlight() {
        if (hasToggledFlightThisFrame) { return; }

        characterController.enabled = false;
        ovrPlayerController.GravityModifier = 0;
        rb.isKinematic = false;
        flightEnabled = true;
        Calibrate();
        hasToggledFlightThisFrame = true;
    }

    void DisableFlight() {
        DisableFlight(initialGravityModifier);
    }

    void DisableFlight(float newGravityModifier) {
        if (hasToggledFlightThisFrame) { return; }

        characterController.enabled = true;
        ovrPlayerController.GravityModifier = newGravityModifier;
        rb.isKinematic = true;
        flightEnabled = false;
        hasToggledFlightThisFrame = true;
    }


    public void ToggleFlight() {
        if (flightEnabled) {
            DisableFlight();
        } else {
            EnableFlight();
        }
    }

    public void Launch(Vector3 launchVector) {
        EnableFlight();
        rb.velocity += launchVector;
    }

    public void Land() {
        rb.velocity = Vector3.zero;
        DisableFlight();
    }

    private float initialGravityModifier;

    // Start is called before the first frame update
    void Start() {
        initialGravityModifier = ovrPlayerController.GravityModifier;

        rb = GetComponent<Rigidbody>();
        if (headsetGO == null) {
            Debug.LogError("Headset variable not assigned");
        }

        // Initializes the toggle state
        flightEnabled = !flightEnabled;
        ToggleFlight();
    }

    // Update is called once per frame
    void Update() {
        hasToggledFlightThisFrame = false;
        if (OVRInput.GetDown(toggleButton)) { ToggleFlight(); }
        if (OVRInput.GetDown(calibrateButton)) { Calibrate(); }
        if (ovrPlayerController.Teleported) { DisableFlight(); }
        if (!flightEnabled) { return; }
        

        UpdatePositionalInputInfo();
        if (allowSpin) {
            TwistPlayArea();
        } else {
            strafePlayArea();
        }

        float downwardAlignment = Vector3.Dot(Vector3.down, flightVector.normalized);
        // Don't apply gravity when the controllers are held flat, but apply more if pointed away
        if (rb.useGravity) {
            float antiGravityMultiplier = (1 - Mathf.Abs(downwardAlignment));
            float gravityBooster = Mathf.Pow(Mathf.Abs(downwardAlignment), 2f);
            rb.AddForce(-Physics.gravity * antiGravityMultiplier * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce(Physics.gravity * gravityBooster * Time.deltaTime, ForceMode.VelocityChange);
        }

        rb.velocity += flightVector * moveSpeed/10 * Time.deltaTime;
       
        // Redirect a portion of the velocity towards where the controllers are pointing
        rb.velocity = Vector3.Lerp(flightVector, rb.velocity, 0.3f).normalized * rb.velocity.magnitude;


        // Speed Boost
        float primarySqueeze = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        primarySqueeze *= Mathf.Abs(desiredSpeed - minSpeed);
        // Air Brake
        float secondarySqueeze = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        secondarySqueeze *= Mathf.Abs(maxSpeed - desiredSpeed);
        // Modify speed
        ManageSpeed((primarySqueeze - secondarySqueeze));
    }

    void ManageSpeed(float desiredSpeedModifier = 0) {
        // Make sure that velocity always has a direction
        if (rb.velocity == Vector3.zero) {
            rb.velocity = headsetGO.transform.forward * Mathf.Epsilon;
        }

        // Enforce the minimum speed
        if (rb.velocity.magnitude < minSpeed) {
            rb.velocity = rb.velocity.normalized * minSpeed;
        }
        // Enforce the maximum speed
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // Try to push the speed towards the desired speed
        float differenceFromDesiredSpeed = desiredSpeed - rb.velocity.magnitude;
        differenceFromDesiredSpeed += desiredSpeedModifier;
        rb.velocity += rb.velocity.normalized * Mathf.Lerp(0, differenceFromDesiredSpeed, 0.95f) * Time.deltaTime;
    }

    void Calibrate() {
        UpdatePositionalInputInfo();
        handZeroHeight = avgControllerLocalPosition.y;
        Debug.Log("Calibrating() to " + handZeroHeight);
    }


    void UpdatePositionalInputInfo() {
        avgControllerLocalPosition = (leftControllerGO.transform.localPosition + rightControllerGO.transform.localPosition) / 2f;
        avgControllerPosition = (leftControllerGO.transform.position + rightControllerGO.transform.position) / 2f;
        CalculateBodyForwardVector();
        CalculateSteeringAngle();
        CalculateFlightVector();
    }
    void CalculateFlightVector() {
        const bool USE_POINTER_FLIGHT_VECTOR = false; // otherwise uses positional vectors instead of the direction that the controllers are pointing

        if (USE_POINTER_FLIGHT_VECTOR) {
            flightVector = (leftControllerGO.transform.forward + rightControllerGO.transform.forward);
        } else {
            const float CLIMB_SPEED_MULTIPLIER = 3.0f;
            flightVector = new Vector3(bodyForwardVector.x, -(handZeroHeight - avgControllerLocalPosition.y) * CLIMB_SPEED_MULTIPLIER, bodyForwardVector.z);
        }
        flightVector.Normalize();

        const float HEADSET_FLIGHT_VECTOR_BIAS = 0.5f; // How much should the look direction impact steering? 0 doesn't use the headset, 1 uses only the headset
        flightVector = Vector3.Lerp(flightVector, headsetGO.transform.forward, HEADSET_FLIGHT_VECTOR_BIAS);
    }

    void CalculateSteeringAngle() {
        float horizontalControllerDistance = Vector3.Distance(rightControllerGO.transform.position - Vector3.up * rightControllerGO.transform.position.y,
            leftControllerGO.transform.position - Vector3.up * leftControllerGO.transform.position.y);
        float controllerHeightDifference = leftControllerGO.transform.position.y - rightControllerGO.transform.position.y;

        steeringAngle = Mathf.Atan2(controllerHeightDifference, horizontalControllerDistance);
        // Debug.Log(steeringAngle);
    }

    void CalculateBodyForwardVector() {
        const float OFFSET_BACK = 2.0f;
        Vector3 headsetPosition = headsetGO.transform.position;// - headsetGO.transform.TransformPoint(-headsetGO.transform.forward * OFFSET_BACK);
        bodyForwardVector = (avgControllerPosition - headsetPosition);
        bodyForwardVector -= bodyForwardVector.y * Vector3.up;

        bodyForwardVector.Normalize();
    }

    float GetControllerSpreadBonus() {
        float controllerDistance = Vector3.Distance(leftControllerGO.transform.position, rightControllerGO.transform.position);
        Debug.Log("controllerDistance = " + controllerDistance);
        float spreadRedirectionBonus = Mathf.Clamp(controllerDistance / 10f, 0, 0.2f);
        spreadRedirectionBonus *= 2;

        // Redirect less if the controllers are not flat
        float downwardAlignment = Vector3.Dot(Vector3.down, flightVector.normalized);
        spreadRedirectionBonus = Mathf.Lerp(spreadRedirectionBonus * Mathf.Abs(downwardAlignment), spreadRedirectionBonus, 0.5f);

        return spreadRedirectionBonus;
    }

    void TwistPlayArea() {
        ovrPlayerController.transform.Rotate(Vector3.up, steeringAngle * spinSpeed);
    }

    void strafePlayArea() {
        float mag = rb.velocity.magnitude;
        rb.velocity += -Vector3.Cross(bodyForwardVector.normalized, Vector3.up) * steeringAngle * strafeSpeed;

        rb.velocity = rb.velocity.normalized * mag;
    }

    private void DEBUG_DrawVector(Vector3 pos, Vector3 vec, Color color) {
        Debug.DrawLine(pos, pos + vec, color);
    }
}
