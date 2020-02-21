using UnityEngine;
using System.Collections;

public abstract class Callable : MonoBehaviour
{
    [Header("Callable")]
    public bool disableOnStart = true;
    public bool enableOnCall = true;
    public bool disableOnCallEnd = true;

    public abstract string Name { get; }

    protected virtual void Awake()
    {
        if (disableOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    public void Call(CallbackBranches.OnCallEndHandler onCallEnd)
    {
        if (enableOnCall)
        {
            gameObject.SetActive(true);
        }
        OnCall(HandleOnCallEnd(onCallEnd));
    }

    private CallbackBranches.OnCallEndHandler HandleOnCallEnd(CallbackBranches.OnCallEndHandler handler)
    {
        return next =>
        {
            if (disableOnCallEnd)
            {
                gameObject.SetActive(false);
            }
            handler(next);
        };
    }

    protected abstract void OnCall(CallbackBranches.OnCallEndHandler onCallEnd);
}
