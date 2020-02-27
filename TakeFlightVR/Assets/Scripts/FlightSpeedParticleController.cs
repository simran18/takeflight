using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSpeedParticleController : MonoBehaviour
{

    public Rigidbody rb;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        CurveParticles();
    }

    void CurveParticles() {
        var vel = ps.velocityOverLifetime;
        vel.orbitalX = rb.angularVelocity.x;
        vel.orbitalY = -rb.angularVelocity.y;
        vel.orbitalZ = rb.angularVelocity.z;
    }
}
