using UnityEngine;
using System.Collections.Generic;

public class CallbackBranches : MonoBehaviour
{
    public delegate void OnCallEndHandler(string nextCall);

    public Callable[] branches;
    public Callable startPoint;

    private Dictionary<string, Callable> branchDict;
    private Callable nextCall;

    public event OnCallEndHandler OnCallEnd;

    public CallbackBranches()
    {
        this.OnCallEnd = HandleOnCallEnd;
    }

    void Start()
    {
        branches = branches == null ? new Callable[0] : branches;
        branchDict = new Dictionary<string, Callable>(branches.Length);
        foreach (Callable c in branches)
        {
            branchDict[c.Name] = c;
        }
        nextCall = startPoint;
    }

    private void Update()
    {
        if (nextCall != null)
        {
            nextCall.Call(this.OnCallEnd);
            nextCall = null;
        }
    }

    private void HandleOnCallEnd(string nextCall)
    {
        Debug.Assert(branchDict.ContainsKey(nextCall));
        this.nextCall = branchDict[nextCall];
    }
}
