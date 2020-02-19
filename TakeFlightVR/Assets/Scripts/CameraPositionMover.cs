using UnityEngine;
 using System.Collections;
 
 public class CameraPos : MonoBehaviour {
 
     //Must be public
     public void ChangePositionStoryBook() {
        // update position based on unity project
        transform.position = new Vector3(-50f, 3f, 0f);
     }
 
 }