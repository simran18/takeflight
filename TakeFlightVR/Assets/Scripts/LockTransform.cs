using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Keeps a gameobject's position and rotation from changing during gameplay
// Currently used to keep VR hands in correct place
public class LockTransform : MonoBehaviour
{
    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    // Start is called before the first frame update
    void Start()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
    }
}
