using UnityEngine;
using System.Collections;

public class AttachGameObject : MonoBehaviour
{
    public Transform attacherTransform;
    public Transform attachmentTransform;
    public Vector3 relativePosition;
    public Quaternion relativeRotation;

    private Transform previousParent;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    public void Attach()
    {
        Debug.Log($"Attach {attachmentTransform.gameObject.name} to {attacherTransform.gameObject.name}");
        previousParent = attachmentTransform.parent;
        previousPosition = attachmentTransform.localPosition;
        previousRotation = attachmentTransform.localRotation;
        attachmentTransform.SetParent(attacherTransform);
        attachmentTransform.localPosition = relativePosition;
        attachmentTransform.localRotation = relativeRotation;
    }

    public void Detach()
    {
        Debug.Log($"Detach {attachmentTransform.gameObject.name} from {attacherTransform.gameObject.name}");
        attachmentTransform.SetParent(previousParent);
        attachmentTransform.localPosition = previousPosition;
        attachmentTransform.localRotation = previousRotation;
    }
}
