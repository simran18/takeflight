using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargeBookOnInteract : MonoBehaviour
{
    public GameObject book;
    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public Vector3 targetScale;
    public float movingAndRotatingTime = 4f;
    public float scalingTime = 2f;

    private Transform bookTransform;

    void Awake()
    {
        if (book == null) {
            book = this.gameObject;
        }
        bookTransform = book.transform;
    }

    public void EnlargeBook()
    {
        StartCoroutine(MoveAndEnlarge());
    }

    IEnumerator MoveAndEnlarge()
    {
        yield return new WaitForSeconds(1f);
        var elapsedTime = 0f;
        var startingPosition = bookTransform.position;
        var startingRotation = bookTransform.rotation;
        while (elapsedTime < movingAndRotatingTime) {
            var timeRatio = elapsedTime / movingAndRotatingTime;
            bookTransform.position = Vector3.Lerp(startingPosition, targetPosition, timeRatio);
            bookTransform.rotation = Quaternion.Lerp(startingRotation, targetRotation, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bookTransform.position = targetPosition;
        bookTransform.rotation = targetRotation;
        elapsedTime = 0f;
        var startingScale = bookTransform.localScale;
        while (elapsedTime < scalingTime) {
            var timeRatio = elapsedTime / scalingTime;
            bookTransform.localScale = Vector3.Lerp(startingScale, targetScale, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bookTransform.localScale = targetScale;
    }
}
