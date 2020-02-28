using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSpeedParticleController : MonoBehaviour
{

    public Rigidbody rb;
    public ParticleSystem ps;
    public Vector3 orbitalSpeedMultipliers = Vector3.one;
    public float particleSpeedMultiplier = 1;

    private FlightController flightController;

    // Start is called before the first frame update
    void Start()
    {
        flightController = GetComponentInParent<FlightController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flightController.flightEnabled && !ps.isPlaying) {
            ps.Play();
        } else if (!flightController.flightEnabled && ps.isPlaying) {
            ps.Stop();
        }

        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        CurveParticles();
        SetParticleSpeed();
    }

    // Curves the path of the particles based on which direction the rigidbody is moving and rotating
    void CurveParticles() {
        var velocityModule = ps.velocityOverLifetime;

        velocityModule.orbitalX = orbitalSpeedMultipliers.x * rb.velocity.y;
        velocityModule.orbitalY = orbitalSpeedMultipliers.x * rb.angularVelocity.y;
        velocityModule.orbitalZ = orbitalSpeedMultipliers.x * rb.angularVelocity.y;
    }

    void SetParticleSpeed() {
        var velocityModule = ps.velocityOverLifetime;

        velocityModule.z = particleSpeedMultiplier * -(rb.velocity.magnitude) + -30;
    }
}
