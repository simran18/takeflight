using UnityEngine;
using System.Collections;

public abstract class Callable : MonoBehaviour
{
    [Header("Callable")]
    public bool disableOnStart = true;
    public bool enableOnCall = true;
    public bool disableOnCallEnd = true;
    public bool autoRegisterSelf = true;

    public abstract string Name { get; }

    protected virtual void Awake()
    {
        if (disableOnStart)
        {
            gameObject.SetActive(false);
        }
        if (autoRegisterSelf)
        {
            registerSelf();
        }
    }

    protected void registerSelf()
    {
        LogicController.Instance.AddCallable(this);
    }

    public void Call(LogicController.OnCallEndHandler onCallEnd)
    {
        if (enableOnCall)
        {
            gameObject.SetActive(true);
        }
        OnCall(HandleOnCallEnd(onCallEnd));
    }

    private LogicController.OnCallEndHandler HandleOnCallEnd(LogicController.OnCallEndHandler handler)
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

    protected abstract void OnCall(LogicController.OnCallEndHandler onCallEnd);
}
