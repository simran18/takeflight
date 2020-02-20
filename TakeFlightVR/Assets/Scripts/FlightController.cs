﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class FlightController : MonoBehaviour
{
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
    public GameObject headset;
    public GameObject leftControllerGO;
    public GameObject rightControllerGO;
    public Rigidbody rb;

    public float moveSpeed = 5;
    float speedLimit = 50;

    public OVRInput.Button calibrateButton = OVRInput.Button.One | OVRInput.Button.Two;
    public OVRInput.Button toggleButton = OVRInput.Button.Three | OVRInput.Button.Four;
    public OVRInput.Button moveButton = OVRInput.Button.PrimaryHandTrigger;

    public Vector3 leftZeroPosition;
    public Vector3 rightZeroPosition;

    public Vector3 leftFlightVector;
    public Vector3 rightFlightVector;

    public OVRPlayerController ovrPlayerController;
    public CharacterController characterController;


    public bool flightEnabled = true;
    void EnableFlight() {
        characterController.enabled = false;
        ovrPlayerController.GravityModifier = 0;
        rb.isKinematic = false;
        flightEnabled = true;
    }

    void DisableFlight() {
        characterController.enabled = true;
        ovrPlayerController.GravityModifier = 0.1f;
        rb.isKinematic = true;
        flightEnabled = false;
    }

    public void ToggleFlight() {
        if (flightEnabled) {
            DisableFlight();
        } else {
            EnableFlight();
        }
    }


    // Start is called before the first frame update
    void Start() {
        
        rb = GetComponent<Rigidbody>();
        if (headset == null) {
            Debug.LogError("Headset variable not assigned");
        }
        Calibrate();

        // Initializes the toggle state
        flightEnabled = !flightEnabled;
        ToggleFlight();
    }

    // Update is called once per frame
    void Update() {
        if (OVRInput.GetDown(toggleButton)) {
            ToggleFlight();
        }

        if (!flightEnabled) {
            return;
        }

        if (OVRInput.GetDown(calibrateButton)) {
            Calibrate();
        }

        CalculateFlightVectors();
        
        Vector3 moveVector = leftFlightVector + rightFlightVector;

        float downwardAlignment = Vector3.Dot(Vector3.down, moveVector.normalized);
        // Don't apply gravity when the controllers are held flat, but apply more if pointed away
        if (rb.useGravity) {
            float antiGravityMultiplier = (1 - Mathf.Abs(downwardAlignment));
            float gravityBooster = Mathf.Pow(Mathf.Abs(downwardAlignment), 2f);
            rb.AddForce(-Physics.gravity * antiGravityMultiplier * Time.deltaTime, ForceMode.VelocityChange);
            rb.AddForce(Physics.gravity * gravityBooster * Time.deltaTime, ForceMode.VelocityChange);
        }

        rb.velocity += moveVector * moveSpeed/10 * Time.deltaTime;

        if (OVRInput.Get(moveButton)) {
            ApplySpeedBoost();
        }
        TwistPlayArea();
        
        // Redirect more if the controllers are further apart
        float controllerDistance = Vector3.Distance(leftControllerGO.transform.position, rightControllerGO.transform.position);
        Debug.Log("controllerDistance = " + controllerDistance);
        float spreadRedirectionBonus = Mathf.Clamp(controllerDistance / 10f, 0, 0.2f);
        spreadRedirectionBonus *= 2;
        // Redirect less if the controllers are not flat
        spreadRedirectionBonus = Mathf.Lerp(spreadRedirectionBonus * Mathf.Abs(downwardAlignment), spreadRedirectionBonus, 0.5f);

        // Redirect a portion of the velocity towards where the controllers are pointing
        rb.velocity = Vector3.Lerp(moveVector, rb.velocity, 0.7f).normalized * rb.velocity.magnitude;

        // Slow down if pointing upward, but less so if at a low speed
        /*
        float speedLimitFactor = Mathf.Clamp01((speedLimit - rb.velocity.magnitude) / speedLimit);
        float airBrakeAmount = Mathf.Clamp01(-downwardAlignment) * (1 - speedLimitFactor);
        airBrakeAmount = Mathf.Clamp01(airBrakeAmount - 0.5f);
        airBrakeAmount /= 10;
        rb.velocity -= rb.velocity.normalized * airBrakeAmount;
        */
    }

    void Calibrate() {
        return;
        Debug.Log("Calibrating!");
        leftZeroPosition = OVRInput.GetLocalControllerPosition(leftController);
        rightZeroPosition = OVRInput.GetLocalControllerPosition(rightController);
    }

    void CalculateFlightVectors() {
        //leftFlightVector = OVRInput.GetLocalControllerPosition(leftController) - leftZeroPosition;
        //leftFlightVector = OVRInput.GetLocalControllerRotation(leftController) * -Vector3.forward;


        //rightFlightVector = OVRInput.GetLocalControllerPosition(rightController) - rightZeroPosition;
        //rightFlightVector = OVRInput.GetLocalControllerRotation(rightController) * -Vector3.forward;


        //leftFlightVector = trackingSpace.transform.TransformVector(leftFlightVector);
        //rightFlightVector = trackingSpace.transform.TransformVector(rightFlightVector);
        //leftFlightVector = headset.transform.InverseTransformVector(leftFlightVector);
        //rightFlightVector = headset.transform.InverseTransformVector(rightFlightVector);

        leftFlightVector = leftControllerGO.transform.forward;
        rightFlightVector = rightControllerGO.transform.forward;
        //leftFlightVector *= -1;
        //rightFlightVector *= -1;
    }
    void ApplySpeedBoost() {
        Vector3 moveVector = leftFlightVector + rightFlightVector;
        float triggerSqueeze = OVRInput.Get(OVRInput.Axis1D.Any);
        //playerController.transform.position += moveVector * speed * triggerSqueeze * Time.deltaTime;

        // Give a speed bonus if the arms are spread apart
        float controllerDistance = Vector3.Distance(leftControllerGO.transform.position, rightControllerGO.transform.position);
        Debug.Log("controllerDistance = " + controllerDistance);
        float spreadBonus = Mathf.Clamp(controllerDistance, 1, 2);


        // Take measures to limit the player's speed
        float speedLimitFactor = ((speedLimit - rb.velocity.magnitude) / speedLimit);
        Debug.Log("speedLimitFactor = " + speedLimitFactor);

        // Move based off of how tightly the trigger is pulled + slow down as the player approaches the speed limit
        rb.velocity += moveVector * moveSpeed * triggerSqueeze * speedLimitFactor * spreadBonus * Time.deltaTime;

        // Clamp the movement speed to a speed limit
        if(rb.velocity.magnitude > speedLimit) {
            rb.velocity = rb.velocity.normalized * speedLimit;
        }

        //playerController.Teleported = true;
    }

    void TwistPlayArea() {
        float controllerHeightDifference = leftControllerGO.transform.position.y - rightControllerGO.transform.position.y;
        ovrPlayerController.transform.Rotate(Vector3.up, controllerHeightDifference);
    }
}
