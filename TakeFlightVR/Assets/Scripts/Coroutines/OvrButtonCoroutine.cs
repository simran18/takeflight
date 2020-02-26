using UnityEngine;
using UnityEngine.Events;
using System.Collections;


[System.Serializable]
public class OVRButtonEvent : UnityEvent<OVRInput.Button> { }

public static class OVRButtonCoroutine
{
    // If you need to do something after the given button down, use "yield return WaitForButtonDown(...)";
    // otherwise, you can ignore the returned value.
    public static IEnumerator WaitForButtonDown(OVRInput.Button button, OVRButtonEvent callback, float timeout = -1)
    {
        var elapsedTime = 0f;
        while (timeout == -1 || elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            if (OVRInput.GetDown(button))
            {
                callback.Invoke(button);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    // If you need to do something after any of the given buttons, use "yield return WaitForButtonDown(...)";
    // otherwise, you can ignore the returned value.
    public static IEnumerator WaitForAnyButtonDown(OVRInput.Button[] buttons, OVRButtonEvent callback, float timeout = -1)
    {
        var elapsedTime = 0f;
        while (timeout == -1 || elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            foreach (var button in buttons)
            {
                if (OVRInput.GetDown(button))
                {
                    callback.Invoke(button);
                    break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
