using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class LogicBranch : MonoBehaviour
{
    [Header("Callable")]
    [FormerlySerializedAs("disableOnStart")]
    public bool disableOnAwake = true;
    [FormerlySerializedAs("enableOnCall")]
    public bool enableOnCall = true;
    [FormerlySerializedAs("disableOnCallEnd")]
    public bool disableOnLeave = true;
    public bool autoRegisterSelf = true;

    [Header("Events")]
    public UnityEvent<string> OnCallEvent;
    public UnityEvent<string> OnCallEndEvent;

    public abstract string Name { get; }

    protected virtual void Awake()
    {
        if (autoRegisterSelf)
        {
            RegisterSelf();
        }
        if (disableOnAwake)
        {
            gameObject.SetActive(false);
        }
    }

    protected void RegisterSelf()
    {
        LogicController.Instance.AddBranch(this);
    }

    public void Call()
    {
        if (enableOnCall)
        {
            gameObject.SetActive(true);
        }
        OnCallEvent?.Invoke(Name);
        OnCall();
    }

    protected abstract void OnCall();

    protected void MoveToBranch(string next)
    {
        OnCallEndEvent?.Invoke(Name);
        if (disableOnLeave)
        {
            gameObject.SetActive(false);
        }
        LogicController.Instance.MoveToBranch(next);
    }
}
