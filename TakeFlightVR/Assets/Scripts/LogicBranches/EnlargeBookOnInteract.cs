using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeBookOnInteract : Callable
{
    [Header("Parameters")]
    public GameObject book;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 targetScale;
    public float movingAndRotatingTime = 4f;
    public float scalingTime = 2f;

    private Transform bookTransform;
    private Quaternion targetRotationQuaternion;

    public override string Name => "EnlargeBookOnInteract";

    protected override void Awake()
    {
        if (book == null) {
            book = this.gameObject;
        }
        bookTransform = book.transform;
        targetRotationQuaternion = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);
        base.Awake();
    }

    protected override void OnCall(LogicController.OnCallEndHandler onCallEnd)
    {
        StartCoroutine(MoveAndEnlarge(onCallEnd));
    }

    IEnumerator MoveAndEnlarge(LogicController.OnCallEndHandler onCallEnd)
    {
        yield return new WaitForSeconds(1f);
        var elapsedTime = 0f;
        var startingPosition = bookTransform.localPosition;
        var startingRotation = bookTransform.localRotation;
        var startingScale = bookTransform.localScale;
        while (elapsedTime < movingAndRotatingTime) {
            var timeRatio = elapsedTime / movingAndRotatingTime;
            bookTransform.localPosition = Vector3.Lerp(startingPosition, targetPosition, timeRatio);
            bookTransform.localRotation = Quaternion.Lerp(startingRotation, targetRotationQuaternion, timeRatio);
            bookTransform.localScale = Vector3.Lerp(startingScale, targetScale, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bookTransform.localPosition = targetPosition;
        bookTransform.localRotation = targetRotationQuaternion;
        bookTransform.localScale = targetScale;
        onCallEnd("MomComesInside");
    }
}
