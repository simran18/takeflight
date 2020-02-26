using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class LogicBranch : MonoBehaviour
{
    [Header("Callable")]
    public bool disableOnStart = true;
    public bool enableOnCall = true;
    public bool disableOnCallEnd = true;
    public bool autoRegisterSelf = true;

    # region inspector workflow changes
    // Working on improving the inspector interface of Callable - 
    [Header("Unity Events")]
    public UnityEvent<string> OnCallEvent;
    public UnityEvent<string> OnCallEndEvent;

    [Header("Triggers + Transitions")]
    [SerializeField] private CallTransition[] transitions = new CallTransition[0];
    

    // This class is just used as a way for transitions to be access via events
    [System.Serializable]
    private class CallTransition
    {
        public string nextCallName;
    }
    
    // num - The index of 'transitions' to trigger the transition for
    // End the current call and transitons to the next
    public void TriggerTransition(int num){
        TriggerTransition(transitions[num].nextCallName);
    }

    // nextCallName - The name of the next call to make
    // End the current call and transitons to the next
    public void TriggerTransition(string nextCallName) {
        MoveToBranch(nextCallName);
    }
    # endregion

    public abstract string Name { get; }

    protected virtual void Awake()
    {
        if (autoRegisterSelf)
        {
            RegisterSelf();
        }
        if (disableOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    protected void RegisterSelf()
    {
        LogicController.Instance.AddCallable(this);
    }

    public void Call()
    {
        if (enableOnCall)
        {
            gameObject.SetActive(true);
        }
        OnCallEvent.Invoke(Name);
        OnCall();
    }

    protected abstract void OnCall();

    protected void MoveToBranch(string next)
    {
        OnCallEndEvent.Invoke(Name);
        if (disableOnCallEnd)
        {
            gameObject.SetActive(false);
        }
        LogicController.Instance.MoveToBranch(next);
    }
}
