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
	public OVRInput.Button increaseDistanceButton = OVRInput.Button.Three;
    public OVRInput.Button decreaseDistanceButton = OVRInput.Button.Four;
	
	public Vector3 positionOffset = new Vector3(3, 0, 0);
	public float distanceMultiplier = 1.5f;

   // public OVRPlayerController ovrPlayerController;

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
        if (OVRInput.GetDown(nextButton)) {
            // Move to the next position
            int newIndex = GetLoopedIndex(1, targetGameobjects);
            currentIndex = newIndex;

            MoveToGameobject(targetGameobjects[newIndex]);

        }
        else if (OVRInput.GetDown(prevButton)) {
            // Move to the previous position
            int newIndex = GetLoopedIndex(-1, targetGameobjects);
            currentIndex = newIndex;

            MoveToGameobject(targetGameobjects[newIndex]);
        }
		
		if (OVRInput.GetDown(increaseDistanceButton)){
			positionOffset *= distanceMultiplier;
			MoveToGameobject(targetGameobjects[currentIndex]);
		} else if (OVRInput.GetDown(decreaseDistanceButton)){
			positionOffset /= distanceMultiplier;
			MoveToGameobject(targetGameobjects[currentIndex]);
		}
    }

    void MoveToGameobject(GameObject gameObject) {

        transform.position = gameObject.transform.position + positionOffset;
        //ovrPlayerController.Teleported = true;
    }

    int GetLoopedIndex(int offset, List<GameObject> list) {
        return (currentIndex + targetGameobjects.Count + offset) % targetGameobjects.Count;
    }
}
