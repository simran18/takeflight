using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class TeleportButton : MonoBehaviour
{
    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    public OVRInput.Button nextButton = OVRInput.Button.One;
    public OVRInput.Button prevButton = OVRInput.Button.Two;

    public OVRPlayerController ovrPlayerController;

    public List<GameObject> targetGameobjects;
    public int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (targetGameobjects == null) {
            targetGameobjects = new List<GameObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(nextButton) || Input.GetKeyDown("left")) {
            // Move to the next position
            int newIndex = GetLoopedIndex(1, targetGameobjects);
            currentIndex = newIndex;

            MoveToGameobject(targetGameobjects[newIndex]);

        }
        else if (OVRInput.GetDown(prevButton) || Input.GetKeyDown("right")) {
            // Move to the previous position
            int newIndex = GetLoopedIndex(-1, targetGameobjects);
            currentIndex = newIndex;

            MoveToGameobject(targetGameobjects[newIndex]);
        }
    }

    void MoveToGameobject(GameObject gameObject) {

        //transform.position = gameObject.transform.position;
        transform.position = new Vector3(0, currentIndex, 0);
        ovrPlayerController.Teleported = true;
    }

    int GetLoopedIndex(int offset, List<GameObject> list) {
        return (currentIndex + targetGameobjects.Count + offset) % targetGameobjects.Count;
    }
}
