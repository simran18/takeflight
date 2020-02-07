using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class FlightController : MonoBehaviour
{
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    public OVRInput.Button calibrateButton = OVRInput.Button.Any;
    public OVRInput.Button moveButton = OVRInput.Button.PrimaryHandTrigger;

    public Vector3 leftZeroPosition;
    public Vector3 rightZeroPosition;

    public Vector3 leftFlightVector;
    public Vector3 rightFlightVector;

    public AnimationCurve deadZone = new AnimationCurve(new Keyframe(0.02f, 0), new Keyframe(0.3f, 1));

    public OVRPlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError("Start");
        Calibrate();
    }

    // Update is called once per frame
    void Update() {
        if (OVRInput.GetDown(calibrateButton)) {
            Debug.LogError("Caliibrating");
            Calibrate();
        }
        CalculateFlightVectors();

        if (OVRInput.Get(moveButton)) {
            Fly();
        }
    }

    void Calibrate() {
        leftZeroPosition = OVRInput.GetLocalControllerPosition(leftController);
        rightZeroPosition = OVRInput.GetLocalControllerPosition(rightController);
    }

    void CalculateFlightVectors() {
        leftFlightVector = OVRInput.GetLocalControllerPosition(leftController) - leftZeroPosition;
        rightFlightVector = OVRInput.GetLocalControllerPosition(rightController) - rightZeroPosition;
        ApplyDeadzone(ref leftFlightVector);
        ApplyDeadzone(ref rightFlightVector);
    }

    void ApplyDeadzone(ref Vector3 vec) {
        if (vec.x < 0) {
            vec.x *= -1;
            vec.x = deadZone.Evaluate(vec.x);
            vec.x *= -1;
        } else {
            vec.x = deadZone.Evaluate(vec.x);
        }

        if (vec.y < 0) {
            vec.y *= -1;
            vec.y = deadZone.Evaluate(vec.y);
            vec.y *= -1;
        }
        else {
            vec.y = deadZone.Evaluate(vec.y);
        }
        if (vec.z < 0) {
            vec.z *= -1;
            vec.z = deadZone.Evaluate(vec.z);
            vec.z *= -1;
        }
        else {
            vec.z = deadZone.Evaluate(vec.z);
        }
    }

    void Fly() {
        Vector3 moveVector = leftFlightVector + rightFlightVector;
        playerController.GravityModifier = 0;
        playerController.transform.position += moveVector * Time.deltaTime;
        Debug.Log("movevector = " + moveVector);
    }
}
