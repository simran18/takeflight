using UnityEngine;
using UnityEngine.Events;
using System.Collections;


#if DEBUG
using System.Collections.Generic;
#endif


[System.Serializable]
public class OVRButtonEvent : UnityEvent<OVRInput.Button> { }

public static class OVRButtonCoroutine
{
#if DEBUG
    private static readonly Dictionary<OVRInput.Button, KeyCode> KEYBOARD_MAPPING = new Dictionary<OVRInput.Button, KeyCode>()
    {
        { OVRInput.Button.One, KeyCode.Alpha1 },
        { OVRInput.Button.Two, KeyCode.Alpha2 },
        { OVRInput.Button.Three, KeyCode.Alpha3 },
        { OVRInput.Button.Four, KeyCode.Alpha4 },
        { OVRInput.Button.PrimaryIndexTrigger, KeyCode.RightBracket },
        { OVRInput.Button.SecondaryIndexTrigger, KeyCode.LeftBracket },
        { OVRInput.Button.PrimaryHandTrigger, KeyCode.Quote },
        { OVRInput.Button.SecondaryHandTrigger, KeyCode.Colon },
    };
#endif

    // If you need to do something after the given button down, use "yield return WaitForButtonDown(...)";
    // otherwise, you can ignore the returned value.
    public static IEnumerator WaitForButtonDown(OVRInput.Button button, OVRButtonEvent callback, float timeout = -1)
    {
        var elapsedTime = 0f;
        while (timeout == -1f || elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            if (OVRInput.GetDown(button))
            {
                callback.Invoke(button);
                break;
            }
#if DEBUG
            if (KEYBOARD_MAPPING.TryGetValue(button, out var key) && Input.GetKeyDown(key))
            {
                callback.Invoke(button);
                break;
            }
#endif
            yield return new WaitForEndOfFrame();
        }
    }

    // If you need to do something after any of the given buttons, use "yield return WaitForButtonDown(...)";
    // otherwise, you can ignore the returned value.
    public static IEnumerator WaitForAnyButtonDown(OVRInput.Button[] buttons, OVRButtonEvent callback, float timeout = -1)
    {
        var elapsedTime = 0f;
        while (timeout == -1f || elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            foreach (var button in buttons)
            {
                if (OVRInput.GetDown(button))
                {
                    callback.Invoke(button);
                    break;
                }
#if DEBUG
                if (KEYBOARD_MAPPING.TryGetValue(button, out var key) && Input.GetKeyDown(key))
                {
                    callback.Invoke(button);
                    break;
                }
#endif
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
