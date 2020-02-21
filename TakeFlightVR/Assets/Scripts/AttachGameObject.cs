using UnityEngine;
using System.Collections;

public class AttachGameObject : MonoBehaviour
{
    public Transform attachmentTransform;

    // Update is called once per frame
    void Update()
    {
        attachmentTransform.position = transform.position;
    }
}
