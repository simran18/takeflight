using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaunchPad : MonoBehaviour
{
    private Collider triggerVolume;

    [Tooltip("True if this trigger should launch the player into flight")]
    public bool canLaunch;
    [Tooltip("True if this trigger should allow the player to land from flight")]
    public bool canLand;
    public float launchSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        triggerVolume = GetComponent<Collider>();
        triggerVolume.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        FlightController flightController = other.GetComponent<FlightController>();
        if (flightController != null) {
            if (flightController.flightEnabled && canLand) {
                flightController.Land();
            } else if (!flightController.flightEnabled && canLaunch) {
                flightController.Launch(Vector3.up * launchSpeed);
            }
        }
    }
}
