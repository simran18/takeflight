using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public abstract class Callable : MonoBehaviour
{
    [Header("Callable")]
    public bool disableOnStart = true;
    public bool enableOnCall = true;
    public bool disableOnCallEnd = true;
    public bool autoRegisterSelf = true;

    # region inspector workflow changes
    // Working on improving the inspector interface of Callable - 
    private LogicController.OnCallEndHandler onCallEnd;
    [Header("Unity Events")]
    public UnityEvent OnCallEvent;
    public UnityEvent OnCallEndEvent;

    [Header("Triggers + Transitions")]
    [SerializeField] private CallTransition[] transitions = new CallTransition[0];
    

    // This class is just used as a way for transitions to be access via events
    [System.Serializable]
    private class CallTransition
    {
        public string nextCallName;
        private LogicController.OnCallEndHandler onCallEnd;

        // Call this from an event to transition to the callable named 'nextCallName'
        public void TriggerTransition() {
            onCallEnd(nextCallName);
        }
    }
    
    // num - The index of 'transitions' to trigger the transition for
    // End the current call and transitons to the next
    public void TriggerTransition(int num){
        transitions[num].TriggerTransition();
    }

    // nextCallName - The name of the next call to make
    // End the current call and transitons to the next
    public void TriggerTransition(string nextCallName) {
        onCallEnd(nextCallName);
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
