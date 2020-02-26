using UnityEngine;
using System.Collections;

public static class MoveCoroutine
{
    public static IEnumerator Move(Transform transform, Vector3 targetPosition, float movingTime)
    {
        var elapsedTime = 0f;
        var startingPosition = transform.localPosition;
        while (elapsedTime < movingTime)
        {
            var timeRatio = elapsedTime / movingTime;
            transform.localPosition = Vector3.Lerp(startingPosition, targetPosition, timeRatio);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = targetPosition;
    }
}
